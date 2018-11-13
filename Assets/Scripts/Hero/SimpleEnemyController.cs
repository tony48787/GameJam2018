using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleEnemyController : MonoBehaviour {

	public Text hpText;
	public float hp = 100f;
	public float damage = 10f;
	public float repelForce = 70f;
	// Use this for initialization
	void Start () {
		UpdateHpText();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void UpdateHpText() {
		hpText.text = "Enemy HP: " + hp.ToString();
	}

	void OnDamaged(float damage) {
		hp -= damage;
		UpdateHpText();
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.collider.CompareTag("Player")) {
			Debug.Log("Enemy attacks player");
			object[] args = new object[2];
			args[0] = damage;
			// repel player from this collider
			Rigidbody2D otherRb2d = other.collider.GetComponent<Rigidbody2D>();
			Vector2 direction = otherRb2d.position - new Vector2 (transform.position.x, transform.position.y);
			direction.Normalize();
			args[1] = direction * repelForce;
			other.collider.SendMessageUpwards("OnDamaged", args, SendMessageOptions.DontRequireReceiver);
		}
	}
}
