using UnityEngine;
using System.Collections;
using Magicolo;

public class FogOfWar : MonoBehaviour {

	static FogOfWar instance;
	static FogOfWar Instance {
		get {
			if (instance == null) {
				instance = FindObjectOfType<FogOfWar>();
			}
			return instance;
		}
	}
	
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
	
	float UnitsToPixels {
		get {
			return (float)precision / 10;
		}
	}
	
	float PixelsToUnits {
		get {
			return (float)10 / precision;
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
		alphaMap = new float[width, height];
		
		foreach (TroopBase troop in TroopManager.GetTroops(NetworkController.CurrentPlayerId)) {
			Vector2 texturePosition = new Vector2(troop.transform.position.x * UnitsToPixels + (float)width / 2, troop.transform.position.z * UnitsToPixels + (float)height / 2);
		
			float pixelSightRadius = troop.sightRadius * UnitsToPixels;
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

	public static bool IsFogged(Vector3 point) {
		return false;
	}
}
