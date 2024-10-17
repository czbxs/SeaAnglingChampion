using UnityEngine;
using System.Collections;

public class AllFishesController : MonoBehaviour {

	public static AllFishesController Instance;
	public Transform LimitTopLeft;
	public Transform LimitBottomRight;

	public Transform InstPosLeft;
	public Transform InstPosRight;

	public GameObject[] prefabs;
	public int prefGroupIndex; //index prefaba koji kreira jato tuna
	bool bKreiranoJato = false;

	
	// Use this for initialization
	void Start () {
		AllFishesController.Instance = this;

	//	Debug.Log ( "UnlockedFish: "+GameData.UnlockedFish + "  " +  " ,Level" + GameData.Level);

		InstantiateFishPrefab_FishingArea();
	}


	void InstantiateFishPrefab_FishingArea()
	{
		for(int i = 0;i<GameData.StartFishCount;i++)
		{
			int prefInd = Random.Range(0,GameData.UnlockedFish);
			if(i == 0 )  prefInd = GameData.UnlockedFish;


			if(prefGroupIndex == prefInd &&  !bKreiranoJato) bKreiranoJato = true;
			else 	if(prefGroupIndex == prefInd && bKreiranoJato) 
			{
				while (prefGroupIndex == prefInd)
				{
					prefInd = Random.Range(0,GameData.UnlockedFish);
				}
			}

			Vector3 position = new Vector3(Random.Range(LimitTopLeft.position.x, LimitBottomRight.position.x), Random.Range(LimitTopLeft.position.y, LimitBottomRight.position.y), 0);
		 
			GameObject go = (GameObject) GameObject.Instantiate( prefabs[prefInd], position, Quaternion.identity );
			go.transform.parent = transform;
			go.transform.localScale = prefabs[prefInd].transform.localScale;
//			Debug.Log ( go.name);

			if(i == 1 )
			{
				Tutorial.copyPositionTransform = go.transform.GetComponentInChildren<FishController>().transform;
			}

		}
	}

	public void InstantiateFishPrefab_Outside()
	{
		int prefInd = Random.Range(0,GameData.UnlockedFish);
		float x=  InstPosLeft.position.x;
		if(Random.Range(0,100) > 50)  x =  InstPosRight.position.x;
		Vector3 position = new Vector3( x , Random.Range(LimitTopLeft.position.y, LimitBottomRight.position.y), 0);
		
		GameObject go = (GameObject) GameObject.Instantiate( prefabs[prefInd], position, Quaternion.identity );
		go.transform.parent = transform;
		go.transform.localScale = prefabs[prefInd].transform.localScale;
		//Debug.Log ( go.name);
	}
	 
}
