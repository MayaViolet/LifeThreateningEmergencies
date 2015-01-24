using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class AnimationInteraction : MonoBehaviour, IPointerClickHandler {

	void IPointerClickHandler.OnPointerClick (PointerEventData eventData)
	{
		animation.Play();
	}

}
