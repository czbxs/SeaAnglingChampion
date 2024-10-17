using UnityEngine;
using System.Collections;
using UnityEngine.UI;

///<summary>
///<para>Scene:All/NameOfScene/NameOfScene1,NameOfScene2,NameOfScene3...</para>
///<para>Object:N/A</para>
///<para>Description: Sample Description </para>
///</summary>

public class GlobalVariables : MonoBehaviour {



	//***************************************************************

	//ovaj deo promenjivih je ubacen uz projekat draw shapes
	public static int gameMod = 0; // 0 - re-create; 1 - create your own
	public static int currentLvl = 0;
//	public static int currentCategory = 0;
//	public static int currentEpisode = 0;
	public static bool categoriesBought = false;
	//public static bool removeAds = false;
	public static int numberOfStart = 0;
	public static string mgDrawShaper_SavedProgres = "";

//	#region ADS_Numbers
//	public static int bannerID = 1;
//	public static int interstitialFinishID = 2;
//	public static int interstitialButtonID = 3;
//	public static int interstitialAppStartID = 4;
//	public static int interstitialAppExitID = 5;
//	public static int interstitialChangeColorShapeID = 6;
//	#endregion


	//***************************************************************

	//ovaj deo promenjivih je ubacen uz projekat puzzle
	public static int PuzzleDifficulty = 1;
	public static int currentCategory = 1;
	public static int currentEpisode = 1;
	//***************************************************************


	public static bool removeAdsOwned = false;
	public static string applicationID;
	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad(gameObject);
		#if UNITY_ANDROID || UNITY_EDITOR_WIN
		applicationID = "com.Test.Package.Name";
		#elif UNITY_IOS
		applicationID = ""; // "bundle.ID";
		#endif


		//draw shapes - izbor slike se ucitava samo jednom prilikom starta igre
		currentLvl = Random.Range(0,19);
	//	Debug.Log("Draw Shapes Image je " + currentLvl);

	 

	}



    public void DisableLog()
    {
        Debug.unityLogger.logEnabled = false;
    }


}
