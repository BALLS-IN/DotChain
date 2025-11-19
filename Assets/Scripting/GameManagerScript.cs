using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour {
	
	public static GameManagerScript instance = null;
	public Transform player;
	public Transform center;
	public GameObject uiGameOver;
	public GameObject uiTuto;
	public FeedbackScript camFbs;
	public FeedbackScript gameFbs;

	private bool playing; public bool Playing { get{return playing;} }

	private string sceneName;
	private float slowDuration;
	private bool isSlowed;
	private bool beforeTouch;

	// Before anything starts
	void Awake()
	{
		Application.targetFrameRate = 60;

		// Singleton setup
		if (instance == null) instance = this;

		beforeTouch = true;
	}

	void Start ()
	{
		StartPlaying();
	}

	public void StartPlaying()
	{
		ScoreScript.instance.ResetScore();
		ScoreScript.instance.DisplayScore(false);
		CollectibleManagerScript.instance.StartSpawn(0);
		OrbitScript.instance.Birth();
		ProgressionScript.instance.InitGame();

		uiGameOver.SetActive(false);
		Time.timeScale = 1.0f;
		
		playing = true;
	}

	public void FirstTouch ()
	{
		if (beforeTouch)
		{
			gameFbs.PlayAnimation(1, "ANIM_Ui_DisableTitle");
			beforeTouch = false;
			ScoreScript.instance.DisplayScore(true);
		}
		
	}

	// Resets the game
	public void ResetGame ()
	{
		gameFbs.PlayAnimation(1, "ANIM_Ui_FadeIn");
		StartPlaying();
	}

	// Lose the game, activating a previously hidden canvas to retry
	public void Lose ()
	{
		beforeTouch = true;
		playing = false;

		OrbitScript.instance.TriggerDeath();
		CollectibleManagerScript.instance.ResetScript();
		FollowerManagerScript.instance.ResetFollowers();
		ScoreScript.instance.SetHighScore();
		ScoreScript.instance.GameOver();
		// Slow mo
		// SlowMo(0.5f);
		Time.timeScale = 0.04f;
		
		// Appear gameover
		StartCoroutine("C_GameToGameOver");
	}

	// Delay before displaying GameOver ui
	public IEnumerator C_GameToGameOver ()
	{
		yield return new WaitForSecondsRealtime(0.7f);
		uiGameOver.SetActive(true);
		gameFbs.PlayAnimation(0, "ANIM_Ui_FadeInGameOver");
	}

	public void HideTuto ()
	{
		if (uiTuto.activeSelf)
		uiTuto.SetActive(false);
	}

	// Triggers a slow mo and call timescale reset function
	public void SlowMo(float _slowDuration)
	{
		StopAllCoroutines();

		Time.timeScale = 0.02f;
		StartCoroutine("C_SlowToNormal", _slowDuration);
	}

	// Delay before reset timescale
	public IEnumerator C_SlowToNormal (float _slowDuration)
	{
		yield return new WaitForSecondsRealtime(_slowDuration);
		Time.timeScale = 1f;
	}

	// Shake cam
	public void ShakeCam ()
	{
		camFbs.PlayAnimation(0, "ANIM_Camera_Shake");
	}
}
