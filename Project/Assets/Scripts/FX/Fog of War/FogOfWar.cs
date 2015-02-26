using UnityEngine;
using System.Collections;
using Magicolo;

public class FogOfWar : MonoBehaviour {

	[SerializeField, PropertyField(typeof(MinAttribute), 1)]
	int precision;
	public int Precision {
		get {
			return precision;
		}
		set {
			precision = value;
				
			CreateTexture();
		}
	}
	
	[SerializeField, PropertyField]
	FilterMode filterMode = FilterMode.Bilinear;
	public FilterMode FilterMode {
		get {
			return filterMode;
		}
		set {
			filterMode = value;
			
			CreateTexture();
		}
	}
		
	Material _material;
	public Material material { get { return _material ? _material : (_material = renderer.sharedMaterial); } }
	
	public FogAgent[] fogAgents;
	
	public float UnitsToPixels {
		get {
			return (float)Precision / 10;
		}
	}
	
	public float PixelsToUnits {
		get {
			return (float)10 / Precision;
		}
	}
	
	int width;
	int height;
	Texture2D texture;
	float[,] alphaMap;
	
	void Awake() {
		CreateTexture();
	}
	
	void Update() {
		UpdateFow();
	}
	
	void UpdateFow() {
		UpdateAlphaMap();
		UpdateTexture();
	}
	
	void UpdateAlphaMap() {
		alphaMap = new float[width, height];
		
		foreach (TroopBase troop in TroopManager.GetTroops(NetworkController.CurrentPlayerId)) {
			RemoveFog(troop.transform.position, troop.sightRadius);
		}
		
		foreach (FogAgent fogAgent in fogAgents) {
			if (fogAgent.sightRadius > 0 && fogAgent.transform != null) {
				if (fogAgent.clearsFog) {
					RemoveFog(fogAgent.transform.position, fogAgent.sightRadius);
				}
				else {
					AddFog(fogAgent.transform.position, fogAgent.sightRadius);
				}
			}
		}
	}

	void UpdateTexture() {
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				texture.SetPixel(x, y, new Color(0, 0, 0, 1 - alphaMap[x, y]));
			}
		}
		
		texture.Apply();
	}
	
	void OnDestroy() {
		texture.Remove();
		material.mainTexture = Texture2D.blackTexture;
	}
	
	void CreateTexture() {
		if (Application.isPlaying) {
			width = (int)(transform.lossyScale.x * precision).Round();
			height = (int)(transform.lossyScale.z * precision).Round();
			texture = new Texture2D(width, height, TextureFormat.Alpha8, false);
			texture.filterMode = FilterMode;
			material.mainTexture = texture;
		}
	}

	void AddFog(Vector3 position, float sightRadius) {
		Vector2 texturePosition = WorldToPixel(position);
		
		float pixelSightRadius = sightRadius * UnitsToPixels;
		int xMin = (int)Mathf.Clamp(texturePosition.x - pixelSightRadius * 2, 0, width).Round();
		int xMax = (int)Mathf.Clamp(texturePosition.x + pixelSightRadius * 2, 0, width).Round();
		int yMin = (int)Mathf.Clamp(texturePosition.y - pixelSightRadius * 2, 0, height).Round();
		int yMax = (int)Mathf.Clamp(texturePosition.y + pixelSightRadius * 2, 0, height).Round();
			
		for (int x = xMin; x < xMax; x++) {
			for (int y = yMin; y < yMax; y++) {
				float distance = Vector2.Distance(new Vector2(x, y), texturePosition);
				alphaMap[x, y] += Mathf.Clamp01((distance - pixelSightRadius) / pixelSightRadius);
			}
		}
	}
	
	void RemoveFog(Vector3 position, float sightRadius) {
		Vector2 texturePosition = WorldToPixel(position);
		
		float pixelSightRadius = sightRadius * UnitsToPixels;
		int xMin = (int)Mathf.Clamp(texturePosition.x - pixelSightRadius * 2, 0, width).Round();
		int xMax = (int)Mathf.Clamp(texturePosition.x + pixelSightRadius * 2, 0, width).Round();
		int yMin = (int)Mathf.Clamp(texturePosition.y - pixelSightRadius * 2, 0, height).Round();
		int yMax = (int)Mathf.Clamp(texturePosition.y + pixelSightRadius * 2, 0, height).Round();
			
		for (int x = xMin; x < xMax; x++) {
			for (int y = yMin; y < yMax; y++) {
				float distance = Vector2.Distance(new Vector2(x, y), texturePosition);
				alphaMap[x, y] += 1 - Mathf.Clamp01((distance - pixelSightRadius) / pixelSightRadius);
			}
		}
	}
	
	Vector2 WorldToPixel(Vector3 worldPoint) {
		return new Vector2(worldPoint.x * UnitsToPixels + (float)width / 2, worldPoint.z * UnitsToPixels + (float)height / 2);
	}
	
	public bool IsFogged(Vector3 point, float alphaThreshold) {
		Vector2 pixel = WorldToPixel(point);
		float alpha = alphaMap[(int)pixel.x.Round(), (int)pixel.y.Round()];
		
		return alpha <= alphaThreshold;
	}
	
	public bool IsFogged(Vector3 point) {
		return IsFogged(point, 0);
	}
}
