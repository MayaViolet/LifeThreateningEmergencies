using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using BitterEnd;

public class DialogueInteraction : MonoBehaviour, IPointerClickHandler {
	public string dialogue;
	DialogueController controller;

	void Start()
	{
		GameObject controllerGO = GameObject.FindGameObjectWithTag("DialogueController");
		controller = controllerGO.GetComponent<DialogueController>();
	}

	private const float MAXIMUM_DISTANCE = 2f;

	void IPointerClickHandler.OnPointerClick (PointerEventData eventData)
	{
		var playerController = GameObject.FindObjectOfType<PlayerController> ();
		var from = playerController.transform.position;
		var to = eventData.worldPosition;
		to = new Vector3 (to.x, from.y, to.z);

   		var diff = to - from;
		if (diff.magnitude > MAXIMUM_DISTANCE) {
			playerController.MoveTo(from + diff.normalized * (diff.magnitude - MAXIMUM_DISTANCE));
		}

		controller.BeginDialogue(RenPyParser.ReadDialogueFromResources(dialogue));
	}
}