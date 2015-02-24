using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using Magicolo;


public class ServerController : MonoBehaviour {

	bool LANOnly = true;
	
	[Disable] public bool serverStarted;
	
	public NetworkController networkController;
	
	
	public int nextUniqueId;
	
	
	public void StartServer(int port){
		if(serverStarted) return;
		
		networkController.clientController.playerId = nextUniqueId++;
		bool useNat=false;
		useNat = LANOnly != true && !Network.HavePublicAddress();
		serverStarted = true;
		
		Network.InitializeServer(16,port,useNat);
	}
	
	void OnServerInitialized() {
		NetworkViewID newViewID = Network.AllocateViewID();
		networkController.playerCount++;
		
		networkController.log("Server as " + newViewID.ToString());
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
	
	/*[RPC]
	void SendNextUniqueId(NetworkMessageInfo info){
		networkView.RPC("", info.sender, viewId, networkPlayer);
	}*/
	
	
	
	void OnPlayerConnected(NetworkPlayer p) {
		networkController.playerCount++;
		
		NetworkViewID newViewID = Network.AllocateViewID();
		
		networkView.RPC("ThisIsYourPlayerId", p, nextUniqueId++);
		
		networkView.RPC("JoinPlayer", RPCMode.All, newViewID, p);
			
		networkController.log("Player " + newViewID.ToString() + " connected from " + p.ipAddress + ":" + p.port);
		if(networkController.networkLinks.Count == 1){
			networkView.RPC("ClientMessageAll",RPCMode.All,"Start game");
			networkView.RPC("StartGame", RPCMode.All);
		}
    }
		
	void OnPlayerDisconnected(NetworkPlayer player) {
		networkController.playerCount--;
		
		networkController.log("Player " + player.ToString() + " disconnected.");
		networkController.log("There are now " + networkController.playerCount + " players.");
		
		networkView.RPC("DisconnectPlayer", RPCMode.All, player);
		networkView.RPC("ClientMessageAll",RPCMode.All,"Stop game");
		networkView.RPC("StopGame", RPCMode.All);
    }
	
	void OnApplicationQuit(){
		Network.Disconnect();
	}
	
}
