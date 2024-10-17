using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 


public class HomeScene : MonoBehaviour {

	public Image SoundOn;
	public Image SoundOff;
	public Image BtnSoundIcon;
 
 
 
	public MenuManager menuManager;
	public static  HomeScene Instance;
 
 
	public ItemsSlider fishCatalogue;
	Animator fishCatalogueAnim;

	public GameObject BtnBack;
	//public GameObject BtnShop;
	public GameObject PopUpShop;
	public GameObject PopUpSpecialOffer;
	bool bFishCatalogue = false;

	public  Text [] txtCoins;
 	

	public static bool bSkipSelectChild = false;

	public GameObject[] HideWhenCatalougeIsVisible;
		
	void Awake()
	{
		
		Input.multiTouchEnabled = false;


		Shop.Instance.txtDispalyStars =  null;


		Instance = this;
	}

	void Start () {

 
		LevelTransition.Instance.ShowScene();

		BlockClicks.Instance.SetBlockAll(true);
		BlockClicks.Instance.SetBlockAllDelay(1f,false);

	/*	if(SoundManager.soundOn == 0) 
			BtnSoundIcon.sprite = SoundOff;
		else  
			BtnSoundIcon.sprite = SoundOn;
 
*/
	 
		Shop.Instance.txtDispalyStars =  txtCoins;
		foreach(Text t in txtCoins)
		{
			t .text = GameData.TotalCoins.ToString();
		}
	 
		 

		fishCatalogueAnim  = fishCatalogue.transform.parent.GetComponent<Animator>();
		if(bSkipSelectChild)
		{
			fishCatalogueAnim.SetBool("bDefaultVisible", true);
			//roomCarousel.transform.parent.GetComponent<ItemsSlider>().StartCoroutine("Init");
		 
			BtnBack.SetActive(true);
		}
		else
		{
			BtnBack.SetActive(false);
		}

		if(SoundManager.soundOn == 1)
		{
			SoundOff.enabled = false;
			//SoundOn.enabled = true;
		}
		else
		{
			SoundOff.enabled = true;
			//SoundOn.enabled = true;
		}
	}

	
	public void ExitGame () {
		if(bFishCatalogue)
		{
			EscapeButtonManager.AddEscapeButonFunction("ExitGame");
			return;
		}
		Debug.Log("EXIT");

		if (Shop.RemoveAds !=2 )
		{
	
		}
		else
		{
			Application.Quit();
		}
	}

 
//	public void btnPlayClick()
//	{
//		SoundManager.Instance.Play_ButtonClick();
//		StartCoroutine(LoadMap());
//	 
//		BlockClicks.Instance.SetBlockAll(true);
//		BlockClicks.Instance.SetBlockAllDelay(2f,false);
//	}
//	
//	IEnumerator LoadMap()
//	{
//		yield return new WaitForSeconds(.3f);
//		SceneManager.LoadScene("SelectRoom");
//		
//	}

 

//	public void btnHelpClick()
//	{
//		SoundManager.Instance.Play_ButtonClick();
//		menuManager.ShowPopUpMenu(PopUpHelp);
//	}
//
//	public void btnCloseHelpClick()
//	{
//		SoundManager.Instance.Play_ButtonClick();
//		menuManager.ClosePopUpMenu(PopUpHelp);
//	}
	 

	public void btnSoundClicked()
	{
 
		if(SoundManager.soundOn == 1)
		{
			SoundOff.enabled = true;
			//SoundOn.enabled = false;
			SoundManager.soundOn = 0;
			SoundManager.Instance.MuteAllSounds();

		}
		else
		{
			SoundOff.enabled = false;
			//SoundOn.enabled = true;
			SoundManager.soundOn = 1;
			SoundManager.Instance.UnmuteAllSounds();
			SoundManager.Instance.Play_ButtonClick();
		//	SoundManager.Instance.Play_Music();
		}

		 
		if(SoundManager.musicOn == 1)
		{
			SoundManager.Instance.Stop_Music();
			SoundManager.musicOn = 0;
		}
		else
		{
			SoundManager.musicOn = 1;
			SoundManager.Instance.Play_Music();
		}

		PlayerPrefs.SetInt("SoundOn",SoundManager.soundOn);
		PlayerPrefs.SetInt("MusicOn",SoundManager.musicOn);
		PlayerPrefs.Save();

	 
		BlockClicks.Instance.SetBlockAll(true);
		BlockClicks.Instance.SetBlockAllDelay(.3f,false);
 

	}

	 

 
 
