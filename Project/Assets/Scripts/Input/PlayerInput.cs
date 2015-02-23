using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class PlayerInput : StateLayer {
	
	public int mouseSelectButton = 0;
	public int mouseActionButton = 1;
	public List<ISelectable> selected = new List<ISelectable>();
}
