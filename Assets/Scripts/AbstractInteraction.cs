using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using BitterEnd;

abstract public class AbstractInteraction : MonoBehaviour, IPointerClickHandler
{
		protected DialogueController _dialogueController;
		protected PlayerController _playerController;
	
		void Start ()
		{
				var dialogueControllerGO = GameObject.FindGameObjectWithTag ("DialogueController");
				_dialogueController = dialogueControllerGO.GetComponent<DialogueController> ();
		
				var playerControllerGO = GameObject.FindGameObjectWithTag ("PlayerController");
				_playerController = playerControllerGO.GetComponent<PlayerController> ();
		}

		void IPointerClickHandler.OnPointerClick (PointerEventData eventData)
		{
				var from = _playerController.transform.position;
				var to = eventData.worldPosition;
				to = new Vector3 (to.x, from.y, to.z);
		
				var diff = to - from;
				var maximumDistance = GetMaximumDistance ();
				if (diff.magnitude > maximumDistance) {
						_playerController.MoveTo (from + diff.normalized * (diff.magnitude - maximumDistance), PerformInteraction);
				} else {
						PerformInteraction ();
				}
		}
	
		protected abstract void PerformInteraction ();

		protected abstract float GetMaximumDistance ();
}