using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using BitterEnd;

public class DialogueInteraction : AbstractInteraction
{
		public string dialogue;

		protected override void PerformInteraction ()
		{
				_dialogueController.BeginDialogue (RenPyParser.ReadDialogueFromResources (dialogue), gameObject);
		}

		protected override float GetMaximumDistance ()
		{
				return 2f;
		}
}