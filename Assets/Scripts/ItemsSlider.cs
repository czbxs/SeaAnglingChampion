using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemsSlider : MonoBehaviour,  IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
	CanvasGroup BlockAll;
	private const float inchToCm = 2.54f;
	private EventSystem eventSystem = null;
	private float dragThresholdCM =  0.5f;//  0.5f; //vrednost u cm
 
	public GameObject[] ItemPanels;
	 

	public static int ActiveItemNo = 1;
	public Sprite[] ItemSprites;
	//public ShopManager shopManager;
	public GameObject[] Items;
 
	public Vector3 HidePosition = new Vector3(20,20,0);
	public Transform InactiveItemsHolder;

	bool bEnableDrag = true;
	bool bDrag = false;
	bool bInertia = false;
	 
	float x;
	float y;
	float speedX = 0;
	float speedLimit = 2;
	float prevX=0;
	Vector3 diffPos = new Vector3(0,0,0);
	Vector3 startPos = new Vector3(0,0,0);

	Vector3 dist1;
	 
	Vector3 dist3;

	float nextItemTresholdX = 1;

	Vector3 ActiveItemPosition;
	float itemDistanceX; 

	public Button Next;
	public Button Prev;

	Transform pomItemHolder;

	void OnEnable () {
		//Debug.Log("BRISI DATA INIT"); 
		//GameData. Init();
		 
		 
		BlockAll = GameObject.Find("BlockAll").GetComponent<CanvasGroup>();

 
		if (eventSystem == null)
		{
			eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
		}
		SetDragThreshold();
 


		StartCoroutine("Init");

	}

	IEnumerator Init()
	{
		//yield return new WaitForEndOfFrame();
		yield return new WaitForSeconds(.2f);

		ActiveItemPosition =   transform.position;
		itemDistanceX = ItemPanels[2].transform.position.x - ItemPanels[1].transform.position.x;

		dist1 = new Vector3(   itemDistanceX  , 0, 0);
		dist3 = new Vector3(   itemDistanceX*3  , 0, 0);
		startPos   = new Vector3(0, ItemPanels[1].transform.position.y,ItemPanels[1].transform.position.z);
		//startPos.x = 0;
		SetItems();
	}

	public void SetItems()
	{
		//prethodni panel
		if(ActiveItemNo>1)
		{ 
			//GameObject.Instantiate(ItemPrefabs[ActiveItemNo-2]);
			pomItemHolder = ItemPanels[0].transform.Find("Item");
			if(pomItemHolder.childCount>0)
			{
				pomItemHolder.GetChild(0).position = HidePosition;
				pomItemHolder.GetChild(0).gameObject.SetActive(false);
				pomItemHolder.GetChild(0).SetParent ( InactiveItemsHolder);
			}
			Items[ActiveItemNo-2].SetActive(true);
			Items[ActiveItemNo-2].transform.SetParent(pomItemHolder);
			Items[ActiveItemNo-2].transform.localPosition = Vector3.zero;

			//ItemPanels[0].transform.FindChild("Item").GetChild(0).GetComponent<Image>().sprite = ItemSprites[ActiveItemNo-2];
			ItemPanels[0].transform.Find("Text"). GetComponent<Text>().text =   GameData.ItemsDataList[ActiveItemNo-2].name;  
//			ItemPanels[0].transform.FindChild("ImageSelected"). GetComponent<Image>().enabled =  GameData.CheckedRooms[ActiveItemNo-2];
		}
		else
		{
			pomItemHolder = ItemPanels[0].transform.Find("Item");
			if(pomItemHolder.childCount>0)
			{
				pomItemHolder.GetChild(0).position = HidePosition;
				pomItemHolder.GetChild(0).gameObject.SetActive(false);
				pomItemHolder.GetChild(0).SetParent ( InactiveItemsHolder);
			}
			Items[ItemSprites.Length-1].SetActive(true);
			Items[ItemSprites.Length-1].transform.SetParent(pomItemHolder);
			Items[ItemSprites.Length-1].transform.localPosition = Vector3.zero;

			//ItemPanels[0].transform.FindChild("Item").GetComponent<Image>().sprite = ItemSprites[ItemSprites.Length-1];
			ItemPanels[0].transform.Find("Text"). GetComponent<Text>().text =   GameData.ItemsDataList[ItemSprites.Length-1].name;  
	//		ItemPanels[0].transform.FindChild("ImageSelected"). GetComponent<Image>().enabled =  GameData.CheckedRooms[ItemSprites.Length-1];
		}



		//trenutni panel

		pomItemHolder = ItemPanels[1].transform.Find("Item");
		if(pomItemHolder.childCount>0)
		{
			pomItemHolder.GetChild(0).position = HidePosition;
			pomItemHolder.GetChild(0).gameObject.SetActive(false);
			pomItemHolder.GetChild(0).SetParent ( InactiveItemsHolder);
		}
		Items[ActiveItemNo-1].SetActive(true);
		Items[ActiveItemNo-1].transform.SetParent(pomItemHolder);
		Items[ActiveItemNo-1].transform.localPosition = Vector3.zero;

		//ItemPanels[1].transform.FindChild("Item").GetComponent<Image>().sprite = ItemSprites[ActiveItemNo-1];
		ItemPanels[1].transform.Find("Text"). GetComponent<Text>().text = GameData.ItemsDataList[ActiveItemNo-1].name;
	//	ItemPanels[1].transform.FindChild("ImageSelected"). GetComponent<Image>().enabled =  GameData.CheckedRooms[ActiveItemNo-1];


		//sledeci panel
		if(ActiveItemNo< ItemSprites.Length)
		{

			pomItemHolder = ItemPanels[2].transform.Find("Item");
			if(pomItemHolder.childCount>0)
			{
				pomItemHolder.GetChild(0).position = HidePosition;
				pomItemHolder.GetChild(0).gameObject.SetActive(false);
				pomItemHolder.GetChild(0).SetParent ( InactiveItemsHolder);
			}
			Items[ActiveItemNo].SetActive(true);
			Items[ActiveItemNo].transform.SetParent(pomItemHolder);
			Items[ActiveItemNo].transform.localPosition = Vector3.zero;

			//ItemPanels[2].transform.FindChild("Item").GetComponent<Image>().sprite = ItemSprites[ActiveItemNo];
			ItemPanels[2].transform.Find("Text"). GetComponent<Text>().text = GameData.ItemsDataList[ActiveItemNo].name;
	//		ItemPanels[2].transform.FindChild("ImageSelected"). GetComponent<Image>().enabled =  GameData.CheckedRooms[ActiveItemNo];
		}
		else
		{

			pomItemHolder = ItemPanels[2].transform.Find("Item");
			if(pomItemHolder.childCount>0)
			{
				pomItemHolder.GetChild(0).position = HidePosition;
				pomItemHolder.GetChild(0).gameObject.SetActive(false);
				pomItemHolder.GetChild(0).SetParent ( InactiveItemsHolder);
			}
			Items[0].SetActive(true);
			Items[0].transform.SetParent(pomItemHolder);
			Items[0].transform.localPosition = Vector3.zero;

			//ItemPanels[2].transform.FindChild("Item").GetComponent<Image>().sprite = ItemSprites[0];
			ItemPanels[2].transform.Find("Text"). GetComponent<Text>().text = GameData.ItemsDataList[0].name;
//			ItemPanels[2].transform.FindChild("ImageSelected"). GetComponent<Image>().enabled =  GameData.CheckedRooms[0];
		}

	}

	 
	private void SetDragThreshold()
	{
		if (eventSystem != null)
		{
			eventSystem.pixelDragThreshold = (int)( dragThresholdCM * Screen.dpi / inchToCm);

		}
	}

 
	void FixedUpdate () {
		 
 
		if(bInertia)
		{
			speedX *=.80f;
			if(speedX <0.05f && speedX > -0.05f)
			{
				speedX = 0;
				bInertia = false;
				//MoveBack
			}


			ItemPanels[1].transform.position  += new Vector3(  speedX   , 0, 0);
			ItemPanels[0].transform.position   = ItemPanels[1].transform.position  - dist1;
			ItemPanels[2].transform.position   = ItemPanels[1].transform.position  + dist1;



			if(ItemPanels[1].transform.position.x < -nextItemTresholdX || ItemPanels[1].transform.position.x >nextItemTresholdX)
			{
				bInertia = false;
				ChangeRoom();
			}
			else if (!bInertia )  StartCoroutine ("SnapToPosition");

		}


		  

	}

	void ChangeRoom()
	{
	 	if(ItemPanels[1].transform.position.x < -nextItemTresholdX ) //pomeranje u levo
		{
				ChangeNext();
		}
		else
		{
				ChangePrevious();
		}
		SwitchPlace(); 

	
	}

	void ChangeNext()
	{
		ActiveItemNo++;
		if(ActiveItemNo > ItemSprites.Length ) 
		{
			ActiveItemNo = 1;
		}

		 

		//Debug.Log("ActiveItemNo:" + ActiveItemNo);
		if(ActiveItemNo < ItemSprites.Length ) 
		{
			pomItemHolder = ItemPanels[0].transform.Find("Item");
			if(pomItemHolder.childCount>0)
			{
				pomItemHolder.GetChild(0).position = HidePosition;
				pomItemHolder.GetChild(0).gameObject.SetActive(false);
				pomItemHolder.GetChild(0).SetParent ( InactiveItemsHolder);
			}
			Items[ActiveItemNo].SetActive(true);
			Items[ActiveItemNo].transform.SetParent(pomItemHolder);
			Items[ActiveItemNo].transform.localPosition = Vector3.zero;

			//ItemPanels[0].transform.FindChild("Item").GetComponent<Image>().sprite = ItemSprites[ActiveItemNo];
			ItemPanels[0].transform.Find("Text"). GetComponent<Text>().text = GameData.ItemsDataList[ActiveItemNo].name;
	//		ItemPanels[0].transform.FindChild("ImageSelected"). GetComponent<Image>().enabled =  GameData.CheckedRooms[ActiveItemNo];
		}
		else
		{
			pomItemHolder = ItemPanels[0].transform.Find("Item");
			if(pomItemHolder.childCount>0)
			{
				pomItemHolder.GetChild(0).position = HidePosition;
				pomItemHolder.GetChild(0).gameObject.SetActive(false);
				pomItemHolder.GetChild(0).SetParent ( InactiveItemsHolder);
			}
			Items[0].SetActive(true);
			Items[0].transform.SetParent(pomItemHolder);
			Items[0].transform.localPosition = Vector3.zero;

			//ItemPanels[0].transform.FindChild("Item").GetComponent<Image>().sprite = ItemSprites[0];
			ItemPanels[0].transform.Find("Text"). GetComponent<Text>().text = GameData.ItemsDataList[0].name;
	//		ItemPanels[0].transform.FindChild("ImageSelected"). GetComponent<Image>().enabled =  GameData.CheckedRooms[0];
		}
 
		GameObject  ItemPanelTmp = ItemPanels[0];
		
		ItemPanels[0] = ItemPanels[1];
		ItemPanels[1] = ItemPanels[2];
		ItemPanels[2] = ItemPanelTmp;
		
		
		StartCoroutine ("SnapToPosition");
		//GameData.IncrementScrollCarCount();
 
		SoundManager.Instance.Play_ButtonClick();
	}

	//*******************
	void ChangePrevious()
	{
		ActiveItemNo--;
		if(ActiveItemNo < 1)  ActiveItemNo = ItemSprites.Length;

	 

		//Debug.Log("ActiveItemNo:" + ActiveItemNo);
 	
			//SoundManager.Instance.Play_PopUpHide(0f);
		if(ActiveItemNo > 1 ) 
		{
			pomItemHolder = ItemPanels[2].transform.Find("Item");
			if(pomItemHolder.childCount>0)
			{
				pomItemHolder.GetChild(0).position = HidePosition;
				pomItemHolder.GetChild(0).gameObject.SetActive(false);
				pomItemHolder.GetChild(0).SetParent ( InactiveItemsHolder);
			}
			Items[ActiveItemNo-2].SetActive(true);
			Items[ActiveItemNo-2].transform.SetParent(pomItemHolder);
			Items[ActiveItemNo-2].transform.localPosition = Vector3.zero;



			//ItemPanels[2].transform.FindChild("Item").GetComponent<Image>().sprite = ItemSprites[ActiveItemNo-2];
			ItemPanels[2].transform.Find("Text"). GetComponent<Text>().text =  GameData.ItemsDataList[ActiveItemNo-2].name; 
//			ItemPanels[2].transform.FindChild("ImageSelected"). GetComponent<Image>().enabled =  GameData.CheckedRooms[ActiveItemNo-2];
		}
		else
		{
			pomItemHolder = ItemPanels[2].transform.Find("Item");
			if(pomItemHolder.childCount>0)
			{
				pomItemHolder.GetChild(0).position = HidePosition;
				pomItemHolder.GetChild(0).gameObject.SetActive(false);
				pomItemHolder.GetChild(0).SetParent ( InactiveItemsHolder);
			}
			Items[ItemSprites.Length-1].SetActive(true);
			Items[ItemSprites.Length-1].transform.SetParent(pomItemHolder);
			Items[ItemSprites.Length-1].transform.localPosition = Vector3.zero;

			//ItemPanels[2].transform.FindChild("Item").GetComponent<Image>().sprite = ItemSprites[ItemSprites.Length-1];
			ItemPanels[2].transform.Find("Text"). GetComponent<Text>().text =  GameData.ItemsDataList[ItemSprites.Length-1].name; 
//			ItemPanels[2].transform.FindChild("ImageSelected"). GetComponent<Image>().enabled =  GameData.CheckedRooms[ItemSprites.Length-1];
		}
		

		GameObject  ItemPanelTmp = ItemPanels[2];

		ItemPanels[2] = ItemPanels[1];
		ItemPanels[1] = ItemPanels[0];
		ItemPanels[0] = ItemPanelTmp;

		 
		StartCoroutine ("SnapToPosition");
		//GameData.IncrementScrollCarCount();
		SoundManager.Instance.Play_ButtonClick();
	}

	IEnumerator SnapToPosition()
	{
		bEnableDrag = false;
		//speedX
		float i =0;
		while(   i<1.1f)
		{
			i+=0.06f;
			yield return new WaitForFixedUpdate();

			ItemPanels[1].transform.position   =   Vector3.Lerp( ItemPanels[1].transform.position, startPos  , i);
			ItemPanels[0].transform.position   = ItemPanels[1].transform.position  - dist1;
			ItemPanels[2].transform.position   = ItemPanels[1].transform.position  + dist1;
		}
		bEnableDrag = true;
	}



	void SwitchPlace()
	{
		if(ItemPanels[0].transform.position.x < itemDistanceX) ItemPanels[0].transform.position  += dist3;
		if(ItemPanels[1].transform.position.x < itemDistanceX) ItemPanels[1].transform.position  += dist3;
		if(ItemPanels[2].transform.position.x < itemDistanceX) ItemPanels[2].transform.position  += dist3;

		if(ItemPanels[0].transform.position.x > itemDistanceX) ItemPanels[0].transform.position  -= dist3;
		if(ItemPanels[1].transform.position.x > itemDistanceX) ItemPanels[1].transform.position  -= dist3;
		if(ItemPanels[2].transform.position.x > itemDistanceX) ItemPanels[2].transform.position  -= dist3;
		
	}


	public void OnBeginDrag(PointerEventData eventData)
	{
		if(MenuManager.activeMenu != "") return;
		if(!bEnableDrag) return;

		bDrag = true;
	 
		diffPos = ItemPanels[1].transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition)   ;
		diffPos = new Vector3(diffPos.x,diffPos.y,0);
		prevX = ItemPanels[1].transform.position.x;
			
	}
	
	public void OnDrag(PointerEventData eventData)
	{
		if(MenuManager.activeMenu != "") return;
		if( bEnableDrag &&  bDrag )
		{
			 
			prevX = ItemPanels[1].transform.position.x;
			x = Input.mousePosition.x;
			y = Input.mousePosition.y;
		 
			ItemPanels[1].transform.position = new Vector3(  (Camera.main.ScreenToWorldPoint(new Vector3(x ,y,100.0f)) + diffPos).x   ,  ActiveItemPosition.y,  ActiveItemPosition.z);
			 

			ItemPanels[0].transform.position   = ItemPanels[1].transform.position  - dist1;
			ItemPanels[2].transform.position   = ItemPanels[1].transform.position  + dist1;

		if( (ActiveItemNo ==1 && ItemPanels[1].transform.position.x> 1) ||  (ActiveItemNo== ItemSprites.Length &&  ItemPanels[1].transform.position.x < -1 ))
			{
				bDrag = false;
				bInertia = true;
			}
		}

	 
	}
	
	public void OnEndDrag(PointerEventData eventData)
	{
		if(  bEnableDrag &&  bDrag   )
		{
			x = Input.mousePosition.x;
			y = Input.mousePosition.y;
 	 
			ItemPanels[1].transform.position = new Vector3(  (Camera.main.ScreenToWorldPoint(new Vector3(x ,y,100.0f)) + diffPos).x   ,  ActiveItemPosition.y,  ActiveItemPosition.z);
			
			ItemPanels[0].transform.position   = ItemPanels[1].transform.position  - dist1;
			ItemPanels[2].transform.position   = ItemPanels[1].transform.position  + dist1;
 	 
			speedX =  ItemPanels[1].transform.position.x  - prevX;
			if(speedX <-speedLimit) speedX = -speedLimit;
			else if(speedX > speedLimit) speedX = speedLimit;
		 
			bDrag = false;
			bInertia = true;
 
		}
	}

	public void OnPointerClick( PointerEventData eventData)
	{
		/*
		//Tutorial.timeWaitToShowHelp = Tutorial.PeriodToShowHelp;
		if(MenuManager.activeMenu != "") return;
		if(BlockAll.blocksRaycasts) return;
		if(!eventData.dragging) 
		{
			if(GameData.ItemsDataList[ActiveItemNo-1].unlocked)
			{
				//Gameplay.roomNo = ActiveItemNo;
				//	SoundManager.Instance.Play_ButtonClick();
				//				Debug.Log("Item" + ActiveItemNo);
				//eventData.rawPointerPress.transform.parent.FindChild("ImageSelected").GetComponent<Animator>().SetTrigger("tShow");
			//	HomeScene.Instance.CarouselSelected(); 
				BlockAll.blocksRaycasts = true;

			}
			else
			{
//				if( GameData.TotalStars  >=  GameData.VehicleDataList[ActiveItemNo-1].stars  )
//				{
//					GameData.UnlockVehicle( ActiveItemNo-1);
//					ItemPanels[1].transform.FindChild("Lock"). GetComponent<Image>().enabled = !GameData.VehicleDataList[ActiveItemNo-1].unlocked;
//					Shop.Instance.AnimiranjeDodavanjaZvezdica( -GameData.VehicleDataList[ActiveItemNo-1].stars,null, ""); 
//					BlockAll.blocksRaycasts = true;
//					StartCoroutine(SetBlockAll(1f,false));
//				}
//				else
//				{
//					Debug.Log("NEMA DOVOLJNO ZVEZDICA");
					//shopManager.ShowPopUpShop();
				 	//SoundManager.Instance.Play_Error();
				//	BlockAll.blocksRaycasts = true;
				//	StartCoroutine(SetBlockAll(1f,false));
//				}
			}
		}
		*/
	}

	public void btnNext(  )
	{
		ChangeNext();
	 
		BlockAll.blocksRaycasts = true;
		StartCoroutine(SetBlockAll(.7f,false));
		//Tutorial.timeWaitToShowHelp = Tutorial.PeriodToShowHelp;
		//GameObject.Find("Tutorial").SendMessage("HidePointer");
		SoundManager.Instance.Play_ButtonClick();
	}
	
	public void btnPrevious(  )
	{
		ChangePrevious();
		 
		BlockAll.blocksRaycasts = true;
		StartCoroutine(SetBlockAll(.7f,false));
		//Tutorial.timeWaitToShowHelp = Tutorial.PeriodToShowHelp;
		//GameObject.Find("Tutorial").SendMessage("HidePointer");
		SoundManager.Instance.Play_ButtonClick();
	}

	IEnumerator SetBlockAll(float time, bool blockRays)
	{
		if(BlockAll == null) BlockAll = GameObject.Find("BlockAll").GetComponent<CanvasGroup>();
		yield return new WaitForSeconds(time);
		BlockAll.blocksRaycasts = blockRays;
		
	}
 

	 
}