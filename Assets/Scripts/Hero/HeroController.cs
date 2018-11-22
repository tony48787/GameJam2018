using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroController : MonoBehaviour {

	private GameManager gm;
	private PlayerStatus status;
	private PrefabManager pm;

	private HealthBar healthBar;
	private ChargeBar chargeBar;

	public float maxHp = 100f;
	public float hp = 100f;
	public Text hpText;
	public float maxChargeBarValue = 1000f;
	public Text chargeText;
	private float currentChargeValue = 0f;
	public float chargeBarIncreaseRate = 5f;
	public float chargeBarDecreaseRate = 10f;
	private bool canChargeAttack = false;
	private bool holdingCharge = false;
	public float chargeCoolDownDuration = 3f;
	private float chargeCoolDownCountdown = 0f;
	public float walkSpeed;
	private Collider2D attackCollider;
	public Transform firePosition;
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
		gm = GameManager.instance;
		status = gm.playerStatus;
		pm = PrefabManager.instance;

		rb2d = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		attackCollider = transform.Find("AttackCollider").GetComponent<Collider2D>();
		attackCollider.enabled = false;
		
		// UpdateHpText();
		// UpdateChargeText();
		healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();
		chargeBar = GameObject.Find("ChargeBar").GetComponent<ChargeBar>();
		UpdateHealthBar();
		UpdateChargeBar();
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
		// UpdateHpText();
		// UpdateChargeText();
		UpdateHealthBar();
		UpdateChargeBar();
	}

	void FixedUpdate() {
		float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
		Vector2 movement = new Vector2(moveHorizontal, moveVertical);
		rb2d.AddForce(movement * walkSpeed);
		animator.SetFloat("Speed", rb2d.velocity.magnitude);

        if (transform.position.y > GameManager.instance.vertExtent * 0.9f)
            transform.position = new Vector3(transform.position.x, GameManager.instance.vertExtent * 0.9f, transform.position.z);
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
		switch (status.currentAttackType) {
			case PlayerAttackType.Shoot:
				status.currentAttackType = PlayerAttackType.Slice;
				Debug.Log("Change attack type from shoot to slice");
				break;
			case PlayerAttackType.Slice:
				status.currentAttackType = PlayerAttackType.Shoot;
				Debug.Log("Change attack type from slice to shoot");
				break;
			default:
				status.currentAttackType = PlayerAttackType.Shoot;
				Debug.Log("Change attack type from unknown to slice");
				break;
		}
		holdingAttack = false;
		holdingShoot = false;
	}

	void HandleShoot() {
		if (Input.GetKeyDown("mouse 0") && status.currentAttackType == PlayerAttackType.Shoot) {
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
				shootCoolDownCountdown = status.shootCoolDownDuration;
				FireBullet();
			}
		}
		else {
			animator.SetBool("IsShooting", false);
		}
	}

	void FireBullet() {
		GameObject bullet = Instantiate(PrefabManager.instance.bulletType, firePosition.position, firePosition.rotation);
	}

	void HandleAttack() {
		if (Input.GetKeyDown("mouse 0") && status.currentAttackType == PlayerAttackType.Slice) {
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
				if (status.currentChargeBarValue < status.maxChargeBarValue) {
					status.currentChargeBarValue += chargeBarIncreaseRate;
				}
				else {
					status.currentChargeBarValue = status.maxChargeBarValue;
				}
			}
			else {
				if (status.currentChargeBarValue > 0) {
					status.currentChargeBarValue -= chargeBarDecreaseRate;
				}
				else {
					status.currentChargeBarValue = 0;
				}
			}
		}
		canChargeAttack = (status.currentChargeBarValue >= status.maxChargeBarValue);

		// handle charge attack skill
		if (Input.GetKey("mouse 0") && canChargeAttack) {
			if (status.currentAttackType == PlayerAttackType.Shoot) {
				Debug.Log("Do Charge Shoot!");
				// TODO: handle charge shoot
				animator.Play("HeroChargeShoot");
				GameObject explosiveBullet = Instantiate(pm.explosiveBulletType,
					firePosition.position, firePosition.rotation);
			}
			else if (status.currentAttackType == PlayerAttackType.Slice) {
				Debug.Log("Do Charge Slice!");
				// TODO: handle charge slice
				// spawn sword wind
				animator.Play("HeroChargeFight");
				GameObject swordWind = Instantiate(pm.swordWindType,
					transform.position, transform.rotation);
			}
			status.currentChargeBarValue = 0;
			canChargeAttack = false;
			chargeCoolDownCountdown = status.chargeCoolDownDuration;
		}
	}

	void UpdateHpText() {
		hpText.text = "HP: " + hp.ToString();
	}

	void UpdateChargeText() {
		chargeText.text = currentChargeValue.ToString() + "/" + maxChargeBarValue.ToString();
	}

	void UpdateHealthBar() {
		healthBar.SetHealthRatio(status.currentHp/status.maxHp);
	}

	void UpdateChargeBar() {
		chargeBar.SetChargeRatio(status.currentChargeBarValue/status.maxChargeBarValue);
	}

	void OnDamaged(DamageMessage msg) {
		float receivedDamage = msg.damage;
		Vector2 repelForce = msg.repelForce;
		if (!isInvincible) {
			// take damage
			status.currentHp -= receivedDamage;
			isInvincible = true;
			isTransparent = true;
			invincibleCountdown = status.invincibleDuration;
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
				invincibleCountdown = status.invincibleDuration;
				isInvincible = false;
				Color tmp = spriteRenderer.color;
				tmp.a = 1f;
				spriteRenderer.color = tmp;
			}
		}
	}
}
