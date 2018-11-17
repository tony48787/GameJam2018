using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroController : MonoBehaviour {

	public float hp = 100f;
	public Text hpText;
	public float maxChargeBarValue = 1000f;
	public Text chargeText;
	private float currentChargeValue = 0f;
	private float chargeBarIncreaseRate = 5f;
	private float chargeBarDecreaseRate = 10f;
	private bool canChargeAttack = false;
	private bool holdingCharge = false;
	public float chargeCoolDownDuration = 3f;
	private float chargeCoolDownCountdown = 0f;
	public float walkSpeed;
	public Collider2D attackCollider;
	public GameObject bulletType;
	public Transform firePosition;
	public GameObject swordWindType;
	public GameObject explosiveBulletType;
	private Rigidbody2D rb2d;
	private Animator animator;
	private SpriteRenderer spriteRenderer;
	private PlayerAttackType currentAttackType = PlayerAttackType.Shoot;
	private bool attacking = false;
	private bool holdingAttack = false;
	private float attackAnimCountdown = 0f;
	public float attackAnimDuration = 4f/12f;
	private bool holdingShoot = false;
	private float shootCoolDownCountdown = 0f;
	public float shootCoolDownDuration = 0.2f;
	private bool isInvincible = false;
	public float invincibleDuration = 2f;
	private float invincibleCountdown = 0f;
	private bool isTransparent = false;
	private float spriteBlinkDuration = 0.2f;
	private float spriteBlinkCountdown = 0f;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		attackCollider.enabled = false;
		UpdateHpText();
		UpdateChargeText();
	}
	
	// Update is called once per frame
	void Update () {
		RotatePlayerToMouse();
		if (Input.GetKeyDown(KeyCode.E)) {
			SwitchAttackType();
		}
		HandleShoot();
		HandleAttack();
		HandleChargeSkill();
		HandleInvinciblePeriod();
		UpdateHpText();
		UpdateChargeText();
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

	void SwitchAttackType() {
		switch (currentAttackType) {
			case PlayerAttackType.Shoot:
				currentAttackType = PlayerAttackType.Slice;
				Debug.Log("Change attack type from shoot to slice");
				break;
			case PlayerAttackType.Slice:
				currentAttackType = PlayerAttackType.Shoot;
				Debug.Log("Change attack type from slice to shoot");
				break;
			default:
				currentAttackType = PlayerAttackType.Shoot;
				Debug.Log("Change attack type from unknown to slice");
				break;
		}
		holdingAttack = false;
		holdingShoot = false;
	}

	void HandleShoot() {
		if (Input.GetKeyDown("mouse 0") && currentAttackType == PlayerAttackType.Shoot) {
			holdingShoot = true;
		}
		if (Input.GetKeyUp("mouse 0")) {
			holdingShoot = false;
		}
		if (holdingShoot && !holdingCharge) {
			animator.SetBool("IsShooting", true);
			if (shootCoolDownCountdown > 0) {
				shootCoolDownCountdown -= Time.deltaTime;
			}
			else {
				shootCoolDownCountdown = shootCoolDownDuration;
				FireBullet();
			}
		}
		else {
			animator.SetBool("IsShooting", false);
		}
	}

	void FireBullet() {
		GameObject bullet = Instantiate(bulletType, firePosition.position, firePosition.rotation);
	}

	void HandleAttack() {
		if (Input.GetKeyDown("mouse 0") && currentAttackType == PlayerAttackType.Slice) {
			// pressed attack key
			holdingAttack = true;
		}
		if (Input.GetKeyUp("mouse 0")) {
			// released attack key
			holdingAttack = false;
		}
		if (!attacking) {
			// not playing attack animation, set trigger box for attack
			if (holdingAttack && !holdingCharge) {
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

	void HandleChargeSkill() {
		if (Input.GetKeyDown("mouse 1")) {
			holdingCharge = true;
		}
		if (Input.GetKeyUp("mouse 1")) {
			holdingCharge = false;
		}

		// cool down time after charge attack
		if (chargeCoolDownCountdown > 0) {
			chargeCoolDownCountdown -= Time.deltaTime;
		}
		else {
			chargeCoolDownCountdown = 0;
			if (holdingCharge) {
				if (currentChargeValue < maxChargeBarValue) {
					currentChargeValue += chargeBarIncreaseRate;
				}
				else {
					currentChargeValue = maxChargeBarValue;
				}
			}
			else {
				if (currentChargeValue > 0) {
					currentChargeValue -= chargeBarDecreaseRate;
				}
				else {
					currentChargeValue = 0;
				}
			}
		}
		canChargeAttack = (currentChargeValue >= maxChargeBarValue);

		// handle charge attack skill
		if (Input.GetKey("mouse 0") && canChargeAttack) {
			if (currentAttackType == PlayerAttackType.Shoot) {
				Debug.Log("Do Charge Shoot!");
				// TODO: handle charge shoot
				GameObject explosiveBullet = Instantiate(explosiveBulletType, firePosition.position, firePosition.rotation);
			}
			else if (currentAttackType == PlayerAttackType.Slice) {
				Debug.Log("Do Charge Slice!");
				// TODO: handle charge slice
				// spawn sword wind
				GameObject swordWind = Instantiate(swordWindType, transform.position, transform.rotation);
			}
			currentChargeValue = 0;
			canChargeAttack = false;
			chargeCoolDownCountdown = chargeCoolDownDuration;
		}
	}

	void UpdateHpText() {
		hpText.text = "HP: " + hp.ToString();
	}

	void UpdateChargeText() {
		chargeText.text = currentChargeValue.ToString() + "/" + maxChargeBarValue.ToString();
	}

	void OnDamaged(DamageMessage msg) {
		float receivedDamage = msg.damage;
		Vector2 repelForce = msg.repelForce;
		if (!isInvincible) {
			// take damage
			hp -= receivedDamage;
			isInvincible = true;
			isTransparent = true;
			invincibleCountdown = invincibleDuration;
			spriteBlinkCountdown = spriteBlinkDuration;

			// repel from enemy
			rb2d.velocity = new Vector2(0f, 0f);
			rb2d.AddForce(repelForce, ForceMode2D.Impulse);
		}
	}
	
	void HandleInvinciblePeriod() {
		if (isInvincible) {
			if (invincibleCountdown > 0) {
				invincibleCountdown -= Time.deltaTime;
				if (spriteBlinkCountdown > 0) {
					spriteBlinkCountdown -= Time.deltaTime;
					Color tmp = spriteRenderer.color;
					if (isTransparent) {
						tmp.a = 0.3f;
					}
					else {
						tmp.a = 1f;
					}
					spriteRenderer.color = tmp;
				}
				else {
					spriteBlinkCountdown = spriteBlinkDuration;
					isTransparent = !isTransparent;
				}
			}
			else {
				invincibleCountdown = invincibleDuration;
				isInvincible = false;
				Color tmp = spriteRenderer.color;
				tmp.a = 1f;
				spriteRenderer.color = tmp;
			}
		}
	}
}
