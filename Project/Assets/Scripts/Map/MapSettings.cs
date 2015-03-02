﻿using UnityEngine;
using System.Collections;

public class MapSettings : MonoBehaviour {

	public MapPlayerSettings[] players;

	public void setUpFor(int playerId){
		players[playerId].setUpFor(playerId);
	}
	
	public void imPlayer(int playerId){
		players[playerId].setUpFor(playerId);
		players[playerId].setUpMeAsPlayer();
	}
	
	void Awake(){
		NetworkController.instance.setCurrentMap(this);
	}
}
