using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularObstacleScript : MonoBehaviour {

	public float rotSpeed;

	private Quaternion initialRot;
	private float currentAngle;
	private Quaternion currentRot;

	// private bool speedReduced;
	private FeedbackScript fbs;

	void Awake ()
	{
		initialRot = transform.rotation;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		SimpleRotate();
	}

	void SimpleRotate ()
	{
		currentAngle += rotSpeed * Time.deltaTime;
		currentAngle = OrbitScript.GetModAngle(currentAngle);

		currentRot = Quaternion.AngleAxis(currentAngle, Vector3.up);

		transform.rotation = (currentRot * initialRot);
	}
}
