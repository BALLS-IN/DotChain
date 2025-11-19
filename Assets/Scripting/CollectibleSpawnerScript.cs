using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manage popCorns and spawning
public class CollectibleSpawnerScript : MonoBehaviour {

	public CollectibleScript base_object;
	public int maxInst;
	public float baseSpawnFrequency;
	public int spawnSimultaneousAmount;
	public bool spawnImmediate;

	public bool circularSpawning;
	public Transform circleSpawning_center;
	public float circleSpawning_radius;
	public float circleSpawning_centerShiftAngle;

	private CollectibleScript[] collectibles;
	private float nextSpawnTime;
	private float spawnFrequency;

	private bool spawnable;
	public bool Spawnable
	{
		get {return spawnable;}
	}

	public void InitCollectibleSet()
	{
		// Instantiate points in an array 
		collectibles = new CollectibleScript[maxInst];
		for (int i=0; i<maxInst; i++)
		{
			collectibles[i] = GameObject.Instantiate(base_object, transform.position + i*Vector3.right*2, Quaternion.identity).GetComponent<CollectibleScript>();
			collectibles[i].DestroyCollectible();
		}

		// Deactivate base_object 
		base_object.DestroyCollectible();

		spawnFrequency = baseSpawnFrequency;
		if (spawnSimultaneousAmount == 0) spawnSimultaneousAmount = 1;
	}

	public void InitSpawner ()
	{
		spawnFrequency = baseSpawnFrequency;
	}

	void Update ()
	{
		if (spawnable)
		{
			HandleSpawning();
		}
	}

	void HandleSpawning ()
	{
		if (Time.time >= nextSpawnTime) SpawnCollectible();
	}

	public void StartSpawn()
	{	
		spawnable = true;
		
		if (spawnImmediate) SpawnCollectible();
		else nextSpawnTime = Time.time + spawnFrequency;

		// InvokeRepeating("SpawnCollectible", 1, spawnFrequency);
	}

	void SpawnCollectible ()
	{
		for (int j=0; j<spawnSimultaneousAmount; j++)
		{
			int i = 0;
			while(i<collectibles.Length)
			{
				// If the target collectible is not active...
				if (!collectibles[i].gameObject.activeSelf) //
				{
					PrepareSpawn(collectibles[i]);
					i = collectibles.Length;
				}

				// Increment loop index anyway
				i++;
			}
		}
		
		nextSpawnTime = Time.time + spawnFrequency;
	}

	void PrepareSpawn (CollectibleScript c)
	{
		Vector3 spawnPos = Vector3.zero;

		if (circularSpawning)
		{
			spawnPos = SpawnCircular(c);
		}

		// ..Spawn it
		SpawnAt(c, spawnPos);
	}

	Vector3 SpawnCircular(CollectibleScript c)
	{
		Vector3 rnPos = Random.insideUnitSphere;

		if (circleSpawning_centerShiftAngle != 0)
		{
			Vector3 relPos = GameManagerScript.instance.player.position - circleSpawning_center.position;
			rnPos = circleSpawning_center.position + UtilityScript.GenerateRotatedPoint (relPos, UtilityScript.RandomSign() * circleSpawning_centerShiftAngle);
		}

		rnPos.y = 0;
		rnPos = rnPos.normalized * circleSpawning_radius + circleSpawning_center.position;

		return rnPos;
	}

	// Spawns a collectible at a position
	void SpawnAt(CollectibleScript o, Vector3 pos)
	{
		o.transform.position = pos;
		o.gameObject.SetActive(true);
	}

	public void DisableCollectibles()
	{
		for (int i=0; i<collectibles.Length; i++)
		{
			collectibles[i].DestroyCollectible();
		}
	}

	public void StopSpawn()
	{
		// CancelInvoke("SpawnCollectible");
		spawnable = false;
	}

	public void ModFrequency (float freqMul)
	{
		spawnFrequency = baseSpawnFrequency * freqMul;
	}

	public void ModSpawnForce (float spawnForceMul)
	{
		for (int i=0; i<collectibles.Length; i++)
		{
			collectibles[i].ModSpawnForce(spawnForceMul);
		}
	}

}
