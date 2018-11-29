using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	private GameManager gm;

	private bool isEnter;

	// Use this for initialization
	void Start () {
		gm = GameManager.instance;
		isEnter = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnPointerEnter(PointerEventData eventData)
    {
		if (gm) {
			isEnter = true;
			gm.mouseInputStatus = MouseInputState.InteractUI;
			gm.UpdateCursorTexture();
		}
    }

    public void OnPointerExit(PointerEventData eventData)
    {
		if (gm) {
			isEnter = false;
			gm.mouseInputStatus = MouseInputState.Attack;
			gm.UpdateCursorTexture();
		}
    }

	private void OnDisable() {
		// when the clickable UI is disabled, change cursor back to attack type
		if (gm) {
			isEnter = false;
			gm.mouseInputStatus = MouseInputState.Attack;
			gm.UpdateCursorTexture();
		}
	}
}
