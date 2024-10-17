using UnityEngine;
using System.Collections;

public class SharkScript : MonoBehaviour {

	Vector3 startPos ;
	Vector3 endPos;
	
	float StartPosX = -14;
	float EndPosX = 14;
	float Ymin = -3;
	float Ymax =  -1;

	public static bool bShark = false;
	public static bool bSharkActive = false;
	float movingTime = 1;
	float movingSpeed = .1f;

	float timeTillActivation;
	public Animator animShark;

	// Use this for initialization
	void Start () {
		bShark = false;
		 timeTillActivation = Random.Range(0,45);
		if(timeTillActivation <25)
		{
			GameObject.Destroy(this.gameObject);
		}
	}


	void Update() 
	{
		if(!Gameplay.Instance.bPause && bShark)
		{
			movingTime += Time.deltaTime* movingSpeed;
			if(movingTime<=1)
			{
				transform.position = Vector3.Lerp(startPos, endPos, movingTime);
				if(movingTime<0.2f) bSharkActive = false;
				else if(movingTime<0.7f)    bSharkActive = true;
				else  bSharkActive = false ;
			}
		  	else 
			{
				bShark = false;
				GameObject.Destroy(this.gameObject);
			}
		}
		else if( !Gameplay.Instance.bPause && !bShark)
        {
			timeTillActivation-= Time.deltaTime;
			if(timeTillActivation<=0)  ActivateShark();
		}
	}

	public void OpenJaws()
	{
			animShark.SetTrigger("tOpenJaws");
			movingSpeed  = 0.05f;
	}

	public void ActivateShark()
	{
		animShark.ResetTrigger("tOpenJaws");
		movingTime = 0;
		bShark = true;

		float direction = Mathf.Sign( Random.Range(-2,2));
		transform.localScale = new Vector3(-direction,1,1);

		startPos = new Vector3(direction*StartPosX,Random.Range(Ymin,Ymax),100);
		endPos = new Vector3(direction*EndPosX,startPos.y,100);

		transform.position = startPos;
	}

}
