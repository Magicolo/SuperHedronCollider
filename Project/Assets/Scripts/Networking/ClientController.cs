using UnityEngine;
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
	
	
	
	public void sendUnitDeplacement(int troopId, Vector3 position, Vector3 velocity){
		networkView.RPC("UpdateUnit",RPCMode.Others, this.playerId, troopId, position, velocity);
	}
	
	[RPC]
	void UpdateUnit(int troopPlayerId, int troopId, Vector3 position, Vector3 velocity, NetworkMessageInfo info){
		if(isMe(troopPlayerId)) return;
		Debug.Log("Yo dog bouge moi ca à " + position);
		GameObject go =  GameObject.Find("TestTroupe");
		go.transform.position = position;
	}
	
	
	public void spawnUnitTest(){
		
	}
	
	
	
	public void spawnUnit(int troopType, Vector3 position, Quaternion rotation){
		networkView.RPC("ToServerSpawnUnit",RPCMode.Server, playerId, troopType, position, rotation);
	}
	
	[RPC]
	void ToClientSpawnUnit(int troopPlayerId, int troopId, int troopType, Vector3 position, Quaternion rotation, NetworkMessageInfo info){
		TroopManager.Spawn(troopPlayerId,troopId,troopType,position,rotation);
	}
	
	
	public void killUnit(int troopPlayerId, int troopId){
		networkView.RPC("ToClientKillUnit",RPCMode.All, troopPlayerId, troopId);
	}
	
	[RPC]
	void ToClientKillUnit(int troopPlayerId, int troopId, NetworkMessageInfo info){
		TroopManager.Despawn(troopPlayerId, troopId);
	}
	
	
	
	
	
	
	
	
	
	
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
