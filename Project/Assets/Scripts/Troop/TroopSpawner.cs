using UnityEngine;
using System.Collections;
[RequireComponent(typeof(CapsuleCollider))]
public class TroopSpawner : MonoBehaviour {
	[System.Serializable]
	public class SpawnerType {
		public GameObject spawner;
		public float rate = 2f;
		public Color lightColour = Color.cyan;
	}
	
	public SpawnerType[] spawners;
	private int CurrentTroopTypeIdToSpawnInTheSpawnner;
	
	public int playerId;
	
	public SpawnerType curSpawn {
		get{
			return spawners[CurrentTroopTypeIdToSpawnInTheSpawnner];
		}
	}
	
	private Color darkColour = Color.black;
	private Color readyColour = Color.white;
	
	private SkinnedMeshRenderer mr;
	private MeshFilter mf;
	private Animator anim;
	private GameObject animObject;
	private Light light;
	
	private float spawnTimer;
	private float cooldownMax = 4f;
	private float cooldown = 4f;
	
	
	// Use this for initialization
	void Start () {
		mr = GetComponentInChildren<SkinnedMeshRenderer>();
		anim = GetComponentInChildren<Animator>();
		animObject = anim.gameObject;
		Debug.Log("My anim is " + anim);
		anim.enabled = false;
		light = GetComponentInChildren<Light>();
		light.color = curSpawn.lightColour;
	}
	
	// Update is called once per frame
	void Update () {
		if(playerId != NetworkController.CurrentPlayerId) return;
		if (cooldown > 0){
			cooldown -= Time.deltaTime;
			mr.material.color = Color.Lerp(darkColour, readyColour, (cooldownMax - cooldown) / cooldownMax);
			if (cooldown <= 0){
				spawnTimer = curSpawn.rate;
				anim.enabled = true;
			}
		} else {
			spawnTimer += Time.deltaTime;
			if (spawnTimer > curSpawn.rate){
				anim.Play("Poop", -1, 0);
				spawnTimer = 0;
				animObject.GetComponent<TrooperPooper>().Spawn(playerId);
			}
		}
	}
	
	void OnMouseDown () {
		if(playerId != NetworkController.CurrentPlayerId) return;
		
		Destroy(animObject);
		CurrentTroopTypeIdToSpawnInTheSpawnner ++;
		if (CurrentTroopTypeIdToSpawnInTheSpawnner == spawners.Length){
			CurrentTroopTypeIdToSpawnInTheSpawnner = 0;
		}
		animObject = Object.Instantiate(curSpawn.spawner, transform.position, Quaternion.identity) as GameObject;
		animObject.transform.parent = transform;
		anim = animObject.GetComponent<Animator>();
		animObject.GetComponent<TrooperPooper>().type = CurrentTroopTypeIdToSpawnInTheSpawnner;
		anim.enabled = false;
		mr = anim.GetComponentInChildren<SkinnedMeshRenderer>();
		light.color = curSpawn.lightColour;
		cooldown = cooldownMax;
	}
	
}
