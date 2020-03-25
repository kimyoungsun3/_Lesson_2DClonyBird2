using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGLooper : MonoBehaviour {
	public static BGLooper ins;
	Transform trans;

	[Header("bg sky")]
	public float skyWidth = 6f;
	public List<Transform> listSkyTrans = new List<Transform>();
	Vector3 startSkyPos;

	[Header("bg ground")]
	public float groundWidth = 6f;
	public List<Transform> listGroundTrans = new List<Transform>();
	List<Vector3> listGroundPos = new List<Vector3> ();
	Vector3 startGroundPos;

	[Header("Pipe")]
	public LayerMask maskPipe;
	public Vector2 minMax;
	public Vector2 pipeWidthMinMax = new Vector2(8f, 10f);
	public float pipeWidthInter = 1000f;
	public float pipeWidth = 10f;
	public List<Transform> listPipeTrans = new List<Transform>();
	public Vector3 pipeGap = new Vector3 (0f, 10f, 0f);
	Vector3 startPipePos;

	void Awake()
	{
		ins = this;
	}

	void Start(){
		trans = transform;
		ReArrange ();
	}

	public void Reset(){
		pipeWidth 					= pipeWidthMinMax.y;
		listPipeTrans [0].position 	= startPipePos;
		listSkyTrans[0].position 	= startSkyPos;
		listGroundTrans[0].position = startGroundPos;
		ReArrange ();
	}

	[ContextMenu("Do ReArrange")]
	void ReArrange()
	{
		//Pipe Re-arrange position x and position y
		Vector3 _pos;
		Transform _t, _t1, _t2;
		float _firstX = 0;
		for (int i = 0, iMax = listPipeTrans.Count; i < iMax; i++) {
			_t = listPipeTrans [i];
			_pos = _t.position;
			if (i == 0) {
				_firstX 		= _pos.x;
				startPipePos 	= _t.position;
			}else{
				_pos.x = _firstX + i * pipeWidth;
			}
			_pos.y = Random.Range (minMax.x, minMax.y);
			_t.position = _pos;

			if (_t.childCount >= 2) {
				_t1 = _t.GetChild (0);
				_t2 = _t.GetChild (1);
				_t1.position = _t2.position + pipeGap;
			}
		}

		//sky, background
		for (int i = 0, iMax = listSkyTrans.Count; i < iMax; i++) {
			_t = listSkyTrans [i];
			_pos = _t.position;
			if (i == 0) {
				_firstX 	= _pos.x;
				startSkyPos = _t.position;
			}else{
				_pos.x = _firstX + i * skyWidth;
			}
			_t.position = _pos;
		}

		for (int i = 0, iMax = listGroundTrans.Count; i < iMax; i++) {
			_t = listGroundTrans [i];
			_pos = _t.position;
			if (i == 0) {
				_firstX 		= _pos.x;
				startGroundPos 	= _t.position;
			}else{
				_pos.x = _firstX + i * groundWidth;
			}
			_t.position = _pos;

		}

		//Debug.Log (pipeWidth);
	}

	void OnTriggerEnter2D(Collider2D _col){
		//Debug.Log (_col.name);
		Vector3 _pos = _col.transform.position;
		if (_col.CompareTag ("bg_sky")) {
			_pos.x += skyWidth * listSkyTrans.Count;

		} else if (_col.CompareTag ("bg_ground")) {
			_pos.x += groundWidth * listGroundTrans.Count;
		} else if (_col.CompareTag ("pipe")) {
			pipeWidth = Mathf.Lerp(pipeWidthMinMax.y, pipeWidthMinMax.x, Mathf.Abs(trans.position.x) / pipeWidthInter);
			//Debug.Log (trans.position.x + " > " + pipeWidth);
			_pos.x += pipeWidth * listPipeTrans.Count;
			_pos.y = Random.Range (minMax.x, minMax.y);
		}

		_col.transform.position = _pos;
	}
}

