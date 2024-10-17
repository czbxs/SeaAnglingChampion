using UnityEngine;
using System.Collections;

public class FishController : MonoBehaviour {

	public Transform TargetPostition;
	public Vector3 PositionOffset = Vector3.zero;
	public float offsetPositionSpeed =.1f;
	float t = 0;
	float tmp2= 0;
	float tPom = 0;
	float tPom2 = 0;
	public bool enablePositionOffset = true;
	bool bMoveToBucket = false;
	public bool bScaleWhenCaught = false;

	Vector3 caughtPosition;
	void Start()
	{
		if(enablePositionOffset) StartCoroutine("SetPostitionOffset", Random.Range(1f,2f));

		if(   transform.GetChild(0) != null &&   transform.GetChild(0).GetComponent<Animator>() !=null )// &&  prefInd != 5 && prefInd !=14  && prefInd !=15 )
		{
			transform.GetChild(0).GetComponent<Animator>().speed = Random.Range(0.9f,1.1f);
			//transform.GetChild(0).GetComponent<Animator>().Play("FishAnimation",-1,Random.Range(0,.7f));
		}
	}

	
	// Update is called once per frame
	void Update () {
		if(bMoveToBucket)
		{
			//ubacivanje u kofu
			tmp2+=Time.deltaTime*3;
			if(tmp2<1)
			{
				tPom2 = (1-tmp2)*tmp2*3f; 
				transform.position = Vector3.Lerp(caughtPosition, BucketPos , tmp2) + new Vector3(0,tPom2,0);
			}
			else
			{
				GameObject.Destroy(this.gameObject);
			}
		}
		else if(!Gameplay.Instance.bPause && TargetPostition!=null)
		{
			if(enablePositionOffset)
			{
				t+=Time.deltaTime*offsetPositionSpeed;
				if(t>=1  ) 
				{
					enablePositionOffset = false;
					StopCoroutine("SetPostitionOffset" );
					StartCoroutine("SetPostitionOffset", Random.Range(2f,4f));
					t=1;
				}
				tPom = (1-t)*t; 
				transform.position = Vector3.Lerp(transform.position, TargetPostition.position+ PositionOffset* tPom, Time.deltaTime);
			}
			 else		 
				 transform.position = Vector3.Lerp(transform.position, TargetPostition.position, Time.deltaTime * 3);
		}

	}

	IEnumerator SetPostitionOffset(float timeWait)
	{

		//float x = Random.Range(-2f,2f);
		yield return new WaitForSeconds(timeWait);
		//float y = Random.Range(-2f,2f);
		PositionOffset = Random.insideUnitCircle*3;
		while(PositionOffset.magnitude<2) 
		{
			PositionOffset = Random.insideUnitCircle*3;
		}
		//PositionOffset = new Vector3(x,y,0);
		enablePositionOffset = true;
		t= 0;
		//Debug.Log("A "+PositionOffset);
	}

	public void FishCaught()
	{
//		Debug.Log(transform.name);
		transform.GetChild(0).GetComponent<Animator>().SetBool("bCaught",true);
		AllFishesController.Instance. InstantiateFishPrefab_Outside();
	}

	public void MoveToBucket(Transform Bucket)
	{
		caughtPosition = transform.position;
		transform.parent = Bucket;

		BucketPos = transform.parent.position + new Vector3(0,.8f,0);
		bMoveToBucket = true;
		t = 0;
		tmp2 = 0;
		tPom2 = 0;
		this.enabled = true;
	}

	Vector3 BucketPos;
}
