using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class TroopBase : StateLayer, ISelectable {
	
	public bool Selected { get; set; }
	
	public Vector3 target;
	public Vector3 Target {
		get {
			return target;
		}
		set {
			target = value;
			navMeshAgent.SetDestination(target);
		}
	}
	
	NavMeshAgent _navMeshAgent;
	public NavMeshAgent navMeshAgent { get { return _navMeshAgent ? _navMeshAgent : (_navMeshAgent = GetComponent<NavMeshAgent>()); } }
}
