using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour {

	public static ScoreScript instance = null;

	public Text uiScore;
	public Text uiScoreGameOver;
	public Text uiMaxScore;
	public SpacializedItemScript uiCombo;
	public int maxComboAmount;
	
	private int currentScore;
	private int currentAmount;
	private int currentCombo;
	private bool comboable;
	private int maxScore;

	public int GetCurrentAmount ()
	{
		return currentAmount;
	}

	void Awake ()
	{
		// Singleton setup
		if (instance == null) instance = this;

		maxScore = GetHighScore();
	}

	// Adds score
	public void AddScoreCombo (int _amount)
	{
		currentAmount += _amount;
		
		if (currentAmount > 1)
		{
			GameManagerScript.instance.ShakeCam();
			if (comboable)
			{
				IncreaseCombo ();
			}
		}

		currentScore += currentAmount;
		UpdateUIScore();
	}

	public void AddScoreSimple (int _amount)
	{
		currentScore += 1;
		UpdateUIScore();
	}

	void IncreaseCombo ()
	{
		currentCombo = Mathf.Clamp(currentCombo + 1, 0, maxComboAmount);
		if (currentCombo == maxComboAmount) OrbitScript.instance.UpdateTrail (1);
		comboable = false;
	}

	void ResetCombo ()
	{
		currentCombo = 0;
		OrbitScript.instance.UpdateTrail (0);
	}

	public void GameOver ()
	{
		DisplayScore(false);
		uiScoreGameOver.text = currentScore.ToString();
	}

	public void DisplayScore (bool _isActive)
	{
		uiScore.gameObject.SetActive(_isActive);
	}

	public void ResetScore ()
	{
		comboable = true;
		ResetCombo();
		currentScore = 0;
		DisplayScore(true);
		UpdateUIScore();
	}

	public void ResetAmount ()
	{
		if (currentAmount < 2)
		{
			ResetCombo();
		}

		currentAmount = 0;
		comboable = true;
	}

	// Update score ui
	private void UpdateUIScore()
	{
		uiScore.text = currentScore.ToString();
		uiMaxScore.text = maxScore.ToString();
	}
	
	public int GetHighScore ()
	{
		if (PlayerPrefs.HasKey("gravityBall_maxScore")) return PlayerPrefs.GetInt("gravityBall_maxScore");
		
		return 0;
	}

	public void SetHighScore ()
	{
		if (currentScore > maxScore)
		{
			maxScore = currentScore;
			PlayerPrefs.SetInt("gravityBall_maxScore", currentScore);
			UpdateUIScore ();
		}
	}
}
