using UnityEngine;
using System.Collections;

public class TestTroupe : MonoBehaviour {

	public float speed = 0f;
	
	
	void OnMouseDown(){
		speed += 3;
	}
	
	void Update () {
		Vector3 velocity = new Vector3(speed * Time.deltaTime,0,0);
		transform.Translate(velocity);
		Camera.main.transform.position = new Vector3(this.transform.position.x,0f,10f);
		NetworkController.instance.clientController.sendUnitDeplacement(0,this.transform.position, velocity);
	}
}
