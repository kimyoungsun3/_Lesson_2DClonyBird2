using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMove : MonoBehaviour {
	public float moveSpeed = .5f;
	Transform trans;
	Player scpPlayer;
	Vector3 pos;

	void Start(){
		trans = transform;
		scpPlayer = GameObject.FindWithTag ("Player").GetComponent<Player> ();
	}

	void Update () {
		if (scpPlayer.bDeath) {
			return;
		}

		pos 			= trans.position;
		pos.x 			+= moveSpeed * Time.deltaTime;
		trans.position 	= pos;
	}
}
