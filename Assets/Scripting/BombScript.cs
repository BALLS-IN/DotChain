using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour {

	public float scaleMulPerPoint;

	private Vector3 initialScale;
	private Vector3 currentScale;
	private float currentScaleMul;

	private FeedbackScript fbs;
	// private CollectibleScript cs;

	void Awake ()
	{
		initialScale = transform.localScale;
		fbs = GetComponent<FeedbackScript>();
		// cs = GetComponent<CollectibleScript>();
	}

	void OnEnable ()
	{

	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider c)
	{
		if (c.tag == "point")
		{
			RaiseScale();
		}
	}

	void RaiseScale ()
	{
		currentScaleMul += scaleMulPerPoint;
		currentScale = initialScale * currentScaleMul;
		transform.localScale = currentScale;

		fbs.PlayAnimation(0, "pop");

	}

	void OnDisable ()
	{
		ResetScale ();
	}

	void ResetScale ()
	{
		currentScaleMul = 1;
		currentScale = initialScale;
		transform.localScale = currentScale;
	}

	
}
