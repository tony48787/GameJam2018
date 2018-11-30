using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleEnemyController : MonoBehaviour {

	public Text hpText;
	public float maxHp = 100f;
	public float hp = 100f;
	public float damage = 10f;
	public float repelForce = 70f;
	private Rigidbody2D rb2d;
	public HealthBar healthBarType;
	private HealthBar healthBar;

    public int coinDrop = 10;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
		hp = maxHp;
		healthBar = Instantiate(healthBarType,
			new Vector2(transform.position.x, transform.position.y + 1),
			new Quaternion(0, 0, 0, 0));
		UpdateHpBar();
	}
	
	// Update is called once per frame
	void Update () {
		healthBar.transform.position = new Vector2(transform.position.x, transform.position.y + 1);
	}

	void UpdateHpText() {
		hpText.text = "Enemy HP: " + hp.ToString();
	}

	void UpdateHpBar() {
		healthBar.SetHealthRatio(hp/maxHp);
	}

	void OnDamaged(DamageMessage msg) {
		float receivedDamage = msg.damage;
        Debug.Log(receivedDamage);
		Vector2 repelForce = msg.repelForce;
		hp -= receivedDamage;
		rb2d.AddForce(repelForce, ForceMode2D.Impulse);
		UpdateHpBar();

        if (hp <= 0)
        {
            SpawnHealthPack();

            GameManager.instance.IncrementCoinBy(coinDrop);
            FindObjectOfType<EnemySpawner>().UpdateCurrentCountBy();
            Destroy(gameObject);
            Destroy(healthBar.gameObject);
        }
        
    }

    private void SpawnHealthPack()
    {
        int percentage = 5;
        if (UnityEngine.Random.Range(0, 100) >= 100 - percentage)
        {
            GameObject healthPack = Instantiate(PrefabManager.instance.healthPackType, transform.position, Quaternion.identity);
        }
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
