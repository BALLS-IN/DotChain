using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerManagerScript : MonoBehaviour {

	public static FollowerManagerScript instance = null;
	public int minTailElementCountBonus;

	public FollowerScript head;
	public GameObject base_object;
	public int maxInst;
	public float base_delay;
	public float base_distance;

	private FollowerScript[] followers;
	private float followLerpIncrement;
	private bool following;
	private int currentFollowerCount;

	void Awake ()
	{
		// Singleton setup
		if (instance == null) instance = this;

		InitFollowers();
	}

	void InitFollowers ()
	{
		followers = new FollowerScript[maxInst];

		followers[0] = GameObject.Instantiate(base_object, Vector3.one * 1000 + Vector3.right*2, Quaternion.identity).GetComponent<FollowerScript>();
		followers[0].InitFollower(head, 0);
		followers[0].name = "follower_0";
		followers[0].TailDisparition(false);

		for (var i=1; i<maxInst; i++)
		{
			followers[i] = GameObject.Instantiate(base_object, Vector3.one * 1000 + i*Vector3.right*2, Quaternion.identity).GetComponent<FollowerScript>();
			followers[i].InitFollower(followers[i-1], i);
			followers[i].name = "follower_" + i;
			followers[i].TailDisparition(false);
		}

		base_object.SetActive(false);
		StartFollowing();
	}

	// Use this for initialization
	void StartFollowing () {
		
		following = true;
	}
	
	// Update is called once per frame
	void LateUpdate () {

		HandleFollowing ();
	}

	void HandleFollowing ()
	{
		if (following)
		{
			followLerpIncrement += Time.deltaTime * base_distance / base_delay;

			AllFollow ();
			
			if (followLerpIncrement >= 1)
			{
				AllUpdateNextPos();
				followLerpIncrement = 0;
			}
		}
	}

	void AllFollow ()
	{
		for (int i=0; i<followers.Length; i++)
		{
			followers[i].UpdatePos(followLerpIncrement);
		}
	}

	void AllUpdateNextPos ()
	{
		for (int i=0; i<followers.Length; i++)
		{
			followers[i].UpdateNextPos();
		}
	}

	public void AddFollowerToHead ()
	{
		if (currentFollowerCount < followers.Length)
		{
			if (currentFollowerCount > 0) followers[currentFollowerCount-1].SetLast(false);
			followers[currentFollowerCount].TailApparition();
			followers[currentFollowerCount].SetLast(true);
			currentFollowerCount ++;
		}
	}

	public void RemoveFollower ()
	{
		if (currentFollowerCount > 0)
		{
			currentFollowerCount--;
		}
	}

	public void RemoveLastFollower ()
	{
		if (currentFollowerCount > 0)
		{
			followers[currentFollowerCount-1].TailDisparition(true);
			followers[currentFollowerCount-1].SetLast(false);

			if (currentFollowerCount > 1) followers[currentFollowerCount-2].SetLast(true);
			currentFollowerCount--;
		}	
		
	}

	public void ResetFollowers ()
	{
		for (var i=0; i<currentFollowerCount; i++)
		{
			followers[i].TailDisparition(true);
			followers[i].SetLast(false);
		}

		currentFollowerCount = 0;
	}

	public void TriggerTailMegaBonus ()
	{
		if (currentFollowerCount >= minTailElementCountBonus)
		{
			GetPointsFromTail();
			ResetFollowers();

			GameManagerScript.instance.SlowMo(1.0f);
			// Invoke("GetPointFromTail", 0.0001f);
		}
		
	}

	void GetPointsFromTail()
	{
		for (int i=0; i<currentFollowerCount; i++)
		{
			ScoreScript.instance.AddScoreSimple(1);
			SpatializationManagerScript.instance.SpawnSpatialized("+1", followers[i].transform);
		}
	}
}
