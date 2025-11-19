using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerScript : MonoBehaviour {

	public bool isHead;
	public GameObject tailVisu;

	private FollowerScript target;
	private Vector3 currentPos;
	private Vector3 nextPos;
	private bool following; public bool Following { get{return following;}}
	private FeedbackScript fbs;
	private Collider c;
	// private bool isLast;
	private int followerPosition;

	void Awake ()
	{
		following = true;
		// isLast = false;
		fbs = GetComponent<FeedbackScript>();
		c = GetComponent<Collider>();
		if (!isHead)
		{
			c.enabled = false;
		}
	}

	public void TailApparition ()
	{
		if (!isHead)
		{
			tailVisu.SetActive(true);
			float apparitionTime = fbs.PlayAnimation(0, "ANIM_Basic_PopHard");
			// Invoke("EnableColliderDelayed", apparitionTime);
			EnableColliderDelayed();
		}
	}

	public void SetLast (bool _isLast)
	{
		// isLast = _isLast;
	}

	public void EnableColliderDelayed ()
	{
		c.enabled = true;
	}

	public void TailDisparition (bool animatedDis)
	{
		if (!isHead)
		{
			CancelInvoke();
			c.enabled = false;
			
			if (animatedDis) fbs.PlayAnimation(0, "ANIM_Basic_PopOut");
			else fbs.PlayAnimation(0, "ANIM_Basic_scaleZero");
		}
	}

	public void InitFollower (FollowerScript t, int i)
	{
		target = t;
		followerPosition = i;

	}

	public void UpdatePos (float lerpIncrement)
	{
		transform.position = Vector3.Lerp (currentPos, nextPos, lerpIncrement);
	}

	public void UpdateNextPos ()
	{
		transform.position = nextPos;
		currentPos = nextPos;
		nextPos = target.transform.position;
	}

	void OnTriggerEnter (Collider col)
	{
		if (!isHead && col.tag == "bomb" && following)
		{
			FollowerManagerScript.instance.RemoveLastFollower();
		}

		if (!isHead && (followerPosition+1) >= FollowerManagerScript.instance.minTailElementCountBonus && col.tag == "Player") // && isLast
		{
			// Debug.Log (c.name + "  " + c.tag);
			// Debug.Log (name);
			// EditorApplication.isPaused = true;
			// Debug.Log ("restart");
			FollowerManagerScript.instance.TriggerTailMegaBonus();
		}
	}
}
