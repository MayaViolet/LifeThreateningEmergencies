using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using BitterEnd;

abstract public class AbstractInteraction : MonoBehaviour, IPointerClickHandler {
	public string dialogue;
	protected DialogueController _dialogueController;
	protected PlayerController _playerController;
	
	void Start()
	{
		var dialogueControllerGO = GameObject.FindGameObjectWithTag("DialogueController");
		_dialogueController = dialogueControllerGO.GetComponent<DialogueController>();
		
		var playerControllerGO = GameObject.FindGameObjectWithTag("PlayerController");
		_playerController = playerControllerGO.GetComponent<PlayerController>();
	}
	
	private const float MAXIMUM_DISTANCE = 2f;
	
	void IPointerClickHandler.OnPointerClick (PointerEventData eventData)
	{
		var from = _playerController.transform.position;
		var to = eventData.worldPosition;
		to = new Vector3 (to.x, from.y, to.z);
		
		var diff = to - from;	
		if (diff.magnitude > MAXIMUM_DISTANCE) {
			_playerController.MoveTo (from + diff.normalized * (diff.magnitude - MAXIMUM_DISTANCE), PerformInteraction);
		} else {
			PerformInteraction ();
		}
	}
	
	protected abstract void PerformInteraction ();
}