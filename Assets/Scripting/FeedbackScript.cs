using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackScript : MonoBehaviour {

	public ParticleSystem[] particles;
	public ParticleSystem[] fixedParticles;
	public Animator[] anim;
	public TrailRenderer[] tr;
	public Renderer[] rdrr;

	void Awake ()
	{
		for (int i=0; i<particles.Length; i++)
		{
			particles[i].transform.SetParent(null);
		}
	}

	public void PlayParticleAt (int i, Vector3 pos)
	{
		particles[i].transform.position = pos;
		particles[i].Play();
	}

	public void PlayParticles (int i)
	{
		fixedParticles[i].Play();
	}

	public void StopParticles (int i)
	{
		fixedParticles[i].Stop();
	}

	public void StopAllParticles ()
	{
		for (int i=0; i<fixedParticles.Length; i++)
		{
			fixedParticles[i].Stop();
		}
	}

	// Returns the duration of the triggered animation
	public float PlayAnimation (int animIndex, string a)
	{
		anim[animIndex].Play(a,0,0);
		float rDuration = 0f;
		int i = 0;
		int animsLength = anim[animIndex].runtimeAnimatorController.animationClips.Length;

		while (i < animsLength)
		{
			if (anim[animIndex].runtimeAnimatorController.animationClips[i].name == a)
			{
				rDuration = anim[animIndex].runtimeAnimatorController.animationClips[i].length;
				i = animsLength;
			}
			i++;
		}
		return rDuration;
	}

	public void DisplayTRenderer (int i, bool b)
	{
		tr[i].enabled = b;
	}

}
