﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
 

/**
  * Scene:Splash
  * Object:Main Camera
  * Description: F-ja zaduzena za ucitavanje MainScene-e, kao i vizuelni prikaz inicijalizaije CrossPromotion-e i ucitavanja scene
  **/
public class SplashScene : MonoBehaviour {
	
	int appStartedNumber;
	AsyncOperation progress = null;
//	Image progressBar;
	float myProgress=0;
	string sceneToLoad;

    void Awake()
    {
        
    }

	// Use this for initialization
	void Start ()
	{
//		GameData.Init();
//		Shop.InitShop();

//		if(PlayerPrefs.HasKey("TutorialCompleted"))
//		{
		sceneToLoad =  "HomeScene";
//		}
//		else
//			sceneToLoad = "TutorialLevel";
		
//		progressBar = GameObject.Find("ProgressBar").GetComponent<Image>();
		if(PlayerPrefs.HasKey("appStartedNumber"))
		{
			appStartedNumber = PlayerPrefs.GetInt("appStartedNumber");
		}
		else
		{
			appStartedNumber = 0;
		}
		appStartedNumber++;
		PlayerPrefs.SetInt("appStartedNumber",appStartedNumber);

		StartCoroutine(LoadScene());
	}
	
	/// <summary>
	/// Coroutine koja ceka dok se ne inicijalizuje CrossPromotion, menja progres ucitavanja CrossPromotion-a, kao i progres ucitavanje scene, i taj progres se prikazuje u Update-u
	/// </summary>
	IEnumerator LoadScene()
	{
		//Debug.Log("interstitialInitialized");
//		while(myProgress < 0.25)
//		{
//			myProgress += 0.05f;
//			progressBar.fillAmount=myProgress;
//			yield return new WaitForSeconds(0.05f);
//		}
		//Debug.Log("offerwallInitialized");

		while(myProgress < 1)
		{
			myProgress += 0.05f;
//			progressBar.fillAmount=myProgress;
			yield return new WaitForSeconds(0.25f);
		}

		//FakeLoading.showFakeLoading = true;
//		
		progress = Application.LoadLevelAsync(sceneToLoad);
		
		yield return progress;
		
	}
	
//	void Update()
//	{
//		if(progress != null && progress.progress>0.49f)
//		{
//			progressBar.fillAmount = progress.progress;
//		}
//		
//	}
}
