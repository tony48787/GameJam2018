using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FollowCinemachine : MonoBehaviour {

	private	CinemachineVirtualCamera cvCamera;
	private GameManager gm;
	private bool hasFollowObject;
	// Use this for initialization
	void Start () {
		gm = GameManager.instance;
		cvCamera = GetComponent<CinemachineVirtualCamera>();
		hasFollowObject = false;
	}


	
	// Update is called once per frame
	void Update () {
		if (!hasFollowObject) {
			cvCamera.Follow = GameObject.FindGameObjectWithTag("Player").transform;
			hasFollowObject = false;
		}
	}
}
