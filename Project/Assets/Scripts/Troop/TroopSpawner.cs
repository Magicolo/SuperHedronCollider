using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Animation))]
[RequireComponent(typeof(MeshRenderer))]
public class TroopSpawner : MonoBehaviour {
	[System.Serializable]
	public class SpawnerType {
		public Mesh mesh;
		public AnimationClip spawnAnimation;
		public GameObject toSpawn;
		public float rate = 2f;
		public Color lightColour = Color.cyan;
	}
	
	public SpawnerType[] spawners;
	private int cur;
	
	public SpawnerType curSpawn {
		get{
			return spawners[cur];
		}
	}
	
	private Color darkColour = Color.black;
	private Color readyColour = Color.white;
	
	private MeshRenderer mr;
	private MeshFilter mf;
	private Animator anim;
	
	private float spawnTimer;
	private float cooldownMax = 5f;
	private float cooldown = 5f;
	
	
	
	// Use this for initialization
	void Start () {
		mr = GetComponent<MeshRenderer>();
		anim = GetComponent<Animator>();
		
		
	}
	
	// Update is called once per frame
	void Update () {
		if (cooldown > 0){
			cooldown -= Time.deltaTime;
			mr.material.color = Color.Lerp(darkColour, readyColour, (cooldownMax - cooldown) / cooldownMax);
		} else {
			spawnTimer += Time.deltaTime;
			if (spawnTimer > curSpawn.rate){
				Debug.Log ("Spawn!");
			}
		}
	}
}
