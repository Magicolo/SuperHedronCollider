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
	
	public float sightRadius = 5;
	public float attackSpeed = 2;
	public float bulletSpeed = 2;
	public float damage = 5;
	public float maxHealth = 16;
	
	[Disable] public float health;
	[Disable] public TroopBase closestInRangeEnemy;
	[Disable] public int id;
	[Disable] public int playerId;
	
	NavMeshAgent _navMeshAgent;
	public NavMeshAgent navMeshAgent { get { return _navMeshAgent ? _navMeshAgent : (_navMeshAgent = GetComponent<NavMeshAgent>()); } }

	public static int priorityCounter;
	
	public override void OnAwake() {
		base.OnAwake();
		
		priorityCounter += 1;
		navMeshAgent.avoidancePriority = priorityCounter;
	}

	public void OnSpawned() {
		SwitchState(GetType().Name + "Idle");
		health = maxHealth;
	}
	
	public void Despawn() {
		TroopManager.Despawn(this);
	}
	
	public bool CheckForEnemies(){
		closestInRangeEnemy = TroopManager.GetClosestInRangeEnemy(this);
		
		return closestInRangeEnemy != null;
	}
	
	public void ReceiveDamage(float damage) {
		health -= damage;
		
		if (health <= 0) {
			SwitchState(GetType().Name + "Dead");
			return;
		}
	}
}
