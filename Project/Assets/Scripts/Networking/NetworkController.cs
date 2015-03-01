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
	[Disable]public string outputMissed;
	
	
	public MapSettings currentMap;
	public MapPlayerSettings currentPlayer;
	
	public GameObject awesomeGuy;
	
	void Awake() {
		DontDestroyOnLoad(gameObject);
		NetworkController.instance = this;
		//outputText = GameObject.Find("AwesomeGUY").transform.FindChild("Log").GetComponent<Text>();
	}
	
	void Update(){
		//Full cheat connect
		if(!isConnected){
			if(Input.GetKeyDown(KeyCode.F2)){
				StartServer(25565);
			}
			if(Input.GetKeyDown(KeyCode.F3)){
				ConnectToLocalServer();
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
		
		
		
		
		if (p.ipAddress == LocalAddress) {
			currentMap.imPlayer(playerId);
			currentPlayer = currentMap.players[playerId];
			localNetworkPlayer = p;
			log("Server accepted my connection request, I am real player now: " + newPlayerView.ToString());
		}else {
			currentMap.setUpFor(playerId);
			log("Another player connected: " + newPlayerView.ToString() + " - " + p.ipAddress);
			
		}
		if(!networkLinks.ContainsKey(p.ToString())){
		   networkLinks.Add(p.ToString(), newPlayer.GetComponent<NetworkLink>());
	   	}
	}
	
	
	public void Disconnect() {
		Network.Disconnect();
	}
	
	public void StartServer(int port) {
		serverController.StartServer(port);
		awesomeGuy.SetActive(true);
	}
	
	public void ConnectToLocalServer() {
		ConnectToServer("127.0.0.1", 25565);
	}

	public void ConnectToServer(string serverIp, int port) {
		Network.Connect(serverIp, port);
		awesomeGuy.SetActive(true);
	}
	
	public void setCurrentMap(MapSettings mapSettings) {
		currentMap = mapSettings;
		if(Network.isServer){
			currentMap.imPlayer(CurrentPlayerId);
			currentPlayer = currentMap.players[CurrentPlayerId];
		}
	}
	
	public void flushMessages(){
		if(outputMissed.Length > 0){
			outputText.text += outputMissed;
			outputMissed = "";
		}
	}
	
	public void log(string message) {
		if (outputText == null) {
			outputMissed += message;
			return;
		}
		
		if (outputMissed.Length > 0) {
			flushMessages();
		}
		
		nbMessage++;
		if (nbMessage > 20) {
			nbMessage--;
			outputText.text = outputText.text.Substring(2 + outputText.text.IndexOf("\n", System.StringComparison.Ordinal));
		}
		outputText.text += message + "\n";
	}
	
}
