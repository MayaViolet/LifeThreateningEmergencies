using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class MovableArea : MonoBehaviour, IPointerClickHandler {

	public virtual void OnPointerClick (PointerEventData eventData)
	{
		ClickHandler.Instance.CallMovementClick(eventData.worldPosition);
	}
}
