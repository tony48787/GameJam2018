using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {

	private Transform bar;
	private Transform barSprite;
	private Transform border;
	private Transform background;
	public float width = 100f;
	public float height = 10f;
	public float borderSize = 2f;
	private float hpRatio = 1f;
	private float currentHpRatio;
	public float lerpRate = 1f;
	
	// Use this for initialization
	void Start () {
		bar = transform.Find("Bar");
		barSprite = bar.Find("BarSprite");
		border = transform.Find("Border");
		background = transform.Find("Background");
		border.localScale = new Vector3(width+borderSize*2, height+borderSize*2);
		barSprite.localScale = new Vector3(width, height);
		background.localScale = new Vector3(width, height);

		currentHpRatio = hpRatio;
	}
	
	// Update is called once per frame
	void Update () {
		if (Mathf.Abs(currentHpRatio - hpRatio) > 0.01) {
			currentHpRatio = Mathf.Lerp(currentHpRatio, hpRatio, lerpRate);
			if (currentHpRatio < 0) {
				currentHpRatio = 0;
			}
			bar.localScale = new Vector3(currentHpRatio, 1f);
		}
	}

	public void SetHealthRatio(float hpRatio) {
		this.hpRatio = hpRatio;
	}
}
