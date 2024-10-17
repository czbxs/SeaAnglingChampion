using UnityEngine;
using System.Collections;

public class FishHolder : MonoBehaviour {

	Vector3 TargetPosition = Vector3.zero;
	Vector3 TargetPositionOld = Vector3.zero;
	float speedMax = .8f;
	float speedMin = .3f;
	float speed =.4f;
	float timeMove = 0;
	FishController[] fishControllerArray;


	//  bool bStop = false;

	public bool WaitAnimationRotateFish = true;
	float animRotateDuration = 1;
	IEnumerator Start () {
		TargetPosition = transform.position;
		TargetPositionOld = TargetPosition;
		yield return new WaitForSeconds(0.1f);
		SetPostitionOffset();
	}
	
	 
	void Update () {
		//if(bStop) return;

		if(!Gameplay.Instance.bPause)
		{
			if(timeMove<1    )
			{
				timeMove +=Time.deltaTime* speed;
				transform.position = Vector3.Lerp(TargetPositionOld, TargetPosition , timeMove);
				 
			}
			else
			{
				 SetPostitionOffset();
				 
			}
		}
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


		if(WaitAnimationRotateFish) timeMove = -(animRotateDuration/speed);
		timeMove= 0;

		fishControllerArray = transform.GetComponentsInChildren<FishController>();
		foreach(FishController fc in fishControllerArray)
		{
			//posalji zahtev za animaciju okretanja
			
			if(TargetPosition.x>TargetPositionOld.x)  fc.transform.localScale = new Vector3(-1,1,1);
			else fc.transform.localScale =   Vector3.one;
		}

 
	//	Debug.Log("NEW POS  "+TargetPosition + "  speed" + speed);
	 
		//bStop = false;
  

	}


}
