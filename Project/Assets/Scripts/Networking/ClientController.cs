using System.ComponentModel;
using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using UnityEngine.UI;

public class ClientController : MonoBehaviour {

	public NetworkController networkController;
	public NetworkLink localNetworkLink;
	
	public int playerId;
	
	public Text centerScreenText;
	public float centerScreenTextTimer;
	
	
	void Update(){
		if(centerScreenTextTimer>0){
			centerScreenTextTimer -= Time.deltaTime;
			if(centerScreenTextTimer <= 0){
				centerScreenText.text = "";
			}
		}
	}
	
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
		networkController.currentMap.setUpFor(playerId);
	}
	
		
	[RPC]
	void JoinPlayer(NetworkViewID newPlayerView, NetworkPlayer p, int playerId){
		networkController.addPlayer(newPlayerView,p,playerId);
		
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
	public void spawnUnit(int troopType, Vector3 position, Quaternion rotation) {
		if (NetworkController.instance.isConnected) {
			if(Network.isServer){
				networkController.serverController.ToServerSpawnUnit(playerId, troopType, position, rotation);
			}else{
				networkView.RPC("ToServerSpawnUnit", RPCMode.Server, playerId, troopType, position, rotation);
			}
			
		}
	}
	
	[RPC]
	void ToClientSpawnUnit(int troopPlayerId, int troopId, int troopType, Vector3 position, Quaternion rotation, NetworkMessageInfo info){
		//networkController.log("Spawn unit " + troopId + " for " + troopPlayerId + " at " + position);
		TroopManager.Spawn(troopPlayerId,troopId,troopType,position,rotation);
	}
	
	public void sendUnitDamage(int troopPlayerId, int troopId, float damage){
		if (NetworkController.instance.isConnected) {
			networkView.RPC("UnitDamage", RPCMode.All, troopPlayerId, troopId, damage);
		}
	}
	
	[RPC]
	void UnitDamage(int troopPlayerId, int troopId, float damage, NetworkMessageInfo info){
		networkController.log("Unit " + troopId + " of " + troopPlayerId + " is dealted for " + damage);
		TroopManager.DamageTroop(troopPlayerId, troopId, damage);
	}
	
	public void sendUnitLightingData(int troopPlayerId, int troopId, float intensity, float range, bool enabled) {
		if (NetworkController.instance.isConnected) {
			networkView.RPC("UnitLightingData", RPCMode.All, troopPlayerId, troopId, intensity, range, enabled);
		}
	}
	
	[RPC]
	void UnitLightingData(int troopPlayerId, int troopId, float intensity, float range, bool lightingEnabled, NetworkMessageInfo info){
		if(!isMe(troopPlayerId)){
			//networkController.log("unit FadeTroopLight " + troopId + " for " + troopPlayerId + " intensity:" + intensity + " , range:" + range + " , enabled:" + lightingEnabled);
			TroopManager.FadeTroopLight(troopPlayerId, troopId, intensity,range,lightingEnabled);
		}
		
	}	
	
	public void sendUnitTarget(int troopId, Vector3 target) {
		if (NetworkController.instance.isConnected) {
			networkView.RPC("UpdateUnitTarget", RPCMode.All, this.playerId, troopId, target);
		}
	}
	
	public void sendUnitPosition(int troopId, Vector3 position) {
		if (NetworkController.instance.isConnected) {
			networkView.RPC("UpdateUnitPosition", RPCMode.All, this.playerId, troopId, position);
		}
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
	
	
	
	public void killUnit(int troopPlayerId, int troopId) {
		if (NetworkController.instance.isConnected) {
			networkView.RPC("ToClientKillUnit", RPCMode.All, troopPlayerId, troopId);
		}
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
		networkView.RPC("UpdateBullet",RPCMode.All, this.playerId, bulletId, position);
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
	void ClientCenterScreenMessage(string message, float duration, NetworkMessageInfo info){
		centerScreenText.text = message;
		centerScreenTextTimer = duration;
	}
	
	
	#region gameLoop
	
	[RPC]
	void PrepareStartGame(NetworkMessageInfo info){
		GameManager.Instance.PrepareStart();
	}
		
	[RPC]
	void StartGame(NetworkMessageInfo info){
		GameManager.Instance.Start();
		ServerStartSuff.StartMoiUnGameDeTest();
	}
	
	[RPC]
	void StopGame(NetworkMessageInfo info){
		GameManager.Instance.Stop();
	}
	
	#endregion

	bool isMe(int otherPlayerId){
		return otherPlayerId == this.playerId;
	}

	void OnApplicationQuit(){
		//Network.Disconnect();
	}
}
