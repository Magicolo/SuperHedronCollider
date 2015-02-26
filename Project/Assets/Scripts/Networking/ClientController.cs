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
		networkController.isConnected = true;
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
		networkView.RPC("UnitDamage",RPCMode.All, troopPlayerId, troopId, damage);
	}
	
	[RPC]
	void UnitDamage(int troopPlayerId, int troopId, float damage, NetworkMessageInfo info){
		TroopManager.DamageTroop(troopPlayerId, troopId, damage);
	}
	
	public void sendUnitLightingData(int troopPlayerId, int troopId, float intensity, float range, bool enabled){
		networkView.RPC("UnitLightingData",RPCMode.Others, troopPlayerId, troopId, intensity, range, enabled);
	}
	
	[RPC]
	void UnitLightingData(int troopPlayerId, int troopId, float intensity, float range, bool lightingEnabled, NetworkMessageInfo info){
		if(!isMe(troopPlayerId)){
			TroopManager.FadeTroopLight(troopPlayerId, troopId, intensity,range,lightingEnabled);
		}
		
	}	
	
	public void sendUnitTarget(int troopId, Vector3 target){
		networkView.RPC("UpdateUnitTarget",RPCMode.Others, this.playerId, troopId,  target);
	}
	
	public void sendUnitPosition(int troopId, Vector3 position){
		networkView.RPC("UpdateUnitPosition",RPCMode.Others, this.playerId, troopId, position);
	}
	
	[RPC]
	void UpdateUnitTarget(int troopPlayerId, int troopId, Vector3 target, NetworkMessageInfo info){
		if(isMe(troopPlayerId)) return;
		TroopManager.SetTroopTarget(troopPlayerId, troopId, target);
	}
	
	[RPC]
	void UpdateUnitPosition(int troopPlayerId, int troopId, Vector3 position, NetworkMessageInfo info){
		if(isMe(troopPlayerId)) return;
		TroopManager.MoveTroop(troopPlayerId, troopId, position);
	}
	
	
	
	public void killUnit(int troopPlayerId, int troopId){
		networkView.RPC("ToClientKillUnit",RPCMode.All, troopPlayerId, troopId);
	}
	
	[RPC]
	void ToClientKillUnit(int troopPlayerId, int troopId, NetworkMessageInfo info){
		TroopManager.KillTroop(troopPlayerId, troopId);
	}
	#endregion
	
	
	#region Bullet
	
	public void spawnBullet(int playerIdSource, int unitIdSource, int playerIdTarget, int unitIdTarget){
		networkView.RPC("ToServerSpawnBullet",RPCMode.Server, playerIdSource, unitIdSource, playerIdTarget, unitIdTarget);
	}
	
	[RPC]
	void ToClientSpawnBullet(int bulletId, int playerIdSource, int unitIdSource, int playerIdTarget, int unitIdTarget, NetworkMessageInfo info){
		BulletManager.Spawn(bulletId, playerIdSource, unitIdSource, playerIdTarget, unitIdTarget);
	}
	
	
	public void sendBulletDeplacement(int bulletId, Vector3 position){
		networkView.RPC("UpdateBullet",RPCMode.Others, this.playerId, bulletId, position);
	}
	
	[RPC]
	void UpdateBullet(int bulletPlayerId, int bulletId, Vector3 position, NetworkMessageInfo info){
		if(isMe(bulletPlayerId)) return;
		BulletManager.MoveBullet(bulletPlayerId, bulletId,position);
	}
	
	public void killBullet(int pId, int bId){
		networkView.RPC("ToClientKillBullet",RPCMode.All, pId, bId);
	}
	
	[RPC]
	void ToClientKillBullet(int bulletPlayerId, int bulletId, NetworkMessageInfo info){
		BulletManager.KillBullet(bulletPlayerId, bulletId);
	}
	#endregion
	
	
	
	
	[RPC]
	void ClientMessageAll(string message, NetworkMessageInfo info){
		networkController.log(message);
	}
	
	[RPC]
	void StartGame(NetworkMessageInfo info){
		GameManager.Start();
		ServerStartSuff.StartMoiUnGameDeTest();
	}
	
	[RPC]
	void StopGame(NetworkMessageInfo info){
		GameManager.Stop();
	}

	bool isMe(int otherPlayerId){
		return otherPlayerId == this.playerId;
	}

	void OnApplicationQuit(){
		//Network.Disconnect();
	}
}
