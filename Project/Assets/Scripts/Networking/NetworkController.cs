using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine.UI;

public class NetworkController : MonoBehaviour {

	[Disable]public string LocalAddress = "127.0.0.1";
	[Disable]public string ServerAddress = "127.0.0.1";
	
	[Disable] public bool isConnected = false;
	
	public static NetworkController instance;
	public static int CurrentPlayerId {
		get {
			return instance.clientController.playerId;
		}
	}
	public GameObject networkLinkPrefab;
	
	public ServerController serverController;
	public ClientController clientController;
	
	public NetworkPlayer localNetworkPlayer;
	
	public Dictionary<string, NetworkLink> networkLinks = new Dictionary<string, NetworkLink>();
	public int playerCount;
	
	private int nbMessage;
	public Text outputText;
	
	
	public MapSettings currentMap;
	public MapPlayerSettings currentPlayer;
	
	void Awake() {
		NetworkController.instance = this;
		outputText = GameObject.Find("AwesomeGUY").transform.FindChild("Log").GetComponent<Text>();
	}
	
	void Update(){
		//Full cheat connect
		if(!isConnected){
			if(Input.GetKeyDown(KeyCode.F2)){
				StartServer(25565);
			}
			if(Input.GetKeyDown(KeyCode.F3)){
				ConnectToServer();
			}
		}
		
	
	}
	
	void Start() {
		LocalAddress = GetLocalIPAddress();
		ServerAddress = LocalAddress;
		log("Local IP Address: " + LocalAddress);
	}
	
	public string GetLocalIPAddress() {
		IPHostEntry host;
		string localIP = "";
		host = Dns.GetHostEntry(Dns.GetHostName());
		foreach (IPAddress ip in host.AddressList) {
			if (ip.AddressFamily == AddressFamily.InterNetwork) {
				localIP = ip.ToString();
			}
		}
		return localIP;
	}

	public void addPlayer(NetworkViewID newPlayerView, NetworkPlayer p, int playerId) {
		GameObject newPlayer = Instantiate(networkLinkPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		newPlayer.transform.parent = transform;
		
		newPlayer.GetComponent<NetworkView>().viewID = newPlayerView;
		newPlayer.GetComponent<NetworkLink>().networkPlayer = p;
		newPlayer.GetComponent<NetworkLink>().playerId = playerId;
		
		networkLinks.Add(p.ToString(), newPlayer.GetComponent<NetworkLink>());
		
		
		
		if (p.ipAddress == LocalAddress) {
			currentMap.imPlayer(playerId);
			currentPlayer = currentMap.players[playerId];
			localNetworkPlayer = p;
			log("Server accepted my connection request, I am real player now: " + newPlayerView.ToString());
		}else {
			currentMap.setUpFor(playerId);
			log("Another player connected: " + newPlayerView.ToString() + " - " + p.ipAddress);
		}
	}
	
	
	public void Disconnect() {
		Network.Disconnect();
	}
	
	public void StartServer(int port) {
		serverController.StartServer(port);
	}
	
	public void ConnectToServer() {
		Network.Connect("127.0.0.1", 25565);
	}
	
	public void log(string message) {
		nbMessage++;
		if(nbMessage > 20){
			nbMessage--;
			outputText.text = outputText.text.Substring(2 + outputText.text.IndexOf("\n", System.StringComparison.Ordinal));
		}
		outputText.text += message + "\n";
	}
	
}
