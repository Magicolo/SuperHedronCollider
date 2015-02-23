using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using UnityEngine.UI;

public class NetworkController : MonoBehaviour {

	[Disable]public string LocalAddress = "127.0.0.1";
	[Disable]public string ServerAddress = "127.0.0.1";
	
	public GameObject playerPrefab;
	
	public ServerController serverController;
	public ClientController clientController;
	
	public NetworkPlayer localNetworkPlayer;
	
	public Hashtable players = new Hashtable();
	public int playerCount;
	
	public Text outputText;
	
	void Awake(){
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
