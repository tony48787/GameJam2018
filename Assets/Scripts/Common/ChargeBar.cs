using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeBar : MonoBehaviour {

	private Transform bar;
	private Transform barSprite;
	private SpriteRenderer barSpriteRenderer;
	private Transform border;
	private Transform background;
	public float width = 100f;
	public float height = 10f;
	public float borderSize = 2f;
	private float chargeRatio = 1f;
	private float currentChargeRatio;
	public float lerpRate = 1f;
	
	// Use this for initialization
	void Start () {
		bar = transform.Find("Bar");
		barSprite = bar.Find("BarSprite");
		barSpriteRenderer = barSprite.GetComponent<SpriteRenderer>();
		border = transform.Find("Border");
		background = transform.Find("Background");
		border.localScale = new Vector3(width+borderSize*2, height+borderSize*2);
		barSprite.localScale = new Vector3(width, height);
		background.localScale = new Vector3(width, height);
		bar.localPosition = new Vector3(bar.localPosition.x*width/100f, 0);
		barSprite.localPosition = new Vector3(barSprite.localPosition.x*width/100f, 0);

		currentChargeRatio = chargeRatio;
	}
	
	// Update is called once per frame
	void Update () {
		if (currentChargeRatio != chargeRatio) {
			currentChargeRatio = chargeRatio;
			bar.localScale = new Vector3(currentChargeRatio, 1f);
			ChangeBarColor();
		}
	}

	void ChangeBarColor() {
		Color tmp = barSpriteRenderer.color;
		if (currentChargeRatio == 1f) {
			tmp.g = 1f;
		}
		else {
			tmp.g = 0.6f;
		}
		barSpriteRenderer.color = tmp;
	}

	public void SetChargeRatio(float chargeRatio) {
		this.chargeRatio = chargeRatio;
	}
}
