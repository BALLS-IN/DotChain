using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpacializedItemScript : MonoBehaviour {

	public Text spatializedText;
	public Vector3 offset;

	private FeedbackScript fbs;

	void Awake ()
	{
		fbs = GetComponent<FeedbackScript>();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SpawnAt(Vector3 relPos, string t)
	{
		spatializedText.text = t;
		transform.position = relPos + offset;

		float dur = fbs.PlayAnimation(0, "ANIM_Ui_Popup");
		Invoke ("DestroySpatialized", dur);
	}

	public void ChangeTextAndPop (string t)
	{
		spatializedText.text = t;
		fbs.PlayAnimation(0, "ANIM_Ui_Pop");
	}

	void DestroySpatialized ()
	{
		gameObject.SetActive(false);
	}
}
