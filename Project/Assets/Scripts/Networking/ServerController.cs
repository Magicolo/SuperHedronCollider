using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using Magicolo;


public class ServerController : MonoBehaviour {

	bool LANOnly = true;
	
	[Disable] public bool serverStarted;
	
	public NetworkController networkController;
	
	
	public static int nextPlayerId;
	public static int nextUnitId;
	public static int nextBulletId;
	
	
	public void StartServer(int port){
		if(serverStarted) return;
		
		int playerId = nextPlayerId++;
		networkController.clientController.playerId = playerId;
		networkController.currentMap.imPlayer(playerId);
		networkController.currentPlayer = networkController.currentMap.players[playerId];
		
		bool useNat=false;
		useNat = LANOnly != true && !Network.HavePublicAddress();
		serverStarted = true;
		
		Network.InitializeServer(16,port,useNat);
	}
	
	void OnServerInitialized() {
		networkController.isConnected = true;
		NetworkViewID newViewID = Network.AllocateViewID();
		networkController.playerCount++;
		
		
		GameObject newPlayer = Instantiate(networkController.networkLinkPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		newPlayer.transform.parent = transform;
		
		newPlayer.GetComponent<NetworkLink>().playerId = 0;
		
		networkController.networkLinks.Add("Server/Client Genre", newPlayer.GetComponent<NetworkLink>());
		
		
		networkController.log("Server as " + newViewID.ToString());
		networkController.log("Server initialized and ready");
    }
	
		
	[RPC]
	void SendAllPlayers(NetworkMessageInfo info){
		if(!Network.isServer) return;
		
		var networkLinks = GetComponentsInChildren<NetworkLink>();
		
		foreach (var link in networkLinks) {
			NetworkPlayer networkPlayer = link.networkPlayer;
			NetworkViewID viewId = link.networkView.viewID;
			networkView.RPC("JoinPlayer", info.sender, viewId, networkPlayer, link.playerId);
		}
	}
	
	
	[RPC]
	public void ToServerSpawnUnit(int playerId, int troopType, Vector3 position, Quaternion rotation){
		if(!Network.isServer) return;
		
		int unitId = nextUnitId++;
		networkView.RPC("ToClientSpawnUnit", RPCMode.All, playerId, unitId,troopType, position, rotation);
		
	}
	
	[RPC]
	void ToServerSpawnBullet(int playerIdSource, int unitIdSource, int playerIdTarget, int unitIdTarget, NetworkMessageInfo info){
		if(!Network.isServer) return;
		
		networkView.RPC("ToClientSpawnBullet", RPCMode.All, nextBulletId++,playerIdSource, unitIdSource, playerIdTarget,unitIdTarget);
		nextBulletId %= int.MaxValue;
	}
	
	
	void OnPlayerConnected(NetworkPlayer p) {
		networkController.playerCount++;
		
		NetworkViewID newViewID = Network.AllocateViewID();
		
		int playerId = nextPlayerId++;
		networkView.RPC("ThisIsYourPlayerId", p, playerId);
		
		networkView.RPC("JoinPlayer", RPCMode.All, newViewID, p, playerId);
			
		networkController.log("Player " + newViewID.ToString() + " connected from " + p.ipAddress + ":" + p.port);
		if(networkController.networkLinks.Count == networkController.currentMap.players.Length){
			networkView.RPC("PrepareStartGame", RPCMode.All);
			GetComponent<GameStartingCounter>().startCounting = true;
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
