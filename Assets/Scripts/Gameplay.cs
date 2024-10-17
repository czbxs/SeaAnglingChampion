using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Gameplay : MonoBehaviour
{

    public static Gameplay Instance;

    public bool bPause = false;


    public MenuManager menuManager;
    public GameObject PopUpPause;
    public GameObject PopUpBrokenItem;
    public GameObject PopUpShop;
    public GameObject PopUpEndGame;
    public Button ButtonWatchVideo;

    public GameObject PopUpGoldChest;
    public GameObject PopUpTimeChest;
    public GameObject PopUpEmptyChest;
    public GameObject PopUpOctopusInk;

    public Animator PopUpPauseAnim;

    Vector3 FishingStringStartRot;
    Vector3 FishingStringEndRot;
    Vector2 FishingStringStartSize;
    public RectTransform FishingString;
    public Image FishingStringEnd; //mreza ili udica
    bool bFisnigWithNet = false;
    public bool bScaleEnd = false;
    Vector3 FishingStringEndStartScale;

    public Image NetOpen;
    public Image NetClosed;

    public bool bFishing = false;
    public bool bFishCaught = false;
    bool bOctopusCaught = false;
    bool bMedusaCaught = false;

    bool bMoveForward = false;
    bool bMoveBack = false;

    float moveBackTime = 2f;


    Vector2 TargetPos = Vector2.zero;
    float TargetDist = 0;
    Vector2 FishingStringEndSize;
    float FishingStringStartLength = 0f;
    float FishingStringStartDist = 0f;

    public float stringSpeed = 1;
    public Text[] TxtCoins;
    public Text[] GameCoins;
    Vector3 fishCauhtPos;

    FishController fc;
    public Transform AllFishes;

    FishController[] fcNet;

    public MedusaScript medusa;
    public string chestReward = "";
    public OctopusScript oktopod;
    public SharkScript shark;
    public Animator PenguinAnim;

    public Transform Bucket;

    public Button[] BlockOnOutOfTime;

    bool appFoucs = true;
    bool bPopUpAutoHide = false;
    float catchTestSize;

    void Awake()
    {
        GameData.bOutOfTime = false;
        GameData.TimeLeft = 60;

        bPause = true;
        Instance = this;
        Input.multiTouchEnabled = false;

    }

    IEnumerator Start()
    {
        bFishing = false;
        Shop.Instance.txtDispalyStars = TxtCoins;
        foreach (Text txt in TxtCoins)
        {
            txt.text = /*"TOTAL COINS: " +*/ GameData.TotalCoins.ToString();
        }

        GameData.CollectedCoins = 0;
        foreach (Text txt in GameCoins)
        {
            txt.text = GameData.CollectedCoins.ToString();
        }


        FishingStringStartRot = FishingString.transform.rotation.eulerAngles;
        FishingStringStartSize = FishingString.sizeDelta;
        FishingStringStartLength = FishingStringStartSize.y;
        FishingStringStartDist = Vector2.Distance((Vector2)FishingString.transform.position, (Vector2)FishingStringEnd.transform.position);


        BlockClicks.Instance.SetBlockAll(true);
        BlockClicks.Instance.SetBlockAllDelay(1.5f, false);





        if (GameData.sTestiranje.Contains("WatchVideo")) //TESTIRANJE
        {
            //			SetWatchVideoButtons();
        }
        else
        {

            //AdsManager.Instance.IsVideoRewardAvailable ();

        }

        yield return new WaitForSeconds(1.1f);
        /*
		if(GameData.sTestiranje.Contains("BrokenItem;")) 
		{
			GameData.FishingRodsLeft =1; //testiranje 
		}

*/
        FishingStringEndStartScale = FishingStringEnd.transform.localScale;
        LevelTransition.Instance.ShowScene();



        if (GameData.GameType == "pecanje")
        {
            PenguinAnim.SetBool("bFishing", false);
            PenguinAnim.SetTrigger("tFishingIdle");
            PenguinAnim.SetBool("bNet", false);

            if (GameData.FishingRodsLeft > 0) GameData.FishingRodsLeft--;
            if (GameData.FishingRodsLeft == 0)
            {
                menuManager.ShowPopUpMenu(PopUpBrokenItem);
            }
            else
            {
                //				Debug.Log("Start game");
                bPause = false;

            }
            catchTestSize = 0.2f;
        }
        else if (GameData.GameType == "mreza")
        {
            PenguinAnim.SetBool("bFishing", false);
            PenguinAnim.SetTrigger("tFishingIdle");
            PenguinAnim.SetBool("bNet", true);

            if (GameData.NetsLeft > 0) GameData.NetsLeft--;
            if (GameData.NetsLeft == 0)
            {
                menuManager.ShowPopUpMenu(PopUpBrokenItem);
            }
            else
            {
                //Debug.Log("Start game");
                bPause = false;

            }
            catchTestSize = 1f;

        }
        GameData.SetBrokenItems();

        //TESTIRANJE TUTORIJALA
        if (GameData.IsTutorialOver())
        {
            GameObject.Destroy(Tutorial.Instance.gameObject);
        }
        else
        {
            Tutorial.Instance.StartTutorial();
        }


        bFisnigWithNet = bScaleEnd;
    }


    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    void Update()
    {

        if (!bFishing && !bPause && Input.GetMouseButtonDown(0))
        {

            if (IsPointerOverUIObject())
            //if(EventSystem.current.IsPointerOverGameObject())
            {
                //				Debug.Log("Clicked on the UI");
                return;
            }
            if (SharkScript.bSharkActive)
            {
                //tOpenJaws;
                shark.OpenJaws();
                return;
            }

            bOctopusCaught = false;
            bMedusaCaught = false;
            bFishCaught = false;
            bFishing = true;
            moveBackTime = 2;
            bMoveBack = false;
            TargetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (!bFisnigWithNet)
            {
                PenguinAnim.SetBool("bFishing", true);
                PenguinAnim.Play("PenguinFishingAnimation", -1, 0);
            }
            else
            {
                PenguinAnim.SetBool("bFishing", true);
                PenguinAnim.Play("PenguinFishingNetAnimation", -1, 0);
            }

            bMoveForward = true;
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(TargetPos, catchTestSize, 1 << LayerMask.NameToLayer("Octopus")); //layermask to filter the varius colliders
            if (hitColliders.Length > 0)
            {
                //skripta je naknadno izmenjena (oktopod sa blagom je zamenjen meduzom)
                if (hitColliders[0].name == "medusa" &&
                   hitColliders[0].transform.position.x > AllFishesController.Instance.LimitTopLeft.position.x
                   && hitColliders[0].transform.position.x < AllFishesController.Instance.LimitBottomRight.position.x)  //upecana je meduza
                {
                    bMedusaCaught = true;
                    chestReward = medusa.chestReward;
                    //Debug.Log(" sadrzaj kovcega "+ MedusaScript.chestReward);

                    medusa.MedusaCaught();
                    bMoveBack = false;

                    if (chestReward == "") TargetPos = medusa.octopus.transform.position;
                    else TargetPos = medusa.chest.transform.position;

                    TargetDist = Vector2.Distance(TargetPos, (Vector2)FishingString.transform.position);
                    Vector2 v_diff = (TargetPos - (Vector2)FishingString.transform.position);
                    float atan2 = Mathf.Atan2(v_diff.y, v_diff.x);
                    FishingStringEndRot = new Vector3(0f, 0f, atan2 * Mathf.Rad2Deg + 90);
                    FishingString.eulerAngles = FishingStringEndRot;
                    FishingStringEndSize = new Vector2(FishingStringStartSize.x, FishingStringStartLength * TargetDist / FishingStringStartDist);


                }

                else if (hitColliders[0].name != "medusa") //upecana je hobotnica
                {

                    bOctopusCaught = true;
                    chestReward = "";
                    TargetPos = hitColliders[0].transform.position;
                    oktopod.OctopusCaught();

                    TargetDist = Vector2.Distance(TargetPos, (Vector2)FishingString.transform.position);
                    Vector2 v_diff = (TargetPos - (Vector2)FishingString.transform.position);
                    float atan2 = Mathf.Atan2(v_diff.y, v_diff.x);
                    FishingStringEndRot = new Vector3(0f, 0f, atan2 * Mathf.Rad2Deg + 90);
                    FishingString.eulerAngles = FishingStringEndRot;
                    FishingStringEndSize = new Vector2(FishingStringStartSize.x, FishingStringStartLength * TargetDist / FishingStringStartDist);

                    SoundManager.Instance.Play_Sound(SoundManager.Instance.FishCaught);

                    if (bFisnigWithNet) IspitivanjePecanjaMrezom();

                }
                else
                {
                    if (!bFisnigWithNet)
                    {
                        StartCoroutine(IspitivanjePecanjaStapom());
                    }
                    else
                    {
                        IspitivanjePecanjaMrezom();
                    }
                }


            }
            else
            {
                if (!bFisnigWithNet)
                {
                    StartCoroutine(IspitivanjePecanjaStapom());
                }
                else
                {
                    IspitivanjePecanjaMrezom();
                }
            }


        }

    }






    void FixedUpdate()
    {
        if (bFishing && bMoveForward && !bPause) // da nije pauza
        {



            if (FishingString.sizeDelta.y < FishingStringEndSize.y)//razvlacenje strune
            {
                FishingString.sizeDelta += new Vector2(0, stringSpeed * Time.fixedDeltaTime);
                if (bScaleEnd)
                {
                    FishingStringEnd.transform.localScale = Vector3.Lerp(FishingStringEnd.transform.localScale, 4 * FishingStringEndStartScale, 2 * Time.fixedDeltaTime); //SKALIRANJE MREZE
                }

                //PUFFER FISH
                if (!bFisnigWithNet && fc != null && fc.bScaleWhenCaught)
                {
                    if (fc.transform.localScale.x > 0) fc.transform.localScale = Vector3.Lerp(fc.transform.localScale, Vector3.one * 3f, 2 * Time.fixedDeltaTime);
                    else fc.transform.localScale = Vector3.Lerp(fc.transform.localScale, new Vector3(-3, 3, 3), 2 * Time.fixedDeltaTime);
                }
                else if (bFisnigWithNet && fcNet != null)
                {
                    for (int i = 0; i < fcNet.Length; i++)
                    {
                        if (fcNet[i] != null && fcNet[i].bScaleWhenCaught)
                        {

                            if (fcNet[i].transform.localScale.x > 0) fcNet[i].transform.localScale = Vector3.Lerp(fcNet[i].transform.localScale, Vector3.one * 1.5f, 2 * Time.fixedDeltaTime);
                            else fcNet[i].transform.localScale = Vector3.Lerp(fcNet[i].transform.localScale, new Vector3(-1.5f, 1.5f, 1.5f), 2 * Time.fixedDeltaTime);
                        }
                    }
                }

            }
            else //struna je razvucen do cilja
            {
                bMoveForward = false;
                moveBackTime = 0f;

                if (bFishCaught)
                {
                    StartCoroutine("PlayAnimCatchFish");
                    if (bOctopusCaught && bFisnigWithNet)
                    {
                        StartCoroutine("OctopodCaught");
                    }
                }
                else if (bMedusaCaught)
                {
                    StartCoroutine("MedusaCaught");

                }
                else if (bOctopusCaught)
                {
                    StartCoroutine("OctopodCaught");
                }
                else
                {
                    bMoveBack = true;
                }
            }
        }

        if (bFishing && bMoveBack && !bPause)
        {

            if (FishingString.sizeDelta.y > FishingStringStartSize.y)
            {
                // privlacenje strune
                FishingString.sizeDelta -= new Vector2(0, stringSpeed * Time.fixedDeltaTime);
                if (bFisnigWithNet && fcNet != null)
                {
                    for (int i = 0; i < fcNet.Length; i++)
                    {
                        if (fcNet[i] != null)
                        {
                            fcNet[i].TargetPostition = NetClosed.transform;
                            fcNet[i].enablePositionOffset = false;
                        }
                    }
                }
            }
            else
            {
                if (moveBackTime == 0 && bFishCaught)
                {
                    //PenguinAnim.SetTrigger("tFishCaught");
                    if (!bFisnigWithNet) PenguinAnim.Play("PenguinFishCaughtAnimation", -1, 0);
                    else PenguinAnim.Play("PenguinFishCaughtAnimationNet", -1, 0);
                    SoundManager.Instance.Play_Sound(SoundManager.Instance.FishCaught);
                }
                else if (moveBackTime == 0 && !bOctopusCaught && !bFishCaught && !bMedusaCaught)
                {
                    //PenguinAnim.SetTrigger("tFishMissed");
                    PenguinAnim.Play("PenguinFishMissedAnimation", -1, 0);
                    SoundManager.Instance.Play_Sound(SoundManager.Instance.FishMissed);
                }

                moveBackTime += Time.fixedDeltaTime;
                if (moveBackTime <= .1f)
                {

                    //vracanje u vertikalni polozaj
                    FishingString.transform.eulerAngles = Vector3.Lerp(FishingStringEndRot, FishingStringStartRot, (moveBackTime) * 10);
                    PenguinAnim.SetBool("bFishing", false);

                }
                else
                {

                    //zavrseno  privlacenje strune
                    FishingString.sizeDelta = FishingStringStartSize;
                    FishingString.transform.eulerAngles = FishingStringStartRot;
                    bFishing = false;
                    bMoveBack = false;
                    FishingStringEnd.transform.localScale = FishingStringEndStartScale;

                    if (bMedusaCaught)
                    {
                        if (chestReward == "gold")
                        {

                            menuManager.ShowPopUpMenu(PopUpGoldChest);
                            StartCoroutine("ClosePopupCatched", PopUpGoldChest);

                        }

                        else if (chestReward == "time")
                        {

                            //GameData.TimeLeft+=15;
                            menuManager.ShowPopUpMenu(PopUpTimeChest);
                            StartCoroutine("ClosePopupCatched", PopUpTimeChest);
                        }

                        else if (chestReward == "empty")
                        {

                            menuManager.ShowPopUpMenu(PopUpEmptyChest);
                            StartCoroutine("ClosePopupCatched", PopUpEmptyChest);
                        }

                        medusa.MedusaCaughtEnd();
                        if (NetOpen != null) NetOpen.enabled = true;
                        if (NetClosed != null) NetClosed.enabled = false;

                        bPause = true;


                    }
                    else if (bOctopusCaught) //oktopod
                    {

                        if (NetOpen != null) NetOpen.enabled = true;
                        if (NetClosed != null) NetClosed.enabled = false;
                        oktopod.OctopusCaughtEnd();
                        bPause = true;
                        menuManager.ShowPopUpMenu(PopUpOctopusInk);
                        StartCoroutine("ClosePopupInk", PopUpOctopusInk);
                    }

                    //	else
                    if (bFishCaught)
                    {
                        if (!bFisnigWithNet)
                        {
                            GameData.CollectedCoins++;
                            foreach (Text txt in GameCoins)
                            {
                                txt.text = GameData.CollectedCoins.ToString();
                            }
                            // Debug.Log("KOFA");
                            //if(fc!=null) GameObject.Destroy(fc.gameObject);
                            if (fc != null) fc.MoveToBucket(Bucket);
                            fc = null;
                        }
                        else
                        {

                            if (NetOpen != null) NetOpen.enabled = true;
                            if (NetClosed != null) NetClosed.enabled = false;
                            //--------------------------------------------------------------------------------------------------------------
                            for (int i = 0; i < fcNet.Length; i++)
                            {
                                if (fcNet[i] != null)
                                {
                                    //Debug.Log("KOFA");
                                    //GameObject.Destroy(fcNet[i].gameObject);
                                    GameData.CollectedCoins++;
                                    if (fcNet[i] != null) fcNet[i].MoveToBucket(Bucket);
                                    fcNet[i] = null;
                                }
                            }

                            foreach (Text txt in GameCoins)
                            {
                                txt.text = GameData.CollectedCoins.ToString();
                            }
                            //********************************************************************
                        }
                    }

                }
            }

        }


    }

    public void UpdateCoins()
    {
        foreach (Text txt in GameCoins)
        {
            txt.text = GameData.CollectedCoins.ToString();
        }
    }

    IEnumerator ClosePopupInk(GameObject activePopUp)
    {
        bPopUpAutoHide = true;
        yield return new WaitForSeconds(5f);
        menuManager.ClosePopUpMenu(activePopUp);
        yield return new WaitForSeconds(.8f);
        if (MenuManager.activeMenu == "") bPause = false;
        bPopUpAutoHide = false;
    }

    IEnumerator ClosePopupCatched(GameObject activePopUp)
    {
        bPopUpAutoHide = true;
        yield return new WaitForSeconds(1);

        string trigger = "tEmpty";
        if (chestReward == "time") trigger = "tTime";
        else if (chestReward == "gold") trigger = "tGold";

        activePopUp.transform.Find("AnimationHolder/Body/CHEST/AnimatorChest").GetComponent<Animator>().SetTrigger(trigger);
        yield return new WaitForSeconds(5f);
        activePopUp.transform.Find("AnimationHolder/Body/CHEST/AnimatorChest").GetComponent<Animator>().SetTrigger("default");
        activePopUp.transform.Find("AnimationHolder/Body/PenguinCharacter 1/AnimationHolder").GetComponent<Animator>().SetTrigger("tIdle");
        yield return new WaitForSeconds(.2f);
        menuManager.ClosePopUpMenu(activePopUp);
        yield return new WaitForSeconds(.8f);
        if (MenuManager.activeMenu == "") bPause = false;
        medusa.ChestCaptured();
        bPopUpAutoHide = false;
    }

    //	void TestFishCount()
    //	{
    //		if(AllFishes.childCount == 0)
    //		{
    //			Debug.Log("KRAJ");
    //			if(SoundManager.Instance !=null ) SoundManager.Instance.Stop_Sound(SoundManager.Instance.TimeCountdown);
    //			menuManager.ShowPopUpMenu( PopUpEndGame );
    //			bPause = true;
    //			BlockClicks.Instance.SetBlockAll(true);
    //			BlockClicks.Instance.SetBlockAllDelay(1f,false);
    //			
    //			StartCoroutine("SetScore");
    //		}
    //	}

    IEnumerator MedusaCaught()
    {
        yield return new WaitForSeconds(0.3f);
        bMoveBack = true;
        //Debug.Log("uhvacena meduza");

        medusa.bCopyPos = true;
        medusa.CopyPos = FishingStringEnd.transform;
        FishingStringEnd.transform.localScale = FishingStringEndStartScale;
        if (bScaleEnd)
        {
            if (NetOpen != null) NetOpen.enabled = false;
            if (NetClosed != null) NetClosed.enabled = true;
        }
    }


    IEnumerator OctopodCaught()
    {
        yield return new WaitForSeconds(0.3f);
        bMoveBack = true;
        //Debug.Log("uhvacen oktopod");

        oktopod.bCopyPos = true;
        oktopod.CopyPos = FishingStringEnd.transform;
        FishingStringEnd.transform.localScale = FishingStringEndStartScale;
        if (bScaleEnd)
        {
            if (NetOpen != null) NetOpen.enabled = false;
            if (NetClosed != null) NetClosed.enabled = true;
        }
    }




    IEnumerator PlayAnimCatchFish()
    {
        yield return new WaitForSeconds(0.3f);
        bMoveBack = true;

        //Debug.Log("pokreni animaciju uhhvacena");


        FishingStringEnd.transform.localScale = FishingStringEndStartScale;

        if (!bFisnigWithNet)
        {
            //fishCauhtPos = fc.transform.position;
            fc.transform.SetParent(FishingStringEnd.transform);

            //fc.transform.position = fishCauhtPos;
        }
        else
        {
            if (bScaleEnd)
            {
                if (NetOpen != null) NetOpen.enabled = false;
                if (NetClosed != null) NetClosed.enabled = true;

                for (int i = 0; i < fcNet.Length; i++)
                {
                    //fcNet[i].enabled = false;
                    fcNet[i].transform.SetParent(FishingStringEnd.transform);
                }
            }
        }

    }






    void IspitivanjePecanjaMrezom()
    {


        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(TargetPos, catchTestSize, 1 << LayerMask.NameToLayer("Fish")); //layermask to filter the varius colliders
        if (hitColliders.Length > 0)
        {
            bFishCaught = true;
            //Debug.Log("FISH");

            //ULOV OGRANICEN NA 5 RIBA
            if (hitColliders.Length > 5) fcNet = new FishController[5];
            else fcNet = new FishController[hitColliders.Length];

            for (int i = 0; (i < hitColliders.Length && i < 5); i++)
            {
                if (hitColliders[i].transform.GetComponent<FishController>() != null)
                {
                    fcNet[i] = hitColliders[i].transform.GetComponent<FishController>();

                    GameObject parentOld = null;
                    fcNet[i].FishCaught();

                    if (fcNet[i].transform.parent.childCount == 2)
                    {
                        parentOld = fcNet[i].transform.parent.gameObject;
                    }

                    fcNet[i].transform.SetParent(FishingString.parent.transform);
                    if (parentOld != null) GameObject.Destroy(parentOld, 3f);
                }
            }

        }
        else
        {
            //nema ulova - sound error
            bFishCaught = false;
            //SoundManager.Instance.Play_Sound(SoundManager.Instance.FishMissed);
        }

        TargetDist = Vector2.Distance(TargetPos, (Vector2)FishingString.transform.position);
        Vector2 v_diff = (TargetPos - (Vector2)FishingString.transform.position);
        float atan2 = Mathf.Atan2(v_diff.y, v_diff.x);
        FishingStringEndRot = new Vector3(0f, 0f, atan2 * Mathf.Rad2Deg + 90);
        FishingString.eulerAngles = FishingStringEndRot;
        FishingStringEndSize = new Vector2(FishingStringStartSize.x, FishingStringStartLength * TargetDist / FishingStringStartDist);

    }



    IEnumerator IspitivanjePecanjaStapom()
    {
        yield return new WaitForEndOfFrame();

        fc = null;
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(TargetPos, catchTestSize, 1 << LayerMask.NameToLayer("Fish")); //layermask to filter the varius colliders
        if (hitColliders.Length > 0)
        {
            bFishCaught = true;
            //Debug.Log("FISH");
            Collider2D closest = hitColliders[0];
            float dist = Vector2.Distance(TargetPos, hitColliders[0].transform.position);
            float dist2 = TargetDist;
            for (int i = 1; i < hitColliders.Length; i++)
            {
                dist2 = Vector2.Distance(TargetPos, hitColliders[i].transform.position);
                if (dist2 < dist)
                {
                    dist = dist2;
                    closest = hitColliders[i];
                }
            }
            TargetPos = closest.transform.position;
            fc = closest.transform.GetComponent<FishController>();
            if (fc != null)
            {
                GameObject parentOld = null;
                fc.FishCaught();


                if (fc.transform.parent.childCount == 2)
                {
                    parentOld = fc.transform.parent.gameObject;
                }

                fc.enabled = false;
                fc.transform.SetParent(FishingString.parent.transform);


                if (parentOld != null) GameObject.Destroy(parentOld, .1f);
            }
            //closest.transform.SendMessage("FishCaught",SendMessageOptions.DontRequireReceiver);

            //			 GameData.CollectedCoins ++;
            //			foreach(Text txt in GameCoins)
            //			{
            //				txt.text =   GameData.CollectedCoins.ToString();
            //			}

            //SoundManager.Instance.Play_Sound(SoundManager.Instance.FishCaught);
        }
        else
        {
            //nema ulova - sound error
            //SoundManager.Instance.Play_Sound(SoundManager.Instance.FishMissed);
            bFishCaught = false;
        }

        TargetDist = Vector2.Distance(TargetPos, (Vector2)FishingString.transform.position);
        Vector2 v_diff = (TargetPos - (Vector2)FishingString.transform.position);
        float atan2 = Mathf.Atan2(v_diff.y, v_diff.x);
        FishingStringEndRot = new Vector3(0f, 0f, atan2 * Mathf.Rad2Deg + 90);
        FishingString.eulerAngles = FishingStringEndRot;
        FishingStringEndSize = new Vector2(FishingStringStartSize.x, FishingStringStartLength * TargetDist / FishingStringStartDist);

    }









    public void OutOfTime()
    {

        StopAllCoroutines();
        BlockClicks.Instance.SetBlockAll(true);

        BlockClicks.Instance.SetBlockAllDelay(5f, false);

        foreach (Button bt in BlockOnOutOfTime) bt.interactable = false;

        Debug.Log("PAUZA");
        bPause = true;
        GameData.SetNewLevelToPP();
        GameData.TimeLeft = -1;
        //Debug.Log("KRAJ");
        if (SoundManager.Instance != null) SoundManager.Instance.Stop_Sound(SoundManager.Instance.TimeCountdown);
        SoundManager.Instance.Play_Sound(SoundManager.Instance.LevelFinished);
        menuManager.ShowPopUpMenu(PopUpEndGame);


        PenguinAnim.SetBool("bFishing", false);

        StartCoroutine("SetScore");

        SoundManager.Instance.StartCoroutine(SoundManager.Instance.FadeOutAndIn(SoundManager.Instance.GameplayMusic, SoundManager.Instance.LevelFinished.clip.length));

    }

    IEnumerator SetScore()
    {

        yield return new WaitForSeconds(1.0f);

        Shop.Instance.AnimiranjeDodavanjaZvezdica(GameData.CollectedCoins, null, "");
        yield return new WaitForSeconds(1.2f);
        foreach (Button bt in BlockOnOutOfTime) bt.interactable = true;
    }


    //	public void ButtonFinishLevelClicked()
    //	{
    //		LevelTransition.Instance.HideSceneAndLoadNext("HomeScene"); 
    //	}


    public void ButtonRestartClicked()
    {

        StopAllCoroutines();

        BlockClicks.Instance.SetBlockAll(true);
        BlockClicks.Instance.SetBlockAllDelay(1f, false);

        //		GameData.IncrementButtonClickedCount();
        if (SoundManager.Instance != null) SoundManager.Instance.Stop_Sound(SoundManager.Instance.TimeCountdown);
        LevelTransition.Instance.HideSceneAndLoadNext(Application.loadedLevelName);
        SoundManager.Instance.Play_ButtonClick();

    }

    public void ButtonNextLevelClicked()
    {

        StopAllCoroutines();

        BlockClicks.Instance.SetBlockAll(true);
        BlockClicks.Instance.SetBlockAllDelay(1f, false);

        //		GameData.IncrementButtonClickedCount();
        if (SoundManager.Instance != null) SoundManager.Instance.Stop_Sound(SoundManager.Instance.TimeCountdown);
        LevelTransition.Instance.HideSceneAndLoadNext(Application.loadedLevelName);

        SoundManager.Instance.Play_ButtonClick();
    }


    public void ButtonPauseClicked()
    {
        menuManager.ShowPopUpMenu(PopUpPause);
        bPause = true;
        BlockClicks.Instance.SetBlockAll(true);
        BlockClicks.Instance.SetBlockAllDelay(1f, false);
        SoundManager.Instance.Play_ButtonClick();

    }

    public void ButtonPlayClicked()
    {
        menuManager.ClosePopUpMenu(PopUpPause);
        StartCoroutine("DelayPlay");
        BlockClicks.Instance.SetBlockAll(true);
        BlockClicks.Instance.SetBlockAllDelay(1f, false);
        //		GameData.IncrementButtonClickedCount();
        SoundManager.Instance.Play_ButtonClick();
    }

    IEnumerator DelayPlay()
    {
        yield return new WaitForSeconds(1f);
        if (MenuManager.activeMenu == "" && !bPopUpAutoHide) bPause = false;

    }


    public void ButtonHomeClicked()
    {

        StopAllCoroutines();

        BlockClicks.Instance.SetBlockAll(true);
        BlockClicks.Instance.SetBlockAllDelay(1f, false);

        //		GameData.IncrementButtonClickedCount();
        if (SoundManager.Instance != null) SoundManager.Instance.Stop_Sound(SoundManager.Instance.TimeCountdown);
        LevelTransition.Instance.HideSceneAndLoadNext("HomeScene");
        SoundManager.Instance.Play_ButtonClick();
    }





    //*******************************************



    public void ButtonBuyClicked()
    {
        int BrokenItemPrice = GameData.BrokenItemPriceFishingRod;
        if (GameData.GameType != "pecanje") BrokenItemPrice = GameData.BrokenItemPriceNet;

        if (GameData.TotalCoins >= BrokenItemPrice)
        {
            //Shop.Instance.txtDispalyStars =  TxtCoins;
            Shop.Instance.AnimiranjeDodavanjaZvezdica(-BrokenItemPrice, null, "");
            StartCoroutine(DelayCloseMenu(PopUpBrokenItem, 1));

            if (GameData.GameType == "pecanje")
                GameData.FishingRodsLeft = 3;
            else GameData.NetsLeft = 3;
            GameData.SetBrokenItems();
            BlockClicks.Instance.SetBlockAll(true);
            BlockClicks.Instance.SetBlockAllDelay(3f, false);
        }
        else
        {
            menuManager.ShowPopUpMessage("Not enough coins!", "watch video for one free game, or visit shop for more coins!");
            if (SoundManager.Instance != null) SoundManager.Instance.Stop_Sound(SoundManager.Instance.FaultNoCoins);
            BlockClicks.Instance.SetBlockAll(true);
            BlockClicks.Instance.SetBlockAllDelay(1, false);
        }


        SoundManager.Instance.Play_ButtonClick();
    }

    public void ButtonWatchVideoClicked()
    {
        FinishWatchingVideo();
        //AdMobAdManager.instance.ShowRewardedAd(() =>
        //{
        //    FinishWatchingVideo();
        //});

        //Sagar

        //AdsManager.Instance.IsVideoRewardAvailable ();


        //		if(GameData.sTestiranje.Contains("WatchVideo")) //TESTIRANJE
        //		{

        //			FinishWatchingVideo();
        //		}
        //		else
        //		{

        ////			AdsManager.Instance.IsVideoRewardAvailable ();


        //		}


        bPause = true;
        BlockClicks.Instance.SetBlockAll(true);
        BlockClicks.Instance.SetBlockAllDelay(1f, false);
        SoundManager.Instance.Play_ButtonClick();
    }

    public void ButtonShopClicked()
    {
        menuManager.ClosePopUpMenu(PopUpBrokenItem);

        BlockClicks.Instance.SetBlockAll(true);
        BlockClicks.Instance.SetBlockAllDelay(1f, false);
        StartCoroutine("DelayShowMenu", PopUpShop);
        SoundManager.Instance.Play_ButtonClick();
    }

    public void ButtonCloseShopClicked()
    {
        menuManager.ClosePopUpMenu(PopUpShop);
        bPause = true;
        BlockClicks.Instance.SetBlockAll(true);
        BlockClicks.Instance.SetBlockAllDelay(1f, false);
        StartCoroutine("DelayShowMenu", PopUpBrokenItem);
        SoundManager.Instance.Play_ButtonClick();
    }

    IEnumerator DelayShowMenu(GameObject menu)
    {
        yield return new WaitForSeconds(0.5f);
        menuManager.ShowPopUpMenu(menu);

    }


    IEnumerator DelayCloseMenu(GameObject menu, float timeW)
    {
        yield return new WaitForSeconds(timeW);
        menuManager.ClosePopUpMenu(menu);
        yield return new WaitForSeconds(.8f);
        if (MenuManager.activeMenu == "") bPause = false;
    }


    public void FinishWatchingVideoError()
    {
        menuManager.ShowPopUpMessage("Error", "Video not available");
        BlockClicks.Instance.SetBlockAll(true);
        BlockClicks.Instance.SetBlockAllDelay(1f, false);
    }


    public void FinishWatchingVideo()
    {
        //Debug.Log("GOTOV VIDEO");


        menuManager.ClosePopUpMenu(PopUpBrokenItem);
        StartCoroutine(DelayPlay());
        BlockClicks.Instance.SetBlockAll(true);
        BlockClicks.Instance.SetBlockAllDelay(1f, false);
    }


    public void SetWatchVideoButtons()
    {
        if (GameData.sTestiranje.Contains("WatchVideo")) //TESTIRANJE
        {
            ButtonWatchVideo.transform.GetChild(1).GetComponent<Text>().text = "one free";
            ButtonWatchVideo.interactable = true;
        }
        else
        {


        }
    }


    void OnApplicationFocus(bool hasFocus)
    {
        if (appFoucs && !hasFocus && (!bPause || (bPause && (MenuManager.activeMenu.StartsWith("PopUpChest") || MenuManager.activeMenu == "PopUpOctopusInk"))))
        // if( appFoucs && !hasFocus &&  !bPause ) 
        //if( !bButtonPlayInterstital && appFoucs && !hasFocus && ( !bPause || ( bPause &&    MenuManager.activeMenu == "" ) ) ) 
        {
            PopUpPause.SetActive(true);
            PopUpPauseAnim.Play("MainMenuEmpty");
            menuManager.ShowPopUpMenu(PopUpPause);
            bPause = true;
            BlockClicks.Instance.SetBlockAll(true);
            BlockClicks.Instance.SetBlockAllDelay(1f, false);
        }
        appFoucs = hasFocus;

    }

    //	public void Reward(){
    //		StartCoroutine( DelayCloseMenu( PopUpBrokenItem,1) );
    //
    //		if (GameData.GameType == "pecanje") {
    //			GameData.FishingRodsLeft = 3;
    //			//GameData.TotalCoins = GameData.BrokenItemPriceFishingRod;
    //
    //		} else if(GameData.GameType == "mreza" ) {
    //			GameData.NetsLeft = 3;
    //			//GameData.TotalCoins = GameData.BrokenItemPriceNet;
    //		}
    //		GameData.SetBrokenItems();
    //		BlockClicks.Instance.SetBlockAll(true);
    //		BlockClicks.Instance.SetBlockAllDelay(3f,false);
    //
    //		ButtonPlayClicked ();
    //
    //	}
}
