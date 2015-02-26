using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class TroopBase : StateLayer, ISelectable {
	
	[SerializeField, PropertyField]
	bool selected;
	public bool Selected {
		get {
			return selected;
		}
		set {
			selected = value;
			
			if (GameManager.Debug) {
				renderer.material.color = selected ? Color.green : Color.red;
			}
		}
	}

	public float radius = 1.25F;
	public float sightRadius = 5;
	public float lightIntensity = 1;
	public float lightRange = 25;
	public float attackSpeed = 1;
	public float bulletLifeTime = 4;
	public int bulletBurst = 3;
	public float bulletSpeed = 2;
	public float damage = 5;
	public float maxHealth = 16;
	
	[Disable] public float health;
	
	[SerializeField, PropertyField(typeof(DisableAttribute))]
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
	
	[Disable] public TroopBase closestInRangeEnemy;
	[Disable] public int playerId;
	[Disable] public int groupId;
	[Disable] public int id;
	
	NavMeshAgent _navMeshAgent;
	public NavMeshAgent navMeshAgent { get { return _navMeshAgent ? _navMeshAgent : (_navMeshAgent = GetComponent<NavMeshAgent>()); } }

	Light _childLight;
	public Light childLight { get { return _childLight ? _childLight : (_childLight = GetComponentInChildren<Light>()); } }
	
	public static int priorityCounter;
	
	public override void OnAwake() {
		base.OnAwake();
		
		priorityCounter += 1;
		navMeshAgent.avoidancePriority = priorityCounter;
	}

	public void Spawned() {
		Target = transform.position;
		health = maxHealth;
	}
	
	public void Despawned() {
		Selected = false;
		TroopManager.RemoveTroopFromGroup(this);
		SwitchState(GetType().Name + "Idle");
	}
	
	public void Despawn() {
		TroopManager.Despawn(this);
	}
	
	public bool CheckForEnemies() {
		closestInRangeEnemy = TroopManager.GetClosestInRangeEnemy(this);
		
		return closestInRangeEnemy != null;
	}

	public Rect GetRect() {
		return Rect.MinMaxRect(transform.position.x - transform.lossyScale.x / 2, transform.position.z - transform.lossyScale.z / 2, transform.position.x + transform.lossyScale.x / 2, transform.position.z + transform.lossyScale.z / 2);
	}
	
	public Rect GetSightRect() {
		return Rect.MinMaxRect(transform.position.x - sightRadius, transform.position.z - sightRadius, transform.position.x + sightRadius, transform.position.z + sightRadius);
	}
	
	public void Damage(float damage) {
		health -= damage;
		
		if (health <= 0) {
			if (!NetworkController.instance.isConnected) {
				Kill();
			}
			else {
				if (playerId == NetworkController.instance.clientController.playerId) {
					NetworkController.instance.clientController.killUnit(playerId, id);
				}
			}
			
			return;
		}
	}

	public void Kill() {
		SwitchState(GetType().Name + "Dead");
	}

	public void Move(Vector3 position) {
		transform.position = position;
	}

	public void SetTarget(Vector3 target) {
		Target = target;
	}

	public void FadeLight(float intensity, float range, bool enabled) {
		childLight.intensity = intensity;
		childLight.range = range;
		childLight.enabled = enabled;
	}
}
