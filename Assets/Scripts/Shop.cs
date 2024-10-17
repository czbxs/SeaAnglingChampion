using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class Shop : MonoBehaviour {

	public static Shop Instance = null;


	 

	public int StarsToAdd  = 0;
	public int StarsToAddStart = 0;


	 
	public static int UnlockAll = 0;
	public static int RemoveAds = 0;
	public static int SmallCoinPack = 0;
	public static int MediumCoinPack = 0;
	public static int LargeCoinPack = 0;
	public static int SpecialOffer = 0;
	 

	public static bool bShowSpecialOfferInShop = false;

	public bool bShopWatchVideo = false;

	public string ShopItemID = "";

	public Text[] txtDispalyStars; //SVA POLJA NA SCENI KOJA TREBA DA SE AZURIRAJU PRILIKOM DODAVANJA ILI ODUZIMANJA ZZVEZDICA

  
	void Awake()
	{
		if(Instance !=null &&  Instance != this ) GameObject.Destroy(gameObject);
		else {  Instance = this; DontDestroyOnLoad(this.gameObject); }

		//Shop.InitShop();
		GameData.Init();
		 
	}


 
	//***************************WATCH VIDEO******************
	public void WatchVideo( )
	{
 		//zahtev da se prikaze video
		//Debug.Log(  "WATCH VIDEO");
		if( GameData.sTestiranje.Contains("WatchVideo;")   )
		{
			GameData.WatchVideoCounter = 0;
			//FinishWatchingVideo( );
			FinishWatchingVideoError();
		}
		else
		{

		}
	}


	public void FinishWatchingVideoError()
	{
		GameObject.Find("Canvas").GetComponent<MenuManager>().ShowPopUpDialogTitleText("Video not available");
		GameObject.Find("Canvas").GetComponent<MenuManager>().ShowPopUpDialogCustomMessageText("Video is not available at this moment. Thank you for understanding."); 
	}
	
	public void FinishWatchingVideo( )
	{
        if (GameData.GameType == "pecanje") {
            GameData.FishingRodsLeft = 3;

        } else if(GameData.GameType == "mreza" ) {
            GameData.NetsLeft = 3;
        }
        GameData.SetBrokenItems();

		if(Gameplay.Instance !=null) Gameplay.Instance.FinishWatchingVideo();

		if(SoundManager.Instance!=null)
		{
			SoundManager.Instance.Coins.Stop();
			SoundManager.Instance.Play_Sound(SoundManager.Instance.Coins);
		}

		 
	 
	}

	//***************************************************************
	//ODBROJAVANJE NOVCICA
	 
	public void AnimiranjeDodavanjaZvezdica(int _StarsToAdd,  Text txtStars  = null , string message = "STARS: " )
	{
		if(Application.loadedLevelName == "HomeScene") message = "";
		if(SoundManager.Instance!=null)
		{
			SoundManager.Instance.Coins.Stop();
			SoundManager.Instance.Play_Sound(SoundManager.Instance.Coins);
		}
  

		StarsToAddStart =  GameData.TotalCoins;
		GameData.TotalCoins +=_StarsToAdd;
		StarsToAdd = _StarsToAdd;
		 GameData.SetStarsToPP();

//		Debug.Log(Coins);
		StopAllCoroutines();
		if(txtStars !=null)
		{

			StartCoroutine(animShopCoins(txtStars, message ));
		}
		else 
			StartCoroutine(animShopStarsAllTextFilds( message));
	}

	IEnumerator animShopCoins( Text txtStars , string message  )
	{
 
		int  StarsToAddProg=0;

		int addC = 0;
		int stepUL = Mathf.FloorToInt(StarsToAdd*0.175f);
		int stepLL = Mathf.FloorToInt(StarsToAdd*0.19f);
 
		while( (Mathf.Abs(StarsToAddProg) + Mathf.Abs(addC)) < Mathf.Abs(StarsToAdd) )
		{
			StarsToAddProg+=addC;
			txtStars.text = message+  (StarsToAddStart + StarsToAddProg).ToString();
			//Debug.Log(CoinsToAddStart + CoinsToAddProg);
			yield return new WaitForSeconds (0.05f);
			addC = Mathf.FloorToInt(UnityEngine.Random.Range(stepLL, stepUL));
		}
		
		StarsToAddProg = StarsToAdd;
		txtStars.text = message + GameData.TotalCoins.ToString();
 
	}

	IEnumerator animShopStarsAllTextFilds(   string message  )
	{
		//AUDIO.PlaySound(  "shop_coin");
		int  StarsToAddProg=0;
		
		int addC = 0;
		int stepUL = Mathf.FloorToInt(StarsToAdd*0.175f);
		int stepLL = Mathf.FloorToInt(StarsToAdd*0.22f);
		if(txtDispalyStars!=null)
		{
			while( (Mathf.Abs(StarsToAddProg) + Mathf.Abs(addC)) < Mathf.Abs(StarsToAdd) )
			{
				StarsToAddProg+=addC;
				for(int i = 0; i<txtDispalyStars.Length;i++)
				{
					if(txtDispalyStars[i].text == "") continue;
					if(txtDispalyStars[i].text.Contains("/"))
					{
						string[] split = txtDispalyStars[i].text.Split('/');
						txtDispalyStars[i].text = message +  (StarsToAddStart + StarsToAddProg).ToString() + "/" + split[1];
					}
					else
						txtDispalyStars[i].text = message +  (StarsToAddStart + StarsToAddProg).ToString();
				}
				//Debug.Log(CoinsToAddStart + CoinsToAddProg);
				yield return new WaitForSeconds (0.05f);
				addC = Mathf.FloorToInt(UnityEngine.Random.Range(stepLL, stepUL));
			}
			
			StarsToAddProg = StarsToAdd;

			for(int i = 0; i<txtDispalyStars.Length;i++)
			{
				if(txtDispalyStars[i].text == "") continue;
				if(txtDispalyStars[i].text.Contains("/"))
				{
					string[] split = txtDispalyStars[i].text.Split('/');
					txtDispalyStars[i].text =  message + (StarsToAddStart + StarsToAddProg).ToString() + "/" + split[1];
				}
				else
					txtDispalyStars[i].text =  message +  GameData.TotalCoins.ToString();
			}


		}
//		DataManager.Instance.SaveLastLevelData();
//		Debug.Log(" ** " + Coins);
	}


	
	//********************************************************

	//***************************************************************
	//ODBROJAVANJE DODAVANJA VREDNOSTI
	public Text[] txtFields;
	int StartVal = 0;
	int ValToAdd = 0;

	public void AnimiranjeDodavanjaVrednosti ( int _Start,  int _Add,   string message = "" )
	{
		if(SoundManager.Instance!=null)
		{
			SoundManager.Instance.Coins.Stop();
			SoundManager.Instance.Play_Sound(SoundManager.Instance.Coins);
		}
		 
		StartVal =  _Start;
 
		ValToAdd = _Add;
		//StopAllCoroutines();
		if(txtFields !=null)
			StartCoroutine(animValue(  message ));
		 
	}
	
	 
	
	IEnumerator animValue(   string message = ""  )
	{
		//AUDIO.PlaySound(  "shop_coin");
		int  ValToAddProg=0;
		
		int addC = 0;
		int stepUL = Mathf.FloorToInt(ValToAdd*0.175f);
		int stepLL = Mathf.FloorToInt(ValToAdd*0.22f);
		if(stepLL == 0 ) stepLL =1;
		if(stepUL ==0 ) stepUL =1;
		if(txtFields!=null)
		{
			while( (Mathf.Abs(ValToAddProg) + Mathf.Abs(addC)) < Mathf.Abs(ValToAdd) )
			{
				ValToAddProg+=addC;
				for(int i = 0; i<txtFields.Length;i++)
				{
					txtFields[i].text = message+  (StartVal + ValToAddProg).ToString();
				}
 
				yield return new WaitForSeconds (0.05f);
				addC = Mathf.FloorToInt(UnityEngine.Random.Range(stepLL, stepUL));
			}
			
			ValToAddProg = ValToAdd;
//			Debug.Log(StartVal + ValToAddProg);
			for(int i = 0; i<txtFields.Length;i++)
			{
				txtFields[i].text = message + (StartVal +ValToAdd).ToString();
			}
		}
	}
	
	
	
	//***********************KUPOVINA *********************************

	 

	public void SendShopRequest(string _shopItemId)
	{
	 
		ShopItemID = _shopItemId;

 
		string __shopItemId = "";
		switch(_shopItemId)
		{
		case "RemoveAds":
			__shopItemId = "remove_ads";
			break;
 
		case "SmallCoinPack":
			__shopItemId = "small_coin_pack";
			break;

		case "MediumCoinPack":
			__shopItemId = "medium_coin_pack";
			break;

		case "LargeCoinPack":
			__shopItemId = "large_coin_pack";
			break;

		case "SpecialOffer":
			__shopItemId = "special_offer";
			break;
	 
 
		}
		if( !GameData.sTestiranje.Contains("OverrideShopCall;"))
		{
			//Debug.Log("ZAHTEV ZA KUPOVINU: " + __shopItemId);
			 //POZIV U NATIVE ZA KUPOVINU
		}
		else
		{
			//brisi TEST
			//Debug.Log("BRISI : TRANSACTION CONFIRMED");
			StartCoroutine (CONFIRM(_shopItemId));
		}

	}

	IEnumerator CONFIRM(string _shopItemId)
	{
		yield return new WaitForSeconds(1);
		//test failed 
		//GameObject.Find("Canvas").GetComponent<MenuManager>().ShowPopUpDialogTitleText("InApp Currently Not Available");
		//GameObject.Find("Canvas").GetComponent<MenuManager>().ShowPopUpDialogCustomMessageText("This InApp purchase is not available at this moment. Thank you for understanding.");

		//test CONFIRMED
		ShopTransactionConfirmed(  _shopItemId);
	}


	public void ShopTransactionConfirmed(string _shopItemId)
	{
 
		//ShopManager shopManager=null;
		//if( GameObject.Find ("PopUpShop")!=null) shopManager = GameObject.Find ("PopUpShop").GetComponent<ShopManager>();
		ShopItemID = _shopItemId;
	 
		GameObject go =  GameObject.Find("CanvasBG/MainMenu/ButtonsHolder/ItemsSlider");
		switch(ShopItemID )
		{

		case "RemoveAds":
			RemoveAds = 2;
			GameData.SetPurchasedItems();

			break;

		 
		case "SmallCoinPack":
			AnimiranjeDodavanjaZvezdica(GameData.SmallCoinPackValue, null, ""); 
	
			break;

		case "MediumCoinPack":
			AnimiranjeDodavanjaZvezdica(GameData.MediumCoinPackValue, null, ""); 

			break;

		case "LargeCoinPack":
			AnimiranjeDodavanjaZvezdica(GameData.LargeCoinPackValue, null, ""); 

			break;

		case "SpecialOffer":
			RemoveAds = 2;
			SpecialOffer = 2;
			GlobalVariables.removeAdsOwned = true;
			GameData.SetPurchasedItems();

			AnimiranjeDodavanjaZvezdica(GameData.LargeCoinPackValue, null, ""); 

			//			
			break;
	 
		}
		ShopItemID = "";
 
		//if(shopManager!=null)  shopManager.SetShopItems();
		if(SoundManager.Instance !=null ) SoundManager.Instance.Stop_Sound(SoundManager.Instance.InAppBought);
	}
  

 
	public void ReturnRestoreData(string shopItemsData)
	{
		
		
	}
 

 

  
}
