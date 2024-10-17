using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameTimer : MonoBehaviour {

	public Text txtTimeLeft;
	Animator animTimeLeft;
 
	void Start () {

		InvokeRepeating("TimerTick",0f,1f);
		animTimeLeft = txtTimeLeft.transform.GetComponent<Animator>();
	}
	
 
	void TimerTick()
	{
		if(!Gameplay.Instance.bPause)
		{
			 GameData.TimeLeft--;
			if(GameData.TimeLeft <=0)
			{
				GameData.TimeLeft = 0;
				 
				transform.SendMessage("OutOfTime");
				GameData.bOutOfTime = true;
			}
			//if(GameData.TimeLeft >10) txtTimeLeft.color = Color.white;
			//else  txtTimeLeft.color = new Color(1,0.2f,0.2f);
		}
		if(GameData.TimeLeft>0)
			txtTimeLeft.text = Mathf.FloorToInt(GameData.TimeLeft/60) +":"+ (GameData.TimeLeft%60).ToString().PadLeft(2,'0');
		else txtTimeLeft.text ="0:00";

		animTimeLeft.SetBool("bTimerBlink", GameData.TimeLeft <=10 && GameData.TimeLeft >0);

 

		if(GameData.TimeLeft <=10 && GameData.TimeLeft >0)
		{
	 		if(Gameplay.Instance.bPause && MenuManager.activeMenu == "PopUpPause")
			{
 				if(SoundManager.Instance !=null ) SoundManager.Instance.Stop_Sound(SoundManager.Instance.TimeCountdown);
			}
 			else
			{
				if(SoundManager.Instance !=null ) SoundManager.Instance.Play_Sound(SoundManager.Instance.TimeCountdown);
			}
		}
		else
		{
			if(SoundManager.Instance !=null ) SoundManager.Instance.Stop_Sound(SoundManager.Instance.TimeCountdown);
		}
	}

	/*
	public void StopTimer()
	{
		CancelInvoke("TimerTick");
		txtTimeLeft.text = Mathf.FloorToInt(GameData.TimeLeft/60) +":"+ (GameData.TimeLeft%60).ToString().PadLeft(2,'0');
		animTimeLeft.SetBool("bTimerBlink",false);
		if(SoundManager.Instance !=null )  SoundManager.Instance.Stop_Sound(SoundManager.Instance.TimeCountdown);

	}
	*/
}
