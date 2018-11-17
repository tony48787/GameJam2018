using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleEnemyController : MonoBehaviour {

	public Text hpText;
	public float hp = 100f;
	public float damage = 10f;
	public float repelForce = 70f;
	private Rigidbody2D rb2d;
	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
		UpdateHpText();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void UpdateHpText() {
		hpText.text = "Enemy HP: " + hp.ToString();
	}

	void OnDamaged(DamageMessage msg) {
		float receivedDamage = msg.damage;
		Vector2 repelForce = msg.repelForce;
		hp -= receivedDamage;
		rb2d.AddForce(repelForce, ForceMode2D.Impulse);
		UpdateHpText();
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.collider.CompareTag("Player")) {
			Debug.Log("Enemy attacks player");
			// repel player from this collider
			Rigidbody2D otherRb2d = other.collider.GetComponent<Rigidbody2D>();
			Vector2 direction = otherRb2d.position - new Vector2 (transform.position.x, transform.position.y);
			direction.Normalize();
			DamageMessage msg = new DamageMessage(damage, direction * repelForce);
			other.collider.SendMessageUpwards("OnDamaged", msg, SendMessageOptions.DontRequireReceiver);
		}
	}
}
