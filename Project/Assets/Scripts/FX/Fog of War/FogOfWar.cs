using UnityEngine;
using System.Collections;

public class FogOfWar : MonoBehaviour {

	public int precision = 1;
	
	public Camera sourceCamera;
	
	Texture2D foWTexture;
	int width;
	int height;
	
	
		
	void Awake(){
		
	}
	
	void Start () {
		width = (int)gameObject.transform.localScale.x;
		height = (int)gameObject.transform.localScale.z;
		foWTexture = new Texture2D(width * precision, height * precision, TextureFormat.Alpha8, false);
		
		Material material = GetComponent<MeshRenderer>().materials[0];
		material.mainTexture = foWTexture;
	}
	
	// Update is called once per frame
	void Update () {
		float[,] alphas = new float[width,height];
		
		foreach (var troop in TroopManager.GetTroops(NetworkController.CurrentPlayerId)) {
			Vector3 dir = (troop.transform.position - sourceCamera.transform.position).normalized;
			Ray johnRay = new Ray(sourceCamera.transform.position, dir);
			Debug.DrawRay(sourceCamera.transform.position, dir * 1000, new Color(1,0.7f,0.2f, 1f));
			RaycastHit toucheJohn;
			if (Physics.Raycast(johnRay, out toucheJohn)){
				Vector3 pointDeTouche = toucheJohn.point;
				Point2D textureHit = worldToTexturePosition(pointDeTouche);
				//troop.sightRadius
				Debug.Log(pointDeTouche + " KWAME " + textureHit);
				int range = 8;
				int xMin = Mathf.Clamp(textureHit.x - range ,0,width);
				int xMax = Mathf.Clamp(textureHit.x + range ,0,width);
				int yMin = Mathf.Clamp(textureHit.y - range ,0,height);
				int yMax = Mathf.Clamp(textureHit.y + range ,0,height);
				Vector2 pdt3 = new Vector2(textureHit.x, textureHit.y);
				for (int x = xMin; x < xMax; x++) {
					for (int y = yMin; y < yMax; y++) {
						float distance = Vector2.Distance(new Vector2(x,y),pdt3);
						alphas[x,y] = 1 - distance/range;;
					}
				}
				
			}
		}
		
		Color[] pixels = foWTexture.GetPixels();
		int index = 0;
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				float alpha = alphas[x,y];
				pixels[index] = new Color(0,0,0, 1- alpha);
				index++;
			}
		}
		foWTexture.SetPixels(pixels);
		foWTexture.Apply();
	}

	Point2D worldToTexturePosition(Vector3 pointDeTouche){
		/*float floatyX = pointDeTouche.x + transform.localScale.x/2;
		float floatyZ = pointDeTouche.z + transform.localScale.z/2;*/
		float floatyX = pointDeTouche.x + 300;
		float floatyZ = pointDeTouche.z + 75;
		
		return new Point2D((int)(floatyX/precision),(int)(floatyZ/precision));
		
	}
}
