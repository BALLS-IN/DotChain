using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpatializationManagerScript : MonoBehaviour {

	public static SpatializationManagerScript instance = null;
	public int maxInst;
	public GameObject base_spatialized;

	private SpacializedItemScript[] spatializeds;

	void Awake ()
	{
		// Singleton setup
		if (instance == null) instance = this;

		InitSet();
	}

	public void InitSet()
	{
		// Instantiate points in an array 
		spatializeds = new SpacializedItemScript[maxInst];
		for (int i=0; i<maxInst; i++)
		{
			spatializeds[i] = GameObject.Instantiate(base_spatialized, transform.position + i*Vector3.right*2, Quaternion.identity).GetComponent<SpacializedItemScript>();
			spatializeds[i].transform.SetParent(transform);
			spatializeds[i].transform.localScale = base_spatialized.transform.localScale;
			spatializeds[i].gameObject.SetActive(false);
		}

		// Deactivate base_object
		base_spatialized.SetActive(false);
	}

	public void SpawnSpatialized (string withText, Transform on)
	{
		int i = 0;
		
		while(i<spatializeds.Length)
		{
			// If the target object is not active...
			if (!spatializeds[i].gameObject.activeSelf)
			{
				SpawnOn(spatializeds[i], withText, on);
				i = spatializeds.Length;
			}

			// Increment loop index anyway
			i++;
		}
	}

	void SpawnOn (SpacializedItemScript si, string t, Transform on)
	{
		// Find Relative position in canvas space
		Vector3 relPos = Camera.main.WorldToScreenPoint(on.position);
		si.gameObject.SetActive(true);

		// Send spawn to object
		si.SpawnAt(relPos, t);
	}
}
