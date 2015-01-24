using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using BitterEnd;

public class DialogueInteraction : MonoBehaviour, IPointerClickHandler {

	public Dialogue dialogue;
	DialogueController controller;

	void Start()
	{
		GameObject controllerGO = GameObject.FindGameObjectWithTag("DialogueController");
		controller = controllerGO.GetComponent<DialogueController>();
	}

	void IPointerClickHandler.OnPointerClick (PointerEventData eventData)
	{

	}

}
