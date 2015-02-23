using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;

public class ClientController : MonoBehaviour {

	public NetworkController networkController;
	public NetworkLink localNetworkLink;
	
	public void ConnectToServer(string ip, int remotePort){
    	Network.Connect(ip, remotePort);
	}
	
	void OnConnectedToServer(){
		networkView.RPC("SendAllPlayers", RPCMode.Server);
	}
	
	void OnFailedToConnect(NetworkConnectionError error) {
        networkController.log("Could not connect to server: " + error);
		//GameState = (int)state.failconnect;
    }

	
	[RPC]
	void JoinPlayer(NetworkViewID newPlayerView, NetworkPlayer p){		
		GameObject newPlayer = Instantiate(networkController.playerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		
		newPlayer.GetComponent<NetworkView>().viewID = newPlayerView;
		newPlayer.GetComponent<NetworkLink>().networkPlayer = p;
		
		networkController.players.Add(p,newPlayer);
		
		if(Network.isClient){
			if(p.ipAddress == networkController.LocalAddress){
				networkController.log("Server accepted my connection request, I am real player now: " + newPlayerView.ToString());
				
				//TODO start game or something
				
				// also, set the global localPlayerObject as a convenience variable
				// to easily find the local player GameObject to send position updates
				
				// TODO localPlayerObject = newPlayer;
			} else {
				
				networkController.log("Another player connected: " + newPlayerView.ToString());
			}
		}
		
	}
	
	
	[RPC]
	void DisconnectPlayer(NetworkPlayer player){
		if(Network.isClient) {
			networkController.log("Player Disconnected: " + player.ToString());
		}
		
		if(networkController.players.ContainsKey(player)){
			if((GameObject)networkController.players[player]) {
				Destroy((GameObject)networkController.players[player]);
			}
			networkController.players.Remove(player);
		}
	}
}
