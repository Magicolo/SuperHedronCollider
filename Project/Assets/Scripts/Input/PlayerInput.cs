using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class PlayerInput : StateLayer {
	
	public bool debug;
	public bool mouseMarginDrag = true;
	public int mouseSelectButton = 0;
	public int mouseActionButton = 1;
	public int mouseMoveButton = 2;
	public KeyCode addToSelectionKey1 = KeyCode.LeftShift;
	public KeyCode addToSelectionKey2 = KeyCode.RightShift;
	public KeyCode zoomKey1 = KeyCode.Equals;
	public KeyCode zoomKey2 = KeyCode.Plus;
	public KeyCode zoomKey3 = KeyCode.KeypadPlus;
	public KeyCode dezoomKey1 = KeyCode.Minus;
	public KeyCode dezoomKey2 = KeyCode.KeypadMinus;
	public KeyCode dezoomKey3 = KeyCode.Underscore;
	public KeyCode fastMoveKey1 = KeyCode.LeftShift;
	public KeyCode fastMoveKey2 = KeyCode.RightShift;
	public List<TroopBase> selectedTroops = new List<TroopBase>();
	
	public override void OnUpdate() {
		base.OnUpdate();
		
		if (debug && Input.GetKey(KeyCode.Space)) {
			if (!NetworkController.instance.isConnected) {
				TroopManager.Spawn(new []{ NetworkController.CurrentPlayerId, 100 }.GetRandom(), Random.Range(0, int.MaxValue), Random.Range(0, 3));
			}
			else if (NetworkController.instance.clientController.playerId == 1) {
				NetworkController.instance.clientController.spawnUnit(0, new Vector3(-50, 0, 0), Quaternion.identity);
			}
			else {
				NetworkController.instance.clientController.spawnUnit(0, new Vector3(50, 0, 0), Quaternion.identity);
			}
			
		}
	}
}
