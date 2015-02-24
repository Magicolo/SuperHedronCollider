using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine.UI;

public class NetworkController : MonoBehaviour {

	[Disable]public string LocalAddress = "127.0.0.1";
	[Disable]public string ServerAddress = "127.0.0.1";
	
	public static NetworkController instance;
	public GameObject networkLinkPrefab;
	
	public ServerController serverController;
	public ClientController clientController;
	
	public NetworkPlayer localNetworkPlayer;
	
	public Dictionary<string, NetworkLink> networkLinks = new Dictionary<string, NetworkLink>();
	public int playerCount;
	
	public Text outputText;
	
	void Awake(){
		NetworkController.instance = this;
		outputText = GameObject.Find("AwesomeGUY").transform.FindChild("Log").GetComponent<Text>();
	}
	
	void Start () {
		LocalAddress = GetLocalIPAddress();
		ServerAddress = LocalAddress;
		log("Local IP Address: " + LocalAddress);
	}
	
	public string GetLocalIPAddress(){
		IPHostEntry host;
		string localIP = "";
		host = Dns.GetHostEntry(Dns.GetHostName());
		foreach (IPAddress ip in host.AddressList){
			if (ip.AddressFamily == AddressFamily.InterNetwork){
				localIP = ip.ToString();
			}
		}
		return localIP;
	}

	public void addPlayer(NetworkViewID newPlayerView, NetworkPlayer p){
		GameObject newPlayer = Instantiate(networkLinkPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		newPlayer.transform.parent = transform;
		
		newPlayer.GetComponent<NetworkView>().viewID = newPlayerView;
		newPlayer.GetComponent<NetworkLink>().networkPlayer = p;
		
		networkLinks.Add(p.ToString(), newPlayer.GetComponent<NetworkLink>());
		
		if(p.ipAddress == LocalAddress){
			localNetworkPlayer = p;
			log("Server accepted my connection request, I am real player now: " + newPlayerView.ToString());
		} else {
			log("Another player connected: " + newPlayerView.ToString() + " - " + p.ipAddress);
		}
	}
	
	
	public void Disconnect(){
		Network.Disconnect();
	}
	
	public void StartServer(int port){
		serverController.StartServer(port);
	}
	
	public void ConnectToServer(){
    	Network.Connect("127.0.0.1", 25565);
	}
	
	public void log(string message){
		outputText.text += message + "\n";
	}
	
}
