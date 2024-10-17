using UnityEngine;
using System.Collections;

public class OctopusScript : MonoBehaviour {

	[HideInInspector]
	public Animator AnimOctopus;

 

//	float nextActivationTime = 0;


	[HideInInspector]
	public Transform CopyPos = null; 
	[HideInInspector]
	public bool bCopyPos = false;
	bool bMoving = true;


	Vector3 TargetPosition = Vector3.zero;
	Vector3 TargetPositionOld = Vector3.zero;
	float speedMax = .8f;
	float speedMin = .3f;
	float speed =.4f;
	float timeMove = 0;


	IEnumerator Start () {
		AnimOctopus = transform.GetComponent<Animator>();

		TargetPosition = transform.position;
		TargetPositionOld = TargetPosition;
		yield return new WaitForSeconds(0.1f);
		SetPostitionOffset();
	 
	
	
	}

	void  SetPostitionOffset( )
	{
		//bStop = true;
		float x = Random.Range(AllFishesController.Instance.LimitTopLeft.position.x,AllFishesController.Instance.LimitBottomRight.position.x);
		float y = Random.Range(AllFishesController.Instance.LimitTopLeft.position.y,AllFishesController.Instance.LimitBottomRight.position.y);
		TargetPosition = new Vector3(x,y,0);
		TargetPositionOld = new Vector3(transform.position.x, transform.position.y, 0);
		float dist = Vector2.Distance((Vector2) TargetPosition, (Vector2) TargetPositionOld);
		//while(dist<4) dist = Vector2.Distance((Vector2) TargetPosition, (Vector2) TargetPositionOld);
		speed = Random.Range(speedMin,speedMax)/dist;
		timeMove= 0;
	}


	 
	// Update is called once per frame
	void FixedUpdate () {
		if(!Gameplay.Instance.bPause   && bMoving)
		{
			if(timeMove<1    )
			{
				timeMove +=Time.fixedDeltaTime* speed;
				transform.position = Vector3.Lerp(TargetPositionOld, TargetPosition , timeMove);

			}
			else
			{
				SetPostitionOffset();

			}
		}
		else if(!Gameplay.Instance.bPause && bCopyPos ) //upecan
		{
			transform.position = Vector3.Lerp(transform.position,CopyPos.position,Time.fixedDeltaTime*10);
		}

	}

	void DecreaseActivationTime()
	{
//		if(!Gameplay.Instance.bPause && !bMoving && !bCopyPos)
//		{
//
//			nextActivationTime --;
//
//		}
//		if(nextActivationTime<1)
//		{
//			ActivateOctopus();
//			nextActivationTime = 3;//Random.Range( 5,10);
//		}
	}

	void ActivateOctopus1()
	{
//		CopyPos = null;
//		bCopyPos = false;
//		movingTime = 0;
//		bMoving = true;
//		startPos = new Vector3(StartPosX,Random.Range(Ymin,Ymax),100);
//		endPos = new Vector3(EndPosX,startPos.y,100);
//
//		int reward = Random.Range(0,6);
//		if(reward<2) chestReward =  "time";
//		else if(reward < 4) chestReward = "gold";
//		else chestReward = "empty";
//
//
//
//		AnimOctopus.SetBool("bChest", chestReward!="");
//
//		chest.SetParent(chestStartParent);
//		chest.localPosition = chestStartLocalPosition;
	}


	public void OctopusCaught()
	{
		AnimOctopus.SetTrigger("tCaught");
		bMoving = false;
	 
	}





	public void OctopusCaughtEnd()
	{
		CopyPos = null;
		bCopyPos = false;
	 
		bMoving = false;
		//GameObject.Destroy(this.gameObject);
		transform.position = new Vector3(-16, Random.Range(AllFishesController.Instance.LimitTopLeft.position.y,AllFishesController.Instance.LimitBottomRight.position.y),0);
		StartCoroutine("ActivateOctopus");

	}

	IEnumerator ActivateOctopus ()  
	{
		AnimOctopus.SetTrigger("tSwim");

		yield return new WaitForSeconds(5f);
		SetPostitionOffset( );

		CopyPos = null;
		bCopyPos = false;
		bMoving = true;
	}
}
