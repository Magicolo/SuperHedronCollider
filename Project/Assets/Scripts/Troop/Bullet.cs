using UnityEngine;
using System.Collections;

public enum TroopTypes {
	tetra = 0,
	hexa = 1,
	iso = 2
}

public class Bullet : MonoBehaviour {
	
	
	
	public Transform target;
	public TroopTypes type;
	public float speed = 2f;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate((target.position - transform.position).normalized * speed * Time.deltaTime);
	}
	
	void OnTriggerEnter(Collider other){
		if (other.transform == target){
			//die
			Debug.Log("Die");
		}
	}
	
	
	static bool HasAdvantage (TroopTypes user, TroopTypes target){
		if (user == TroopTypes.tetra){
			return target == TroopTypes.tetra;
		} else {
			return user > target;
		}
	}
}
