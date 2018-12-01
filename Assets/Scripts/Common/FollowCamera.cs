using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

	private GameObject player;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (player == null) {
			player = GameObject.FindGameObjectWithTag("Player");
		}
		if (player) {
			transform.localPosition = new Vector3(
				player.transform.localPosition.x, player.transform.localPosition.y, transform.localPosition.z);
		}	
	}
}
