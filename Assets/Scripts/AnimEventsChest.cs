using UnityEngine;
using System.Collections;

public class AnimEventsChest : MonoBehaviour {

	public Transform Coin1;
	public Transform Coin2;
	public Transform Coin3;

	public Transform Coin1a;
	public Transform Coin2a;
	public Transform Coin3a;

	public Transform CoinsDestination;
	Vector3 lp1  ;
	Vector3 lp2 ;
	Vector3 lp3  ;

	public Animator Pingvin;

	public void AnimEvent_CollectCoins()
	{
		 
		 lp1 = Coin1.position;
		 lp2 = Coin2.position;
		 lp3 = Coin3.position;

		Coin1a.gameObject.SetActive( true);
		Coin2a.gameObject.SetActive( true);
		Coin3a.gameObject.SetActive( true);
 
		Coin1a.position = Coin1.position;
		Coin2a.position = Coin2.position;
		Coin3a.position = Coin3.position;
	 
		StartCoroutine("MoveToDestination");
	}


	public void AnimEvent_EndCollectCoins()
	{
		 
		StopCoroutine("MoveToDestination");
	}

	IEnumerator MoveToDestination()
	{
		float moveTime = 0;
		Vector3 endPos = CoinsDestination.position;

		while(moveTime<2f)
		{
		 
			Coin1a.position =  Vector3.Lerp(lp1, endPos, moveTime);
			Coin2a.position = Vector3.Lerp(lp2, endPos, moveTime - 0.6f);
			Coin3a.position = Vector3.Lerp(lp3, endPos, moveTime - 0.8f);
			yield return new WaitForEndOfFrame( );
			moveTime +=Time.deltaTime*1.5f;
			if( Coin1a.gameObject.activeSelf &&  moveTime > 1.0f ) 
			{ 
				Coin1a.gameObject.SetActive( false); 
				//if(SoundManager.Instance !=null ) SoundManager.Instance.Play_Sound(SoundManager.Instance.ChestPopupCoins);//Correct
				GameData.CollectedCoins +=5;
				Gameplay.Instance.UpdateCoins();
			}
			else if(Coin2a.gameObject.activeSelf && moveTime > 1.6f) 
			{ 
				GameData.CollectedCoins +=5;
				Gameplay.Instance.UpdateCoins();
				Coin2a.gameObject.SetActive( false); 
				//if(SoundManager.Instance !=null ) SoundManager.Instance.Play_Sound(SoundManager.Instance.Correct);
			}
			else if(Coin3a.gameObject.activeSelf && moveTime > 1.8f) 
			{
				GameData.CollectedCoins +=5;
				Gameplay.Instance.UpdateCoins();
				Coin3a.gameObject.SetActive( false);
				//if(SoundManager.Instance !=null ) SoundManager.Instance.Play_Sound(SoundManager.Instance.Correct);
			}
		}

		yield return new WaitForEndOfFrame();

		//Coin1a.gameObject.SetActive( false);
		//Coin2a.gameObject.SetActive( false);
		//Coin3a.gameObject.SetActive( false);
	}

	public void AnimEvent_CollectTime()
	{
		GameData.TimeLeft+=15;
	}

	public void AnimEvent_CollectEmpty()
	{
		if(SoundManager.Instance !=null ) SoundManager.Instance.Play_Sound(SoundManager.Instance.ChestPopupEmpty);
		Pingvin.SetTrigger("tSad");
	}

	public void AnimEvent_PengunHappy()
	{
		if(Gameplay.Instance.chestReward == "time" && SoundManager.Instance !=null ) SoundManager.Instance.Play_Sound(SoundManager.Instance.ChestPopupTime);
		else if(Gameplay.Instance.chestReward == "gold" && SoundManager.Instance !=null ) SoundManager.Instance.Play_Sound(SoundManager.Instance.ChestPopupCoins); 
 
		Pingvin.SetTrigger("tHappy");
	}

	public void AnimEvent_PengunIdle()
	{
		Pingvin.SetTrigger("tIdle");
	}

}
