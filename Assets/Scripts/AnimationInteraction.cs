using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class AnimationInteraction : MonoBehaviour, IPointerClickHandler {

	public bool singleUse;

	void IPointerClickHandler.OnPointerClick (PointerEventData eventData)
	{
		animation.Play();
		if (singleUse)
		{
			enabled = false;
		}
	}

}
