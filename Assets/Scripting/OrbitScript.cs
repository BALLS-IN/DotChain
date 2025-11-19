using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitScript : MonoBehaviour {

	public static OrbitScript instance = null;

	public float rotSpeed;
	public float switchSpeed;
	public float switchAngle;
	public SatelliteScript satellite;
	public float slowFactor;

	private Quaternion initialRot;
	private float currentAngle;
	private Quaternion currentRot;
	private Vector3 initialRelativePosition;
	private Vector3 targetRelativePosition;
	private bool switching;
	private float switchLerpIncrement;

	private bool slowed;
	private bool switched;
	// private bool speedReduced;
	private FeedbackScript fbs;

	public bool Switching
	{
		get{return switching;}
	}

	void Awake ()
	{	
		// Singleton setup
		if (instance == null) instance = this;

		initialRot = transform.rotation;
		fbs = GetComponent<FeedbackScript>();
	}

	public void Birth ()
	{
		fbs.DisplayTRenderer(0, true);
		Blob ();
	}

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update ()
	{
		// if (Input.GetKeyDown("s"))
		// {
		// 	SwitchSide();
		// }
		SimpleRotate();
		HandleSwitch();
	}

	public void UpdateInput (bool _slowed, bool _switch, bool _switched)
	{
		if (!switching)
		{
			if (_switch)
			{
				SwitchSide();
			}
		}
	}

	void SimpleRotate ()
	{
		currentAngle += rotSpeed * Time.deltaTime;
		currentAngle = GetModAngle(currentAngle);

		currentRot = Quaternion.AngleAxis(currentAngle, Vector3.up);

		transform.rotation = (currentRot * initialRot);
	}

	public void ReduceRotSpeed ()
	{
		rotSpeed *= slowFactor;
		// speedReduced = true;
	}

	public void RevertRotSpeed ()
	{
		
		rotSpeed /= slowFactor;
		// speedReduced = false;
	}

	public void SwitchSide()
	{
		GameManagerScript.instance.HideTuto();
		GameManagerScript.instance.FirstTouch();
		
		fbs.PlayParticleAt(0, satellite.transform.position);

		initialRelativePosition = satellite.transform.localPosition;
		targetRelativePosition = initialRelativePosition;
		
		// switchAngle *= -1;

		// Non Linear switch
		Vector3 relPos = -targetRelativePosition - transform.position;
		targetRelativePosition = transform.position + UtilityScript.GenerateRotatedPoint(relPos, switchAngle);

		// targetRelativePosition.z *= -1;

		switchLerpIncrement = 0;
		ReduceRotSpeed();

		switching = true;
		
	}

	void HandleSwitch()
	{
		if (switching)
		{
			switchLerpIncrement += Time.deltaTime / switchSpeed;
			if (switchLerpIncrement > 1)
			{
				FinishSwitch();
			}
			else
			{
				satellite.transform.localPosition = Vector3.Lerp(initialRelativePosition, targetRelativePosition, switchLerpIncrement);
			}
		}
	}

	void FinishSwitch ()
	{
		switching = false;
		satellite.transform.localPosition = targetRelativePosition;
		RevertRotSpeed ();
		ScoreScript.instance.ResetAmount();
	}

	public static float GetModAngle(float angle)
    {
		if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;

        return angle;
    }

	public void Blob ()
	{
		fbs.PlayAnimation(0, "pop");
	}

	public void UpdateTrail (int trailLevel)
	{
		fbs.StopAllParticles();
		// fbs.PlayParticles(trailLevel); ON FIRE TRAIL
	}

	public void TriggerDeath ()
	{
		fbs.DisplayTRenderer(0, false);
		fbs.PlayAnimation(0, "ANIM_Basic_scaleZero");
		fbs.PlayParticleAt(1, satellite.transform.position);
		fbs.StopAllParticles ();
	}
}
