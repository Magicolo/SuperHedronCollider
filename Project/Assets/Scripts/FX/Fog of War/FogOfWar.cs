using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.Collections;
using Magicolo;

public class FogOfWar : MonoBehaviour {

	[SerializeField, PropertyField(typeof(RangeAttribute), 1, 25)]
	int precision;
	public int Precision {
		get {
			return precision;
		}
		set {
			precision = Mathf.Clamp(value, 1, 25);
				
			if (Application.isPlaying) {
				CreateTexture();
				updateFow = true;
			}
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
			
			if (Application.isPlaying) {
				CreateTexture();
				updateFow = true;
			}
		}
	}
	
	public bool manualUpdate;
	
	public List<FogAgent> fogAgents;
	FogAgent[] troopFogAgents = new FogAgent[0];
	
	public bool updateFow { get; set; }
	
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
		
	Material _material;
	public Material material { get { return _material ? _material : (_material = renderer.sharedMaterial); } }
	
	int width;
	int height;
	Texture2D texture;
	float[,] currentAlphaMap;
	Color[] currentPixels;
	float deltaTime;
	Thread updateThread;
	
	void OnEnable() {
		updateThread = new Thread(new ThreadStart(UpdateFowAsync));
		updateThread.Start();
	}
	
	void OnDisable() {
		updateThread.Abort();
		updateThread = null;
	}
	
	void Awake() {
		CreateTexture();
	}
	
	void Start() {
		updateFow = true;
	}
	
	void Update() {
		deltaTime = Time.deltaTime;
		
		if (!manualUpdate) {
			updateFow = true;
		}
		
		UpdateAgents();
		texture.SetPixels(currentPixels);
		texture.Apply();
	}
	
	void UpdateAgents() {
		TroopBase[] troops = TroopManager.GetTroops(NetworkController.CurrentPlayerId);
		troopFogAgents = new FogAgent[troops.Length];
		
		for (int i = 0; i < troops.Length; i++) {
			TroopBase troop = troops[i];
			troopFogAgents[i] = new FogAgent(troop.transform, troop.sightRadius * 2);
		}
		
		foreach (FogAgent fogAgent in fogAgents) {
			fogAgent.Update();
		}
	}
	
	void UpdateFowAsync() {
		while (true) {
			if (updateFow) {
				try {
					updateFow = false;
					float[,] alphaMap = new float[width, height];
		
					UpdateAlphaMap(alphaMap);
					UpdateTexture(alphaMap);
		
					currentAlphaMap = alphaMap;
				}
				catch (System.Exception exception) {
					Logger.LogError(exception);
				}
			}
			
			Thread.Sleep((int)(deltaTime * 100));
		}
	}
	
	void UpdateAlphaMap(float[,] alphaMap) {
		FogAgent[] troopAgents = new FogAgent[troopFogAgents.Length];
		troopFogAgents.CopyTo(troopAgents, 0);
		
		for (int i = troopAgents.Length - 1; i >= 0; i--) {
			ModifyFog(alphaMap, troopAgents[i]);
		}
		
		for (int i = fogAgents.Count - 1; i >= 0; i--) {
			ModifyFog(alphaMap, fogAgents[i]);
		}
	}
	
	void UpdateTexture(float[,] alphaMap) {
		int xLength = alphaMap.GetLength(0);
		int yLength = alphaMap.GetLength(1);
		Color[] pixels = new Color[xLength * yLength];
		int pixelCounter = 0;
		
		for (int y = 0; y < yLength; y++) {
			for (int x = 0; x < xLength; x++) {
				pixels[pixelCounter].a = 1 - alphaMap[x, y];
				pixelCounter += 1;
			}
		}
		
		if (currentPixels.Length == pixels.Length) {
			currentPixels = pixels;
		}
	}
	
	void OnDestroy() {
		texture.Remove();
		material.mainTexture = Texture2D.blackTexture;
	}
	
	void ModifyFog(float[,] alphaMap, Vector3 position, float sightRadius, float strength) {
		Vector2 texturePosition = WorldToPixel(position);
		int xLength = alphaMap.GetLength(0);
		int yLength = alphaMap.GetLength(1);
		float pixelSightRadius = sightRadius * UnitsToPixels * 0.5F;
		
		int xMin = (int)Mathf.Clamp(texturePosition.x - pixelSightRadius * 2, 0, xLength).Round();
		int xMax = (int)Mathf.Clamp(texturePosition.x + pixelSightRadius * 2, 0, xLength).Round();
		int yMin = (int)Mathf.Clamp(texturePosition.y - pixelSightRadius * 2, 0, yLength).Round();
		int yMax = (int)Mathf.Clamp(texturePosition.y + pixelSightRadius * 2, 0, yLength).Round();
			
		for (int y = yMin; y < yMax; y++) {
			for (int x = xMin; x < xMax; x++) {
				float distance = Vector2.Distance(new Vector2(x, y), texturePosition);
				alphaMap[x, y] += (1 - Mathf.Clamp01((distance - pixelSightRadius) / pixelSightRadius)) * strength;
			}
		}
	}
	
	void ModifyFog(float[,] alphaMap, FogAgent fogAgent) {
		if (fogAgent != null) {
			ModifyFog(alphaMap, fogAgent.position, fogAgent.sightRadius, fogAgent.strength);
		}
	}
	
	void CreateTexture() {
		width = (int)(transform.lossyScale.x * precision).Round();
		height = (int)(transform.lossyScale.z * precision).Round();
		currentAlphaMap = new float[width, height];
		texture = new Texture2D(width, height, TextureFormat.Alpha8, false);
		texture.filterMode = filterMode;
		material.mainTexture = texture;
		
		currentPixels = new Color[width * height];
		for (int i = 0; i < currentPixels.Length; i++) {
			currentPixels[i] = Color.white;
		}
		texture.SetPixels(currentPixels);
	}

	Vector2 WorldToPixel(Vector3 worldPoint) {
		return new Vector2(worldPoint.x * UnitsToPixels + (float)width / 2, worldPoint.z * UnitsToPixels + (float)height / 2);
	}
	
	public void AddAgent(params FogAgent[] agents) {
		fogAgents.AddRange(agents);
		
		CreateTexture();
	}
	
	public void RemoveAgent(params FogAgent[] agents) {
		foreach (FogAgent agent in agents) {
			fogAgents.Remove(agent);
		}
		
		CreateTexture();
	}
	
	public bool IsFogged(Vector3 point, float alphaThreshold) {
		Vector2 pixel = WorldToPixel(point);
		float alpha = currentAlphaMap[(int)pixel.x.Round(), (int)pixel.y.Round()];
		
		return alpha <= alphaThreshold;
	}
	
	public bool IsFogged(Vector3 point) {
		return IsFogged(point, 0);
	}
}
