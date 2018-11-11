using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour {

	public float walkSpeed;
	public Collider2D attackCollider;
	public Rigidbody2D bulletType;
	public Transform firePosition;
	private Rigidbody2D rb2d;
	private Animator animator;
	private bool attacking = false;
	private bool holdingAttack = false;
	private float attackAnimCountdown = 0f;
	public float attackAnimDuration = 4f/12f;
	private bool holdingShoot = false;
	private float shootCoolDownCountdown = 0f;
	public float shootCoolDownDuration = 0.2f;
	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		attackCollider.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		RotatePlayerToMouse();
		HandleShoot();
		HandleAttack();
	}

	void FixedUpdate() {
		float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
		Vector2 movement = new Vector2(moveHorizontal, moveVertical);
		rb2d.AddForce(movement * walkSpeed);
		animator.SetFloat("Speed", rb2d.velocity.magnitude);
	}

	void RotatePlayerToMouse() {
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 direction = new Vector2(
			mousePosition.x - transform.position.x,
			mousePosition.y - transform.position.y
		);
		transform.up = direction;
	}

	void HandleShoot() {
		if (Input.GetKeyDown("mouse 0")) {
			holdingShoot = true;
			animator.SetBool("IsShooting", true);
		}
		if (Input.GetKeyUp("mouse 0")) {
			holdingShoot = false;
			animator.SetBool("IsShooting", false);
		}
		if (holdingShoot && !holdingAttack) {
			if (shootCoolDownCountdown > 0) {
				shootCoolDownCountdown -= Time.deltaTime;
			}
			else {
				shootCoolDownCountdown = shootCoolDownDuration;
				FireBullet();
			}
		}
	}

	void FireBullet() {
		Rigidbody2D bullet = Instantiate(bulletType, firePosition.position, firePosition.rotation);
	}

	void HandleAttack() {
		if (Input.GetKeyDown("mouse 1")) {
			// pressed attack key
			holdingAttack = true;
		}
		if (Input.GetKeyUp("mouse 1")) {
			// released attack key
			holdingAttack = false;
		}
		if (!attacking) {
			// not playing attack animation, set trigger box for attack
			if (holdingAttack && !holdingShoot) {
				animator.SetBool("IsFighting", true);
				attackCollider.enabled = true;
				attacking = true;
				attackAnimCountdown = attackAnimDuration;
			}
		}
		if (attacking) {
			// playing attack animation
			if (attackAnimCountdown > 0) {
				// within attacking movement animation, continue countdown
				attackAnimCountdown -= Time.deltaTime;
			}
			else {
				// finished attacking movement animation, reset countdown
				attackAnimCountdown = attackAnimDuration;
				attackCollider.enabled = false;
				attacking = false;
				animator.SetBool("IsFighting", false);
			}
		}
		
	}
}
