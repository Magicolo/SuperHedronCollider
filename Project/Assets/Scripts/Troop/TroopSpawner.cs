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
	private int cur;
	
	public SpawnerType curSpawn {
		get{
			return spawners[cur];
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
			}
		}
	}
	
	void OnMouseDown () {
		Destroy(animObject);
		cur ++;
		if (cur == spawners.Length){
			cur = 0;
		}
		animObject = GameObject.Instantiate(curSpawn.spawner, Vector3.zero, Quaternion.identity) as GameObject;
		anim = animObject.GetComponent<Animator>();
		anim.enabled = false;
		mr = anim.GetComponentInChildren<SkinnedMeshRenderer>();
		light.color = curSpawn.lightColour;
		cooldown = cooldownMax;
	}
	
}
