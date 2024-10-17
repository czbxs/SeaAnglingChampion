using UnityEngine;
using System.Collections;

public class PopUpInkScript : MonoBehaviour {
	public   Animator octopusAnim;
	public   Animator penguinAnim;

	public void AnimEvent_OctopusStartAnim()
	{
		penguinAnim.SetTrigger("tIdle");
		octopusAnim.SetTrigger("tCaught");
	}

	public void AnimEvent_OctopusStartInk()
	{
		penguinAnim.SetTrigger("tIdle");
		octopusAnim.SetTrigger("tInk");
		if(SoundManager.Instance !=null ) SoundManager.Instance.Play_Sound(SoundManager.Instance.OctopusInk);
	}

	public void AnimEvent_PenguinInk()
	{
		penguinAnim.SetTrigger("tInk");
	}

	public void AnimEvent_OctopusStopInk()
	{
		
		octopusAnim.SetTrigger("tEscape");

	}
}
