using UnityEngine;
using System.Collections;

public class FogOfWar : MonoBehaviour {

	public float precision = 0.5f;
	
	public Camera sourceCamera;
	
	Texture2D foWTexture;
	int width;
	int height;
	
	
		
	void Awake(){
		
	}
	
	void Start () {
		/*width = (int)(gameObject.transform.localScale.x * precision);
		height = (int)(gameObject.transform.localScale.z * precision);
		foWTexture = new Texture2D(width , height , TextureFormat.RGBA32, false);
		
		Material material = GetComponent<MeshRenderer>().materials[0];
		material.mainTexture = foWTexture;*/
	}
	
	// Update is called once per frame
	void Update () {
		width = (int)(gameObject.transform.localScale.x * precision);
		height = (int)(gameObject.transform.localScale.z * precision);
		foWTexture = new Texture2D(width , height , TextureFormat.RGBA32, false);
		
		Material material = GetComponent<MeshRenderer>().materials[0];
		material.mainTexture = foWTexture;
		
		
		float[,] alphas = new float[width,height];
		
		foreach (var troop in TroopManager.GetTroops(NetworkController.CurrentPlayerId)) {
			Vector3 dir = (troop.transform.position - sourceCamera.transform.position).normalized;
			Ray johnRay = new Ray(sourceCamera.transform.position, dir);
			RaycastHit toucheJohn;
			if (Physics.Raycast(johnRay, out toucheJohn)){
				Vector3 pointDeTouche = toucheJohn.point;
				Point2D textureHit = worldToTexturePosition(pointDeTouche);
				int range = (int)(troop.sightRadius * width / 600.0f *4);
				Debug.Log(troop.sightRadius * width + " - " + range);
				int xMin = Mathf.Clamp(textureHit.x - range ,0,width);
				int xMax = Mathf.Clamp(textureHit.x + range ,0,width);
				int yMin = Mathf.Clamp(textureHit.y - range ,0,height);
				int yMax = Mathf.Clamp(textureHit.y + range ,0,height);
				Vector2 pdt3 = new Vector2(textureHit.x, textureHit.y);
				for (int x = xMin; x < xMax; x++) {
					for (int y = yMin; y < yMax; y++) {
						float distance = Mathf.Abs(Vector2.Distance(new Vector2(x,y),pdt3));
						alphas[x,y] = 1- distance/range;
					}
				}
				
			}
		}
		
		Color[] pixels = foWTexture.GetPixels();
		int index = 0;
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				float alpha = alphas[x,y];
				pixels[index] = new Color(0,0,0, 1 - alpha);
				index++;
			}
		}
		foWTexture.SetPixels(pixels);
		foWTexture.Apply();
	}

	Point2D worldToTexturePosition(Vector3 pointDeTouche){
		float machinX = 300/2;
		float machinY = 72/2;
		float floatyX = (pointDeTouche.x + machinX) * width / 300;
		float floatyZ = (pointDeTouche.z + machinY) * height / 75;
		return new Point2D((int)(floatyX),(int)(floatyZ));
		
	}
}
