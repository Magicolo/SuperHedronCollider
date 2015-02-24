﻿using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;

public class ClientController : MonoBehaviour {

	public NetworkController networkController;
	public NetworkLink localNetworkLink;
	
	public int playerId;
	
	
	public void ConnectToServer(string ip, int remotePort){
    	Network.Connect(ip, remotePort);
	}
	
	void OnConnectedToServer(){
		networkView.RPC("SendAllPlayers", RPCMode.Server);
	}
	
	void OnFailedToConnect(NetworkConnectionError error) {
        networkController.log("Could not connect to server: " + error);
    }

	[RPC]
	void ThisIsYourPlayerId(int myNewPlayerId, NetworkMessageInfo info){
		playerId = myNewPlayerId;
	}
	
		
	[RPC]
	void JoinPlayer(NetworkViewID newPlayerView, NetworkPlayer p){
		networkController.addPlayer(newPlayerView,p);
	}
	
	
	[RPC]
	void DisconnectPlayer(NetworkPlayer player){
		if(Network.isClient) {
			networkController.log("Player Disconnected: " + player.ToString());
		}
		
		networkController.networkLinks.Remove(player.ToString());
	}
	
	public void sendMessage(string message){
		networkView.RPC("ClientMessageAll",RPCMode.All,message);
	}
	
	
	
	
	
	
	public void spawnUnitTest(){
		
	}
	
	
	#region Unit
	public void spawnUnit(int troopType, Vector3 position, Quaternion rotation){
		networkView.RPC("ToServerSpawnUnit",RPCMode.Server, playerId, troopType, position, rotation);
	}
	
	[RPC]
	void ToClientSpawnUnit(int troopPlayerId, int troopId, int troopType, Vector3 position, Quaternion rotation, NetworkMessageInfo info){
		TroopManager.Spawn(troopPlayerId,troopId,troopType,position,rotation);
	}
	
	public void sendUnitDamage(int troopPlayerId, int troopId, float damage){
		networkView.RPC("UnitDamage",RPCMode.All, this.playerId, troopId, damage);
	}
	
	[RPC]
	void UnitDamage(int troopPlayerId, int troopId, int damage, NetworkMessageInfo info){
		TroopManager.UnitDamage(troopPlayerId, troopId,damage);
	}
	
	public void sendUnitDeplacement(int troopId, Vector3 position, Vector3 velocity){
		networkView.RPC("UpdateUnit",RPCMode.Others, this.playerId, troopId, position, velocity);
	}
	
	[RPC]
	void UpdateUnit(int troopPlayerId, int troopId, Vector3 position, Vector3 velocity, NetworkMessageInfo info){
		if(isMe(troopPlayerId)) return;
		Debug.Log("Yo dog bouge moi ca à " + position);
		TroopManager.MoveUnit(troopPlayerId, troopId,position,velocity);
		GameObject go =  GameObject.Find("TestTroupe");
		go.transform.position = position;
	}
	
	public void killUnit(int troopPlayerId, int troopId){
		networkView.RPC("ToClientKillUnit",RPCMode.All, troopPlayerId, troopId);
	}
	
	[RPC]
	void ToClientKillUnit(int troopPlayerId, int troopId, NetworkMessageInfo info){
		TroopManager.Kill(troopPlayerId, troopId);
	}
	#endregion
	
	
	#region Bullet
	
	public void spawnBullet(Vector3 position, Quaternion rotation){
		networkView.RPC("ToServerSpawnBullet",RPCMode.Server, playerId, position, rotation);
	}
	
	[RPC]
	void ToClientSpawnBullet(int bulletPlayerId, int bulletId, Vector3 position, Quaternion rotation, NetworkMessageInfo info){
		// TODO  YO KEVIN!! decommente ca !
		//BulletManager.Spawn(bulletPlayerId, bulletId, position, rotation);
	}
	
	
	public void sendBulletDeplacement(int bulletId, Vector3 position, Vector3 velocity){
		networkView.RPC("UpdateBullet",RPCMode.Others, this.playerId, bulletId, position, velocity);
	}
	
	[RPC]
	void UpdateBullet(int bulletPlayerId, int bulletId, Vector3 position, Vector3 velocity, NetworkMessageInfo info){
		if(isMe(bulletPlayerId)) return;
		// TODO  YO KEVIN!! decommente ca !
		//TroopManager.MoveBullet(bulletPlayerId, bulletId,position,velocity);
	}
	
	public void killBullet(int pId, int bId){
		networkView.RPC("ToClientKillBullet",RPCMode.All, pId, bId);
	}
	
	[RPC]
	void ToClientKillBullet(int bulletPlayerId, int bulletId, NetworkMessageInfo info){
		// TODO  YO KEVIN!! decommente ca !
		//BulletManager.Kill(bulletPlayerId, bulletId);
	}
	#endregion
	
	
	
	
	[RPC]
	void ClientMessageAll(string message, NetworkMessageInfo info){
		networkController.log(message);
	}
	
	[RPC]
	void StartGame(NetworkMessageInfo info){
		GameManager.Start();
	}
	
	[RPC]
	void StopGame(NetworkMessageInfo info){
		GameManager.STOP();
	}

	bool isMe(int otherPlayerId){
		return otherPlayerId == this.playerId;
	}

	void OnApplicationQuit(){
		//Network.Disconnect();
	}
}