	public void ButtonShowFishCatalogue()
	{
		StartCoroutine("ShowFishCatalogue");
		SoundManager.Instance.Play_ButtonClick();
	}

	IEnumerator ShowFishCatalogue()
	{

		GameData.IncrementFishCatalogueButtonClickedCount();
		BlockClicks.Instance.SetBlockAll(true);
		BlockClicks.Instance.SetBlockAllDelay(1.2f,false);

		fishCatalogueAnim.SetTrigger("tShow");
		yield return new WaitForSeconds(.1f);
		foreach(GameObject go in HideWhenCatalougeIsVisible)
		{
			if(go!=null) go.SetActive(false);
 		}

		yield return new WaitForSeconds(0.2f);
		BtnBack.SetActive(true);

		bFishCatalogue = true;
		if(EscapeButtonManager.EscapeButonFunctionStack.Count > 0 && EscapeButtonManager.EscapeButonFunctionStack.Peek() != "HideFishCatalogue") 
			EscapeButtonManager.AddEscapeButonFunction("HideFishCatalogue");
	}

	public void ButtonHideFishCatalogue()
	{
		StartCoroutine("HideFishCatalogue");
		SoundManager.Instance.Play_ButtonClick();
	}

	IEnumerator HideFishCatalogue()
	{
		BlockClicks.Instance.SetBlockAll(true);
		BlockClicks.Instance.SetBlockAllDelay(0.5f,false);
		yield return new WaitForSeconds(0.2f);
		BtnBack.SetActive(false);
		foreach(GameObject go in HideWhenCatalougeIsVisible)
		{
			if(go!=null) go.SetActive(true); 
		}

		fishCatalogueAnim.SetBool("bDefaultVisible", false);
		fishCatalogueAnim.SetTrigger("tHide");
		 
		bFishCatalogue = false;
		if(EscapeButtonManager.EscapeButonFunctionStack.Count > 0 && EscapeButtonManager.EscapeButonFunctionStack.Peek() == "HideFishCatalogue") 
			EscapeButtonManager.EscapeButonFunctionStack.Pop ();

	}

 

	public void EndWatchingVideo()
	{
		 
	}


	public void btnPlayClicked(string gameLevel)
	{
		if(gameLevel == "Gameplay1")
			GameData.GameType = "pecanje";
		else GameData.GameType = "mreza";
		LevelTransition.Instance.HideSceneAndLoadNext(gameLevel);
		SoundManager.Instance.Play_ButtonClick();
		//Sagar
		//AdMobAdManager.instance.ShowInterstitialAd();
	}

	public void ButtonShopClicked()
	{
		BlockClicks.Instance.SetBlockAll(true);
		BlockClicks.Instance.SetBlockAllDelay(1f,false);
		menuManager.ShowPopUpMenu(PopUpShop);
		SoundManager.Instance.Play_ButtonClick();
	}
	
	public void ButtonCloseShopClicked()
	{
		BlockClicks.Instance.SetBlockAll(true);
		BlockClicks.Instance.SetBlockAllDelay(1f,false);
		menuManager.ClosePopUpMenu(PopUpShop);
		SoundManager.Instance.Play_ButtonClick();
	}


	public void ButtonSpecialOfferClicked()
	{
		BlockClicks.Instance.SetBlockAll(true);
		BlockClicks.Instance.SetBlockAllDelay(1f,false);
		menuManager.ShowPopUpMenu(PopUpSpecialOffer);
		SoundManager.Instance.Play_ButtonClick();
	}

	public void ButtonCloseSpecialOfferClicked()
	{
		BlockClicks.Instance.SetBlockAll(true);
		BlockClicks.Instance.SetBlockAllDelay(1f,false);
		menuManager.ClosePopUpMenu(PopUpSpecialOffer);
		SoundManager.Instance.Play_ButtonClick();
	}


}
