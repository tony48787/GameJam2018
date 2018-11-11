using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour {

	public float damage = 10;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log("Trigger eneter");
		if (!other.isTrigger && other.CompareTag("Enemy")) {
			other.SendMessageUpwards("Damage", damage);
			Debug.Log("Damage target");
		}
	}
}
