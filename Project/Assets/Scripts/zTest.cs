using UnityEngine;
using System.Collections;
using Magicolo;

public class zTest : MonoBehaviour {

	[Button("Test", "Test", NoPrefixLabel = true)] public bool test;
	void Test() {
		
	}
	
	void OnEnable() {
		Logger.Log("OnEnable");
	}
	
	void OnDisable() {
		Logger.Log("OnDisable");
	}
}
