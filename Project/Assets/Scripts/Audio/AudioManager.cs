using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class AudioManager : MonoBehaviourExtended {

	PureDataSequenceItem sequence;
	int currentStep = -1;
	
	[Button("PlaySequence", "PlaySequence", NoPrefixLabel = true)] public bool playSequence;
	void PlaySequence() {
		sequence = PureData.PlaySequence("Music");
	}
	
	[Button("SetRandomPattern", "SetRandomPattern", NoPrefixLabel = true)] public bool setRandomPattern;
	void SetRandomPattern() {
		int sendSize = 2;
		int subdivision = Random.Range(2, 16);
		float[,] pattern = new float[sendSize, subdivision];
		
		for (int row = 0; row < sendSize; row++) {
			for (int column = 0; column < subdivision; column++) {
				pattern[row, column] = Random.Range(0, 10) + Random.Range(10, 500) * row;
			}
		}
		
		if (sequence != null) {
			sequence.ApplyOptions(PureDataOption.TrackPattern(1, 2, pattern));
		}
	}
	
	void Awake() {
		PureData.OpenPatch("main");
	}
	
	void Update() {
		if (sequence != null) {
			if (currentStep != sequence.CurrentStepIndex) {
				currentStep = sequence.CurrentStepIndex;
				
				if (currentStep == sequence.GetStepCount() - 1) {
					for (int trackIndex = 0; trackIndex < sequence.GetTrackCount(); trackIndex++) {
						int patternIndex = Random.Range(0, sequence.GetPatternCount(trackIndex));
						
						for (int stepIndex = 0; stepIndex < sequence.GetStepCount(); stepIndex++) {
//							sequence.ApplyOptions(PureDataOption.StepPattern(trackIndex, stepIndex, patternIndex));
						}
					}
				}
			}
		}
	}
}

