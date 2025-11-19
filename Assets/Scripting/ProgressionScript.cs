using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionScript : MonoBehaviour {

	public static ProgressionScript instance = null;

	public CollectibleSpawnerScript pointSpawner;
	public CollectibleSpawnerScript bombSpawner;

	public DifficultyStage[] difficultyStages;
	
	private int currentStage;
	private bool maxStageLock;
	private float startTime;
	private bool progressionStarted;

	void Awake ()
	{
		// Singleton setup
		if (instance == null) instance = this;
	}

	public void InitGame ()
	{
		progressionStarted = false;
		currentStage = 0;
		SetupDifficulty();
		maxStageLock = false;
	}

	public void StartProgression ()
	{
		startTime = Time.time;
		progressionStarted = true;
	}

	void Update ()
	{
		if (progressionStarted)
		{
			CheckForDifficultyChange();
		}
	}

	public void CheckForDifficultyChange ()
	{
		if (!maxStageLock)
		{
			if (Time.time - startTime > difficultyStages[currentStage].scoreMax)
			{
				currentStage ++;
				if (currentStage == difficultyStages.Length)
				{
					maxStageLock = true;
					currentStage --;
				}
				SetupDifficulty();
			}
		}
		
	}

	private void SetupDifficulty ()
	{
		// Spawn Frequencies
		pointSpawner.ModFrequency(difficultyStages[currentStage].point_spawnFrequencyMul);
		bombSpawner.ModFrequency(difficultyStages[currentStage].bomb_spawnFrequencyMul);

		// Spawn Forces
		pointSpawner.ModSpawnForce(difficultyStages[currentStage].point_spawnForceMul);
		bombSpawner.ModSpawnForce(difficultyStages[currentStage].bomb_spawnForceMul);
	}
}
