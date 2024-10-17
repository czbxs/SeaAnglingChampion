using UnityEngine;
using System.Collections;

public class AnimationEvents : MonoBehaviour {

	 
	public void StartParticles()
	{
		//transform.GetComponentInChildren<ParticleSystem>().Play();
	}
		
 
	public void CarouselSelectedAnimationOver( )
	{
		//Debug.Log("CarouselSelectedAnimationOver");
		//HomeScene.Instance.CarouselSelected();
	}

	public void CarouselSelectRoomShowAnimationEnded( )
	{
		//Debug.Log("CarouselSelectRoomShowAnimationEnded");
		 //transform.FindChild("Carousel").GetComponent<ItemsSlider>().Init();
		 
	}

	public void CarouselSelectRoomHideAnimationEnded( )
	{
		//Debug.Log("CarouselSelectRoomHideAnimationEnded");
		// HomeScene.Instance.CarouselSelected();
	}
}
