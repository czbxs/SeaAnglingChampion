using UnityEngine;
using System.Collections;

public class MedusaScript : MonoBehaviour {
	public Animator AnimOctopus;
	[HideInInspector]
	public Transform octopus;
	public Transform chest;
	Transform chestStartParent;
	Vector3 chestStartLocalPosition;

	public bool bChest;
	public string chestReward;
	float nextActivationTime = 0;
	bool bMoving = false;
	float movingTime = 1;
	float movingSpeed = .1f;

	[HideInInspector]
	public Transform CopyPos = null; 
	[HideInInspector]
	public bool bCopyPos = false;

	Vector3 startPos ;
	Vector3 endPos;

	float StartPosX = -15;
	float EndPosX = 17;
	float Ymin = -3;
	float Ymax =  -1;
 

	// Use this for initialization
	void Start () {
		bChest = false;
		chestReward = "";
		nextActivationTime = 5;//Random.Range( 5,10);
		InvokeRepeating("DecreaseActivationTime",1,1);
		octopus  = AnimOctopus.transform;
		chestStartParent = chest.transform.parent;
		chestStartLocalPosition = chest.localPosition;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(!Gameplay.Instance.bPause && bMoving)
		{
			movingTime += Time.fixedDeltaTime* movingSpeed;
			if(movingTime<=1)
			{
				octopus.position = Vector3.Lerp(startPos,endPos,movingTime);
			}
			else if(!bCopyPos  )
			{
				if(!bChest)
				{
					ActivateOctopus();
					nextActivationTime =  Random.Range(3,7);// 10,20);
				}
			}

			if( bCopyPos ) chest.position = Vector3.Lerp(chest.position,CopyPos.position,Time.fixedDeltaTime*10);
		}
		else if(!Gameplay.Instance.bPause && bCopyPos && chestReward == "" )
		{
			octopus.position = Vector3.Lerp(octopus.position,CopyPos.position,Time.fixedDeltaTime*10);
		}
//		else
//		{
//			ActivateOctopus();
//			nextActivationTime = 3;//Random.Range( 5,10);
//		}
	}

	void DecreaseActivationTime()
	{
		if(!Gameplay.Instance.bPause && !bMoving && !bCopyPos)
		{
			if(!bChest)
				nextActivationTime --;

		}
		if(nextActivationTime<1)
		{
			if(!bChest)
			{
			 	ActivateOctopus();
				nextActivationTime =  Random.Range(3,7);// 10,20);
			}
		}
	}

	void ActivateOctopus()
	{
		CopyPos = null;
		bCopyPos = false;
		movingTime = 0;
		bMoving = true;
		startPos = new Vector3(StartPosX,Random.Range(Ymin,Ymax),100);
		endPos = new Vector3(EndPosX,startPos.y,100);

		int reward = Random.Range(0,6);
		if(reward<2) chestReward =  "time";
		else if(reward < 4) chestReward = "gold";
		else chestReward = "empty";

 

 		AnimOctopus.SetBool("bChest", true);

		chest.SetParent(chestStartParent);
		chest.localPosition = chestStartLocalPosition;
	}


	public void MedusaCaught()
	{
		movingSpeed = .3f;
		if(chestReward != "") 
		{
			chest.SetParent (octopus.parent);
			AnimOctopus.SetBool("bChest", false);
			bChest = true;
		}
//		else
//		{
//			bMoving = false;
//		}
		if(SoundManager.Instance !=null ) SoundManager.Instance.Play_Sound(SoundManager.Instance.ChestCaught);
	}



	 

	public void  MedusaCaughtEnd()
	{


		movingSpeed = .1f;
		octopus.position =  new Vector3(StartPosX, Ymin, 100);

		chest.SetParent(chestStartParent);
		chest.localPosition = chestStartLocalPosition;
		CopyPos = null;
		bCopyPos = false;

	}

	public void ChestCaptured()
	{
		bMoving = false;
		movingTime = 0;
		bChest = false;



	}
}
