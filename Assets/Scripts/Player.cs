using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	Transform trans;
	public Animator animator;
	public Vector3 gravity = new Vector3 (0f, -20f, 0f);
	public Vector3 JUMP_VELOCITY = new Vector3 (0, 10f, 0f);
	Vector3 velocity = Vector3.zero;
	public float MAX_VELOCITY_GRAVITY = 20f;
	public float MOVE_SPEED = 1f;
	[HideInInspector] public bool bDeath = true;
	public LayerMask maskObstacle;
	int layerObstacle;
	Rigidbody2D rb;
	Vector3 orgPos;

	public List<Animator> listAnimator = new List<Animator> ();
	int listIdx = 0;

	// Use this for initialization
	void Start () {
		InitFirst ();
	}

	public void InitFirst(){
		if (trans == null) {
			trans = transform;
		}
		velocity = Vector3.zero;
		bDeath = false;
		rb = GetComponent<Rigidbody2D> ();
		orgPos = trans.position;

		//2^layer = mask.value 
		//  layer = log2(mask.value)
		layerObstacle = (int)Mathf.Log (maskObstacle.value, 2);
	
		DisableControl ();
	}


	//--------------------------------
	public void DisableControl()
	{
		bDeath = true;
		UiScore.ins.VisibleRestart(this, true);
	}

	public void EnableControl()
	{
		bDeath = false;
		UiScore.ins.VisibleRestart(this, false);
	}

	public void Reset()
	{
		trans.position 	= orgPos;
		velocity 		= Vector3.zero;
		bDeath 			= false;
		rb.gravityScale = 0;
		animator.SetInteger ("aniDie", 0);
		animator.SetTrigger ("aniFly");
	}

	//--------------------------------	
	// Update is called once per frame
	void Update () {
		if (bDeath) {
			if (Input.GetMouseButtonDown (1)) {
				listIdx++;
				if (listIdx >= listAnimator.Count) {
					listIdx = 0;
				}
				animator.gameObject.SetActive (false);
				animator = listAnimator [listIdx];
				animator.gameObject.SetActive (true);
			}
			return;
		}


		//1. Input key.
		velocity += gravity * Time.deltaTime;
		velocity.x = MOVE_SPEED;
		if (Input.GetMouseButtonDown (0) || Input.GetKeyDown(KeyCode.Space)) {
			velocity = JUMP_VELOCITY;
			animator.SetTrigger ("aniFly");
		} else if (velocity.y > 0 && (Input.GetMouseButtonUp (0) || Input.GetKeyUp(KeyCode.Space))) {
			velocity = velocity / 2f;
		}
		//Debug.Log (velocity.y);
		velocity = Vector3.ClampMagnitude(velocity, MAX_VELOCITY_GRAVITY);

		//2. move, gravity
		trans.position += velocity * Time.deltaTime;


		//3. Face direct
		float _angle = 0f;
		//if (velocity.y > 0) {
		//	_angle = Mathf.Lerp (30, 0,  velocity.y / MAX_VELOCITY_GRAVITY);
		//}else 
		if (velocity.y < 0f) {
			_angle = Mathf.Lerp (0, -80f, -velocity.y / MAX_VELOCITY_GRAVITY);
		}
		trans.rotation = Quaternion.Euler (0, 0, _angle);
	}

	public bool debugGodMode = false;
	void OnCollisionEnter2D(Collision2D _col){	
		if (debugGodMode) {
			return;
		}
		//Debug.Log ("OnCollisionEnter2D:"+_col.collider.name);
		if (!bDeath && _col.collider.gameObject.layer == layerObstacle) {
			bDeath = true;
			//Debug.Log (1);
			animator.SetInteger ("aniDie", 1);
			rb.gravityScale = 1;

			//Debug.Log ("UiRestart ");
			UiScore.ins.VisibleRestart(this, true);
		}
	}

	void OnTriggerEnter2D(Collider2D _col){
		//Debug.Log ("OnTriggerEnter2D:" +  _col.name);
		UiScore.ins.SetScore(1);
	}
}
