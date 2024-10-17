using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameData : ScriptableObject {

	public static int TotalCoins = 0 ;
	public static int CollectedCoins = 0;
	public static int TimeLeft = 60;
	public static bool bOutOfTime = false;
	public static  string GameType = "pecanje"; //1- pecanje, 2- mreza
	//public static bool bWatchVideoReady = false;
	//public static bool bWatchVideoStart = false;
 
	public static bool TestTutorial = false;
	public static string Unlocked = "dafE1A";
	public static int TutorialShown = 0;
	public static List<GameItemData> ItemsDataList = new List<GameItemData>();

	public static int NetsLeft = 3;
	public static int FishingRodsLeft = 3;
	public static int BrokenItemPriceFishingRod = 50;
	public static int BrokenItemPriceNet = 100;

	public static int SmallCoinPackValue = 300;
	public static int MediumCoinPackValue = 1000;
	public static int LargeCoinPackValue = 5000;


	 
	public static string sTestiranje = "";

	public static int WatchVideoCounter = 0;

	public static int Level = 1;
	public static int UnlockedFish = 5;
	public static int StartFishCount =   15;
	 
	public static bool IsTutorialOver()
	{
		 
		if( GameType == "pecanje" && (TutorialShown==1 || TutorialShown==3)) return true;
		else if( GameType == "mreza" && (TutorialShown==2 || TutorialShown==3 )) return true;
		else return false;

	}
	public static void SetTutorialShown()
	{
		//1 samo za pecanje ,2 samo za mrezu,  3 oba,

		if( GameType == "pecanje" && TutorialShown!=1) TutorialShown +=1;
		if( GameType == "mreza" && TutorialShown!=2) TutorialShown +=2;
		if(TutorialShown >3) TutorialShown =3;
		 
		PlayerPrefs.SetInt("TUTORIAL", TutorialShown);
	}

	public static void Init()
	{
		//-----------------------------------------------------------------
		//OVO JE SAMO ZA TESTIRANJE  - OTKLJUCANO SVE
		//PlayerPrefs.SetInt("GINKO_BILOBA",2);

		if(  /*true ||  */PlayerPrefs.GetInt("GINKO_BILOBA",0) == 2) 
		{
			sTestiranje = "Test;"
			 	//+ "OverrideShopCall;"  
				//+ "TestPopUpTransaction;"	
			//	+ "WatchVideo;"
			//	+"BrokenItem;"
				+"InternetOff;"
			 ;

			Debug.Log("TESTIRANJE UKLJUCENO: " + sTestiranje);
		}
		//-----------------------------------------------------------------------

		if(ItemsDataList.Count>0) return;

		TutorialShown = PlayerPrefs.GetInt("TUTORIAL",0);
		 

		GetStarsFromPP();
		GetBrokenItems();
		GetPurchasedItems();

		ItemsDataList.Add(new GameItemData(  "clownfish", 0  ));
		ItemsDataList.Add(new GameItemData(  "regal tang", 0  ));
		ItemsDataList.Add(new GameItemData(  "pufferfish", 0 ));
		ItemsDataList.Add(new GameItemData(  "french angelfish", 0 )); 
		ItemsDataList.Add(new GameItemData(  "moorish idol", 0  ));  //5
		ItemsDataList.Add(new GameItemData(  "mantis shrimp", 5  ));
		ItemsDataList.Add(new GameItemData(  "lion fish", 10  )); 
		ItemsDataList.Add(new GameItemData(  "juvenile emperor angelfish",15  ));
		ItemsDataList.Add(new GameItemData(  "mandarinfish", 20  )); 
		ItemsDataList.Add(new GameItemData(  "tuna", 25  ));//10
		ItemsDataList.Add(new GameItemData(  "triggerfish", 30  ));
		ItemsDataList.Add(new GameItemData(  "blueface angel fish", 35  ));
		ItemsDataList.Add(new GameItemData(  "banggai cardinalfish", 40  ));
		ItemsDataList.Add(new GameItemData(  "yellow tang", 45  ));
		ItemsDataList.Add(new GameItemData(  "starfish", 50 ));
		ItemsDataList.Add(new GameItemData(  "sea horse", 55  ));
		ItemsDataList.Add(new GameItemData(  "yellow longnose butterflyfish", 60  )); //?? ?? ?? naziv yellow blue tang
		ItemsDataList.Add(new GameItemData(  "fantail goldfish", 65  ));

		ItemsDataList.Add(new GameItemData(  "octopus", 0  ));
		ItemsDataList.Add(new GameItemData(  "shark", 0  ));
		 

	//	Unlocked = 	PlayerPrefs.GetString("Data2","dafE");
 
 
		Level  = PlayerPrefs.GetInt("Level",1) ;
		SetUnlockedFish();
	}


	static void SetUnlockedFish()
	{
		UnlockedFish =  5+ Level/5;
		if(UnlockedFish > ItemsDataList.Count) UnlockedFish = ItemsDataList.Count;

//		StartFishCount = 5+Level/10;
//		if(StartFishCount >10) StartFishCount = 10;
	}

	public  static void SetNewLevelToPP(  )
	{
		Level++;
		PlayerPrefs.SetInt("Level",Level) ;
		SetUnlockedFish();
	}



	public static void GetStarsFromPP()
	{
		string tmp = PlayerPrefs.GetString("Data1","7542");
		tmp= tmp.Replace("_","9");
		tmp= tmp.Replace("76q","8");
		tmp= tmp.Replace("nmFs","7");
		tmp= tmp.Replace("Tr;","6");
		tmp= tmp.Replace("^3","5");
		tmp= tmp.Replace("D","4");
		tmp= tmp.Replace("EE","3");
		tmp= tmp.Replace("g$","2");
		tmp= tmp.Replace("=0","1");
		tmp= tmp.Replace("Ase","0");
 
//		Debug.Log( tmp );
		int tmpStars = int.Parse(tmp);
		TotalCoins = tmpStars - 7542;
//		 Debug.Log("GET TS" + tmpStars + "  " + TotalStars + "  " + tmp); 

		 
	}

 

	public  static void SetStarsToPP()
	{
		 
		string tmp = (TotalCoins+7542).ToString();

		tmp= tmp.Replace("0","Ase");
		tmp= tmp.Replace("1","=0");
		tmp= tmp.Replace("2","g$");
		tmp= tmp.Replace("3","EE");
		tmp= tmp.Replace("4","D");
		tmp= tmp.Replace("5","^3");
		tmp= tmp.Replace("6","Tr;");
		tmp= tmp.Replace("7","nmFs");
		tmp= tmp.Replace("8","76q");
		tmp= tmp.Replace("9","_");

		//Debug.Log("SNIMANJE  TS:" +TotalCoins + "  " + (TotalCoins+7542).ToString() + "  " + tmp);
		PlayerPrefs.SetString("Data1", tmp);

	  
	}

	public static void GetBrokenItems()
	{
		int tmp = PlayerPrefs.GetInt("Data2", 33 );
		NetsLeft = tmp/10;
		FishingRodsLeft = tmp -NetsLeft*10;

//		Debug.Log(NetsLeft + "  "  + FishingRodsLeft);
	}

	public static void SetBrokenItems()
	{
		PlayerPrefs.SetInt("Data2",NetsLeft*10+ FishingRodsLeft);
	//	Debug.Log(NetsLeft + "  "  + FishingRodsLeft);
	}



	public static void GetPurchasedItems()
	{
		string tmp = PlayerPrefs.GetString("Data3","22317");
		tmp= tmp.Replace("<","9");
		tmp= tmp.Replace("7>q","8");
		tmp= tmp.Replace("nmFs","7");
		tmp= tmp.Replace("Vy;","6");
		tmp= tmp.Replace("*2","5");
		tmp= tmp.Replace("H","4");
		tmp= tmp.Replace("JE","3");
		tmp= tmp.Replace("B#","2");
		tmp= tmp.Replace("+0","1");
		tmp= tmp.Replace("Kce","0");
		
		int tmpPurchased = int.Parse(tmp);
		int purchased = tmpPurchased - 22317;

 
		Shop.RemoveAds = purchased ;

		 
	 
		if(Shop.RemoveAds == 2) GlobalVariables.removeAdsOwned = true;
 
 
	}

	public static void SetPurchasedItems()
	{
  
		if(Shop.RemoveAds == 2) GlobalVariables.removeAdsOwned = true;
		 
 
		int purchased =  Shop.RemoveAds;
 
		string tmp = (purchased+22317).ToString();
		
		tmp= tmp.Replace("0","Kce");
		tmp= tmp.Replace("1","+0");
		tmp= tmp.Replace("2","B#");
		tmp= tmp.Replace("3","JE");
		tmp= tmp.Replace("4","H");
		tmp= tmp.Replace("5","*2");
		tmp= tmp.Replace("6","Vy;");
		tmp= tmp.Replace("7","nmFs");
		tmp= tmp.Replace("8","7>q");
		tmp= tmp.Replace("9","<");
  
		PlayerPrefs.SetString("Data3", tmp);
		PlayerPrefs.Save();
	}

 
	 
	//BROJANJE KLIKOVA ZA PRIKAZ REKLAMA
 
 
	 
	public static void IncrementButtonClickedCount()
	{
		if (Shop.RemoveAds !=2    )
		{ 
//			
		}
	}

	public static void IncrementFishCatalogueButtonClickedCount()
	{
		if (Shop.RemoveAds !=2    )
		{
//			
		}
	}
 
}

public class GameItemData 
{
	public string name = "";
	public int unlocked_level = 0;
 
	public GameItemData( string name, int unlocked_level )
	{
		this.name = name;
		this.unlocked_level = unlocked_level;
		 
	}
	
} 

