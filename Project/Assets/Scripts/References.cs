﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class References : MonoBehaviourExtended {

	static References instance;
	static References Instance {
		get {
			if (instance == null) {
				instance = FindObjectOfType<References>();
			}
			return instance;
		}
	}
}

