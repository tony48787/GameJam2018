using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour {

	public float recoverHp = 20f;
	private CircleCollider2D circleCollider2D;
    private GameManager gm;
    private PlayerStatus playerStatus;
	// Use this for initialization
	void Start () {
		circleCollider2D = GetComponent<CircleCollider2D>();
        gm = GameManager.instance;
    }
	
	void OnTriggerEnter2D(Collider2D other) {
		if (!other.isTrigger && other.CompareTag("Player")) {
            gm.playerStatus.currentHp += recoverHp;
			if (gm.playerStatus.currentHp >= gm.playerStatus.maxHp) {
                gm.playerStatus.currentHp = gm.playerStatus.maxHp;
            }
            Debug.Log("Current: " + gm.playerStatus.currentHp);
			Destroy(gameObject);
		}
	}
}
