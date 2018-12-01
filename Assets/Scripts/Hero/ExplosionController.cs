using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour {

	public float damage = 50f;
	public float forceMagnitude = 500f;
	public float forceVariation = 200f;
	private float maxLifeTime = 5f/12f;		// frame set in animation
	private float activeTime = 0f;

	private CircleCollider2D circleCollider2D;
	private PointEffector2D pointEffector2D;

	// Use this for initialization
	void Start () {
		circleCollider2D = GetComponent<CircleCollider2D>();
		pointEffector2D = GetComponent<PointEffector2D>();
		pointEffector2D.forceMagnitude = forceMagnitude;
		pointEffector2D.forceVariation = forceVariation;

        GetComponent<AudioSource>().Play();

    }
	
	// Update is called once per frame
	void Update () {
		if (activeTime < maxLifeTime) {
			activeTime += Time.deltaTime;
		}
		else {
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (!other.isTrigger && other.CompareTag("Enemy")) {
			DamageMessage msg = new DamageMessage(damage);
			other.SendMessageUpwards("OnDamaged", msg);
		}
	}
}
