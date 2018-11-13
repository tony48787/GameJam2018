using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

	public float speed = 10f;
	public float damage = 10f;
	private Rigidbody2D bulletBody;
	private float maxLifeTime = 3f;
	private float activeTime = 0f;
	// Use this for initialization
	void Start () {
		bulletBody = GetComponent<Rigidbody2D>();
		bulletBody.velocity = bulletBody.transform.up * speed;
	}
	
	// Update is called once per frame
	void Update () {
		activeTime += Time.deltaTime;
		if (activeTime > maxLifeTime) {
			Destroy(this.gameObject);
		}
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.collider.CompareTag("Enemy")) {
			Debug.Log("Bullet collide with Enemy");
			other.collider.SendMessageUpwards("OnDamaged", damage, SendMessageOptions.DontRequireReceiver);
		}
		Destroy(this.gameObject);
	}
}
