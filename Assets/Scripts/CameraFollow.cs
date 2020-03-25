using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
	public static CameraFollow ins;
	Transform trans;
	Transform transPlayer;
	Player scpPlayer;
	Vector3 offset, posPlayer;

	void Awake(){
		ins 	= this;
		trans 	= transform;
	}

	void Start(){
		InitFirst ();
	}

	public void InitFirst(){
		if (transPlayer == null) {
			transPlayer = GameObject.FindGameObjectWithTag ("Player").GetComponent<Transform>();
			scpPlayer 	= transPlayer.GetComponent<Player> ();
		}
		offset = transform.position - transPlayer.position;
	}


	void Update () {
		if (scpPlayer.bDeath)
			return;
		
		posPlayer.Set (transPlayer.position.x, 0, 0);
		trans.position = posPlayer + offset;
	}
}
