﻿using UnityEngine;
using System.Collections;

public class MainMenuFunctions : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void LoadMap (int levelId){
		Application.LoadLevel(levelId);
	}
	
	
}
