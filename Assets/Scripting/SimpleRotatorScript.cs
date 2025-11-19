using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotatorScript : MonoBehaviour {

	public float rotationSpeed;
	public Vector3 rotationAxis;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
	}
}
