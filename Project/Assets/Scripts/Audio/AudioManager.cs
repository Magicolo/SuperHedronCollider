using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class AudioManager : MonoBehaviourExtended {

	[Button("PlayDrum", "PlayDrum", NoPrefixLabel = true)] public bool playDrum;
	void PlayDrum() {
		PureData.PlaySequence("Drum");
	}
	
	void Awake() {
		PureData.OpenPatch("main");
	}
}

