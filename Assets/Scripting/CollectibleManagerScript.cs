using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manage popCorns and spawning
public class CollectibleManagerScript : MonoBehaviour {

	public static CollectibleManagerScript instance = null;
	public CollectibleSpawnerScript[] cps;

	// Before anything starts
	void Awake()
	{
		// Singleton setup
		if (instance == null) instance = this;

		InitCollectibleManager();
	}
	
	// First instanciations
	void InitCollectibleManager()
	{
		for (int i=0; i<cps.Length; i++)
		{
			cps[i].InitCollectibleSet();
		}
	}
	
	// Starts spawn
	public void StartSpawn(int spawnerIndex)
	{	
		if (spawnerIndex < 0)
		{
			for (int i=0; i<cps.Length; i++)
			{
				cps[i].StartSpawn();
			}
		}
		else
		{
			cps[spawnerIndex].StartSpawn();
		}
	}

	void DisableAll()
	{
		for (int i=0; i<cps.Length; i++)
		{
			cps[i].DisableCollectibles();
			cps[i].StopSpawn();
		}
	}

	public void ResetScript()
	{
		// For each inst objects, Set active false
		DisableAll();
	}

}
