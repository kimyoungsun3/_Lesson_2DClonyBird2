using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UiScore : MonoBehaviour {
	public static UiScore ins;
	int score, high;
	public Text scoreText, hightText;
	public GameObject btnRestart;
	Player scpPlayer;

	void Awake(){
		ins = this;
	}

	void Start(){
		score 	= 0;
		high 	= PlayerPrefs.GetInt ("high", 0);
		btnRestart.SetActive(false);

		hightText.text = "High:" + high;
		scoreText.text = "Score:" + score;
	}

	public void SetScore(int _s){
		score += _s;
		if (score > high) {
			high = score;
			hightText.text = "High:" + high;
		}
		scoreText.text = "Score:" + score;
	}

	void OnApplicationQuit(){
		PlayerPrefs.SetInt ("high", high);
	}

	//----------------------------------
	public void VisibleRestart(Player _scpPlayer, bool _b = true){
		btnRestart.SetActive(_b);	
		scpPlayer = _scpPlayer;
	}

	public void InvokeRestart(){
		//Debug.Log (1);

		int _h = PlayerPrefs.GetInt ("high", 0);
		if (high > _h) {
			PlayerPrefs.SetInt ("high", high);
		}
		score = 0;
		hightText.text = "High:" + high;
		scoreText.text = "Score:" + score;
		//Debug.Log (score);

		scpPlayer.EnableControl ();
		//SceneManager.LoadScene ("Game");

		//map reset...
		scpPlayer.Reset();
		BGLooper.ins.Reset ();
	}
}
