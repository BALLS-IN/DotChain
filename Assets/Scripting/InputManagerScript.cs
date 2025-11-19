using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Overall player control
public class InputManagerScript : MonoBehaviour {

	public static InputManagerScript instance;

	private Touch myTouch;
	private bool touched;
	private bool touchedUp;
	private bool touching;
	private Vector2 currentPosition;
	private Vector2 aimingDirection;

	private bool controllable;

	public bool TouchedUp
	{
		get{return touchedUp;}
	}
	public bool Touched
	{
		get{return touched;}
	}
	public bool Controllable
	{
		get{return controllable;}
		set{controllable = value;}
	}

	// Before anything starts
	void Awake()
	{
		// Singleton setup
		if (instance == null) instance = this;
		// else if (instance != this) Destroy(gameObject);
		// DontDestroyOnLoad(gameObject);

		InitInputs();
	}

	// Init controlled object
	void InitInputs ()
	{

	}
	
	// Update is called once per frame
	void Update () {

		if (GameManagerScript.instance.Playing)
		{
			HandleInputs ();

			OrbitScript.instance.UpdateInput (touching, touched, touchedUp);
		}
	}

	public void StartControl ()
	{
		Controllable = true;
	}


	// Check and update inputs states
	void HandleInputs ()
	{
		// Touch Inputs
		// if (Input.touches.Length > 0)
		// {
		// 	myTouch = Input.GetTouch(0);

		// 	touching = true;
		// 	touched = (myTouch.phase == TouchPhase.Began);
		// 	touchedUp = (myTouch.phase == TouchPhase.Ended);
		// }
		// else
		// {
		// 	touching = false;
		// }

		// Mouse Inputs
		touching = Input.GetMouseButton(0);
		touched = Input.GetMouseButtonDown(0);
		touchedUp = Input.GetMouseButtonUp(0);
	}
}
