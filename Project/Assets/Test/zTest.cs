using UnityEngine;
using System.Collections;
using Magicolo;

public class zTest : MonoBehaviour {

	public int renderQueue = 1000;
	
	[Button("Test", "Test", NoPrefixLabel = true)] public bool test;
	void Test() {
		Logger.Log(3 / 2);
	}
	
	void OnEnable() {
	}
	
	void OnDisable() {
	}
}
