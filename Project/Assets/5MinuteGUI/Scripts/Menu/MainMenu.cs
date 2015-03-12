using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMG;


namespace FMG
{
	public class MainMenu : MonoBehaviour {
		public GameObject mainMenu;

		public bool useLevelSelect = true;
		public bool useExitButton = true;

		public GameObject exitButton;
		
		public MenuPanel currentMenu;
		
		//Server stuff
		public Text portText;
		public Text mapText;
		public int currentMap;
		public string[] maps;
		
		//Join Stuff
		public InputField clientIp;
		public InputField clientPort;
		public Text clientStatusFeedback;
		
		public void Awake()
		{
			if(useExitButton==false)
			{
				exitButton.SetActive(false);
			}
		}

		public void onCommand(string str){
			if( currentMenu.handleCommand(str, this) ) return;
			
			
			if(str.Equals("Exit")){
				Application.Quit();
			}else if(str.Equals("Back")){
				swithTo(currentMenu.previousPanel);
			}
			
			if(str.Equals("LevelSelect")){
					Application.LoadLevel(1);
			}

			if(str.Equals("nextMap")){
				currentMap = (int)((currentMap+1) % maps.Length);
				mapText.text = maps[currentMap];
				
			}else if(str.Equals("previousMap")){
				currentMap = (int)((currentMap-1) % maps.Length);
				mapText.text = maps[currentMap];
			}else if(str.Equals("StartServer")){
				int port;
				if(int.TryParse(portText.text, out port)){
					NetworkController.instance.StartServer(port);
					NetworkController.instance.serverController.sceneIndex = currentMap + 1;
					Application.LoadLevel(currentMap + 1);
				}else{
					//TODO faire qlq chose sil met pas un port car yé CAVE !
				}
			}else if(str.Equals("JoinServer")){
				string serverIp = clientIp.text;
				int port;
				if(int.TryParse(clientPort.text, out port)){
					clientStatusFeedback.text = "Connecting ...";
					NetworkController.instance.ConnectToServer(serverIp,port);
				}else{
					clientStatusFeedback.text = "A port number is a number";
				}
			}


		}

		public void swithTo(MenuPanel menuPanel) {
			Constants.fadeInFadeOut(menuPanel.gameObject,currentMenu.gameObject);
			menuPanel.previousPanel = currentMenu;
			currentMenu = menuPanel;
		}
	}
	
	[System.Serializable]
	public class Transition {
		public string command;
		public GameObject toPanel;
		
		public void transition(GameObject currentPanel){
			Constants.fadeInFadeOut(currentPanel,toPanel);
		}
	}
	
}
