using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleScript : MonoBehaviour {

	public float spawnForce;
	public float colActivationDelay;
	public float attractionDuration;
	public float attractionPower;
	public float attractionPreDelay;
	public float minDistanceCollect;	
	public Transform target;
	public float maxDistanceToExplode;
	public float bombDecalAngle;
	public Color bombPreSpawnColor;
	public Color bombSpawnedColor;
	
	[HideInInspector]
	public Rigidbody rb;

	private bool isCollectibable;
	private Collider[] arenaCol;
	private bool attracting;
	private CollectibleScript cs;
	private Vector3 dir;
	private float base_attractionPower;
	private FeedbackScript fbs;
	private Vector3 spawnVector;
	private float attractionIncrement;
	private float currentSpawnForce;

	void Awake()
	{
		
		rb = GetComponent<Rigidbody>();
		arenaCol = GetComponentsInChildren<Collider>();
		base_attractionPower = attractionPower;
		fbs = GetComponent<FeedbackScript>();
	}

	void OnEnable ()
	{
		PrepareCollectible ();
	}

	void PrepareCollectible ()
	{
		// fbs.PlayAnimation(0, "ANIM_Basic_scaleZero");
		CancelInvoke();

		// Appear within short time frame
		float launchDelay = Random.Range(0, 0.6f);
		Invoke("PreApparition", launchDelay);
	}

	void PreApparition ()
	{
		// Play prespawn animation;
		float animDur = fbs.PlayAnimation(0, "scaleUp");
		if (tag=="bomb")
		{
			fbs.rdrr[0].material.SetColor("_Color", bombPreSpawnColor);
			fbs.PlayParticleAt(3, transform.position);
		}
		animDur += colActivationDelay;
	
		// Invoke launch after animation
		Invoke("LaunchCollectible", animDur);
	}

	public void LaunchCollectible ()
	{
		// The collectible is now collectibable
		isCollectibable = true;
		spawnVector = (GameManagerScript.instance.center.position - transform.position).normalized;

		// The collectible is now launched
		if (tag == "bomb")
		{
			spawnVector = Quaternion.AngleAxis(bombDecalAngle, Vector3.up) * spawnVector;
			fbs.rdrr[0].material.SetColor("_Color", bombSpawnedColor);
		}
		
		rb.AddForce(spawnVector * currentSpawnForce);

		UtilityScript.SetColliderActive(arenaCol, true);
	}

	void LateUpdate ()
	{
		HandleAttraction();
	}

	void TriggerAttraction (Transform _target)
	{
		target = _target;
		attractionIncrement = 0;
		fbs.PlayParticleAt(0, transform.position);
		UtilityScript.SetColliderActive(arenaCol, false);
		rb.linearVelocity = Vector3.zero;
		rb.isKinematic = true;
		Invoke("TriggerAttractionDelayed", attractionPreDelay);
	}

	void TriggerAttractionDelayed ()
	{
		attracting = true;
	}

	void HandleAttraction ()
	{
		if (attracting)
		{
			// Attraction based on fixed Lerp
			attractionIncrement += Time.deltaTime / attractionDuration;
			transform.position = Vector3.Lerp(transform.position, target.position, attractionIncrement);
			if (attractionIncrement > 1) FinishAttraction();
		}
	}

	void FinishAttraction()
	{
		fbs.PlayParticleAt(1, GameManagerScript.instance.player.position);
		DestroyCollectible();
	}

	public void DestroyCollectible()
	{
		UtilityScript.SetColliderActive(arenaCol, false);
		gameObject.SetActive(false);
	}

	void TriggerDeath ()
	{
		fbs.PlayParticleAt(0, transform.position);
		fbs.PlayParticleAt(1, transform.position);
		fbs.PlayParticleAt(2, transform.position);
		GameManagerScript.instance.Lose();
		DestroyCollectible();
	}

	void OnTriggerEnter(Collider c)
	{
		if (isCollectibable && GameManagerScript.instance.Playing)
		{
			if (c.tag == "Player")
			{
				if (tag == "point")
				{
					TriggerAttraction(c.gameObject.transform);
					CollectPoint ();
					
				}
				else if (tag == "bomb")
				{
					TriggerDeath();
					// Debug.Log (Vector3.Distance(c.gameObject.transform.position, gameObject.transform.position));
					// if (Vector3.Distance(c.gameObject.transform.position, gameObject.transform.position) < maxDistanceToExplode)
					// {
					// 	TriggerDeath();
					// }
					// else
					// {
					// 	TriggerAlmost();
					// }
				}
			}

			if (c.tag == "snakeTail")
			{
				if (tag == "bomb")
				{
					BombExplode ();
				}
				// else if (tag == "point")
				// {
				// 	TriggerAttraction (c.gameObject.transform);
				// 	CollectPoint();
				// }
			}

			if (tag == "point" && c.tag == "bomb")
			{
				TriggerAttraction(c.gameObject.transform);
			}
		}
	}

	void BombExplode()
	{
		fbs.PlayParticleAt(0, transform.position);
		fbs.PlayParticleAt(1, transform.position);
		// GameManagerScript.instance.SlowMo(0.3f);
		DestroyCollectible ();
	}

	void TriggerAlmost()
	{
		Debug.Log ("almost");
	}

	void CollectPoint()
	{
		ScoreScript.instance.AddScoreCombo(1);
		SpatializationManagerScript.instance.SpawnSpatialized("+" + ScoreScript.instance.GetCurrentAmount(), transform);
		FollowerManagerScript.instance.AddFollowerToHead();
		OrbitScript.instance.Blob();

		// Prevent bomb spawn before collecting one point
		// TODO must be independant 
		if (!CollectibleManagerScript.instance.cps[1].Spawnable)
		{
			// Start bomb spawn
			CollectibleManagerScript.instance.StartSpawn(1);

			// Trigger progression start
			ProgressionScript.instance.StartProgression();
		}
	}

	void OnCollisionEnter (Collision c)
	{
		if (isCollectibable)
		{
			if (c.gameObject.tag == "arena")
			{
				UtilityScript.SetColliderActive(arenaCol, false);
				float animDur = fbs.PlayAnimation(0, "ANIM_Basic_PopOut");
				Invoke("DestroyCollectible", animDur);
				// DestroyCollectible();
			}
		}
	}

	void OnDisable ()
	{
		attractionPower = base_attractionPower;
		rb.linearVelocity = Vector3.zero;
		rb.isKinematic = false;
		isCollectibable = false;
		attracting = false;
		CancelInvoke();
	}

	public void ModSpawnForce (float spawnForceMul)
	{
		currentSpawnForce = spawnForce * spawnForceMul;
	}

	
}
