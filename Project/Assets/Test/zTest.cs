using UnityEngine;
using System.Collections;
using Magicolo;

public class zTest : MonoBehaviour {

	public int renderQueue = 1000;
	
	[Button("Test", "Test", NoPrefixLabel = true)] public bool test;
	void Test() {
		renderer.sharedMaterial.renderQueue = renderQueue;
	}
	
	void OnEnable() {
	}
	
	void OnDisable() {
	}
}
