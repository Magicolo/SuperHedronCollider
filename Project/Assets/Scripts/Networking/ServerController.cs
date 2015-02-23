using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using Magicolo;


public class ServerController : MonoBehaviour {

	private bool LANOnly = true;
	
	public NetworkController networkController;
	
	
	public void StartServer(int port){
		bool useNat=false;
		useNat = LANOnly != true && !Network.HavePublicAddress();
		
		Network.InitializeServer(16,port,useNat);	
	}
	
	void OnServerInitialized() 
	{
		networkController.log("Server initialized and ready");
		//GameState = (int)state.waitclient;
    }
	
		
	[RPC]
	void SendAllPlayers(NetworkMessageInfo info){
		var networkLinks = GetComponentsInChildren<NetworkLink>();
		
		foreach (var link in networkLinks) {
			NetworkPlayer networkPlayer = link.networkPlayer;
			NetworkViewID viewId = link.networkView.viewID;
			if(networkPlayer.ToString() != info.sender.ToString()){
				networkView.RPC("JoinPlayer", info.sender, viewId, networkPlayer);
			}
		}
	}
	
	void OnPlayerConnected(NetworkPlayer p) {
		networkController.playerCount++;
		
		NetworkViewID newViewID = Network.AllocateViewID();
		
		networkView.RPC("JoinPlayer", RPCMode.All, newViewID, p);
			
		networkController.log("Player " + newViewID.ToString() + " connected from " + p.ipAddress + ":" + p.port);
		networkController.log("There are now " + networkController.playerCount + " players.");
    }
		
	void OnPlayerDisconnected(NetworkPlayer player) {
		networkController.playerCount--;
		
		networkController.log("Player " + player.ToString() + " disconnected.");
		networkController.log("There are now " + networkController.playerCount + " players.");
		
		networkView.RPC("DisconnectPlayer", RPCMode.All, player);
    }
	
}
