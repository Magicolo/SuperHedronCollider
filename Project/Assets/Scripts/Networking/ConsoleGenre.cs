using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ConsoleGenre : MonoBehaviour {

	
	public Text consoleOutput;
	private bool activeToggle = true;
	
	void Start () {
	
	}
	
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.F1)){
			consoleOutput.gameObject.SetActive( !activeToggle);
			activeToggle = !activeToggle;
		}
	}
}
