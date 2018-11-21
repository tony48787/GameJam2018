using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBulletController : MonoBehaviour {

	private GameManager gm;
	private WeaponStatus status;

	public float speed = 10f;
	public float damage = 50f;
	public GameObject explosion;
	private Rigidbody2D bulletBody;
	private float maxLifeTime = 3f;
	private float activeTime = 0f;
	// Use this for initialization
	void Start () {
		gm = GameManager.instance;
		status = gm.weaponStatus;

		bulletBody = GetComponentInChildren<Rigidbody2D>();
		bulletBody.velocity = bulletBody.transform.up * status.chargeBulletSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		if (activeTime < maxLifeTime) {
			activeTime += Time.deltaTime;
		}
		else {
			Explode();
		}
	}

	 void OnCollisionEnter2D(Collision2D other) {
		Debug.Log("Collide with sth");
		Explode();
	}

	void Explode() {
		GameObject exp = Instantiate(explosion, transform.position, transform.rotation);
		ExplosionController ec = exp.GetComponent<ExplosionController>();
		ec.damage = status.chargeBulletDamage;
		Destroy(transform.parent.gameObject);
	}
}
