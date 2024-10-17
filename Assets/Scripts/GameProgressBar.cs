using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameProgressBar : MonoBehaviour {
 
	public Image ProgressFill;
	public int TotalSteps = 0;
	//public int CompletedSteps = 0;
	float step; 
	Vector2 hidePos = new Vector2(0,80);
	//TODO: podesiti sa promenom pozicije bannera
	Vector2  endPosition = new Vector2(0,-226);
	RectTransform recTr;


	void Start()
	{
		ProgressFill.fillAmount = 0;


		recTr = transform.GetComponent<RectTransform>();
		recTr.position = hidePos;


	}

	public void ShowProgressBar () {

		ProgressFill.fillAmount = 0;
		step = 1/((float)TotalSteps);

		recTr.anchoredPosition =hidePos;
		StartCoroutine("MoveDown");
	}

	public void HideProgressBar () {

		//ProgressFill.fillAmount = 1;
		//step = 1/(10f*TotalSteps);

		//recTr.anchoredPosition =hidePos;
		StartCoroutine("MoveUp");
	}

	IEnumerator MoveDown()
	{
		float t = 0;
		while(t<1)
		{
			t +=Time.deltaTime;
			recTr.anchoredPosition = Vector2.Lerp(recTr.anchoredPosition, endPosition, t);
			yield return new WaitForEndOfFrame();
		}
		recTr.anchoredPosition = endPosition;
	}

	IEnumerator MoveUp()
	{
		float t = 0;
		while(t<1)
		{
			t +=Time.deltaTime;
			recTr.anchoredPosition = Vector2.Lerp(recTr.anchoredPosition, hidePos, t);
			yield return new WaitForEndOfFrame();
		}
		recTr.anchoredPosition = hidePos;
	}

 
	public void IncreaseBar()
	{
		//CompletedSteps++;
		StartCoroutine("Incrising");
	}
	 
	IEnumerator Incrising()
	{
		
		float pom = 0;
		float pom2 = 0;
		while(pom <= step)
		{
			pom2 = (step*Time.deltaTime*2 );
			ProgressFill.fillAmount +=pom2;
			pom +=pom2;
			yield return new WaitForEndOfFrame();
		}
 
	}
}
