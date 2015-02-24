using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class TroopBase : StateLayer, ISelectable {
	
	public bool debug;
	
	[SerializeField, PropertyField]
	bool selected;
	public bool Selected {
		get {
			return selected;
		}
		set {
			selected = value;
			
			if (debug) {
				renderer.material.color = selected ? Color.green : Color.red;
			}
		}
	}

	[SerializeField, PropertyField]
	Vector3 target;
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

	public static int priorityCounter;
	
	public override void OnAwake() {
		base.OnAwake();
		
		priorityCounter += 1;
		navMeshAgent.avoidancePriority = priorityCounter;
	}
}
