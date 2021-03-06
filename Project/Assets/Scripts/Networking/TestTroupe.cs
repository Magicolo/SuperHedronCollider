﻿using UnityEngine;
using System.Collections;

public class TestTroupe : MonoBehaviour {

	public float speed = 0f;
	
	
	void OnMouseDown(){
		speed += 3;
	}
	
	void Update () {
		if(speed > 0){
			Vector3 velocity = new Vector3(speed * Time.deltaTime,0,0);
			transform.Translate(velocity);
			NetworkController.instance.clientController.sendUnitPosition(0,this.transform.position);
		}
		
		Camera.main.transform.position = new Vector3(this.transform.position.x,0f,10f);
	}
}
