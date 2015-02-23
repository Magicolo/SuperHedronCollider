using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using Magicolo;


public class ServerController : MonoBehaviour {

	bool LANOnly = true;
	
	[Disable] public bool serverStarted;
	
	public NetworkController networkController;
	
	
	public void StartServer(int port){
		if(serverStarted) return;
		
		bool useNat=false;
		useNat = LANOnly != true && !Network.HavePublicAddress();
		serverStarted = true;
		
		Network.InitializeServer(16,port,useNat);
	}
	
	void OnServerInitialized() {
		networkController.log("Server initialized and ready");
    }
	
		
	[RPC]
	void SendAllPlayers(NetworkMessageInfo info){
		var networkLinks = GetComponentsInChildren<NetworkLink>();
		
		foreach (var link in networkLinks) {
			NetworkPlayer networkPlayer = link.networkPlayer;
			NetworkViewID viewId = link.networkView.viewID;
			networkView.RPC("JoinPlayer", info.sender, viewId, networkPlayer);
		}
	}
	
	void OnPlayerConnected(NetworkPlayer p) {
		networkController.playerCount++;
		
		NetworkViewID newViewID = Network.AllocateViewID();
		
		networkView.RPC("JoinPlayer", RPCMode.All, newViewID, p);
			
		networkController.log("Player " + newViewID.ToString() + " connected from " + p.ipAddress + ":" + p.port);
		networkController.log("There are now " + networkController.playerCount + " players.");
		if(networkController.networkLinks.Count == 2){
			foreach (var client in networkController.networkLinks.Values) {
				
				//TODO start the game
				//networkView.RPC("ClientMessageAll",client.networkPlayer,messageSend);
			}
		}
    }
		
	void OnPlayerDisconnected(NetworkPlayer player) {
		networkController.playerCount--;
		
		networkController.log("Player " + player.ToString() + " disconnected.");
		networkController.log("There are now " + networkController.playerCount + " players.");
		
		networkView.RPC("DisconnectPlayer", RPCMode.All, player);
    }
	
	[RPC]
	void ServerMessageAll(string message, NetworkMessageInfo info){
		string messageSend = System.DateTime.Now.ToShortTimeString() + " : " + message;
		foreach (var client in networkController.networkLinks.Values) {
			networkView.RPC("ClientMessageAll",client.networkPlayer,messageSend);
		}
	}
	
	void OnApplicationQuit(){
		Network.Disconnect();
	}
	
}
