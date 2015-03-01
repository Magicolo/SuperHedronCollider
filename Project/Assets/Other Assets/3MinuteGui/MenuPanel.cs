using UnityEngine;
using System.Collections;
using FMG;

namespace TMG{
	public class MenuPanel : MonoBehaviour {

		public MenuPanel[] transitions;
		
		public MenuPanel previousPanel;

		public bool handleCommand(string command, MainMenu menu) {
			foreach (var menuPanel in transitions) {
				if(menuPanel.name.Equals(command)){
					menu.swithTo(menuPanel);
					return true;
				}
			}
			return false;
		}
	}
	
}
