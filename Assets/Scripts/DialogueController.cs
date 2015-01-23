﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DaikonForge;
using DaikonForge.Tween;
using BitterEnd;

public class DialogueController : MonoBehaviour {

	public Text dialogueText;
	public Image portraitImage;
	public RectTransform dialogueBase;
	public Dialogue.Iterator dialogueIterator;

	Vector3 showPosition;
	Vector3 hidePosition;

	public bool visible
	{
		set
		{
			if (value)
			{
				dialogueBase.TweenMoveTo(showPosition)
					.SetDuration(0.5f)
					.SetEasing(TweenEasingFunctions.EaseOutBack)
					.Play();
			}
			else
			{
				dialogueBase.TweenMoveTo(hidePosition)
					.SetDuration(0.5f)
					.SetEasing(TweenEasingFunctions.EaseInBack)
					.Play();
			}
		}
	}

	void Start()
	{
		showPosition = dialogueBase.position;
		hidePosition = showPosition + dialogueBase.rect.height * 2 * Vector3.down;
		dialogueBase.position = hidePosition;

		dialogueIterator = Dialogue.GetTestDialogue ().Start ();
		ShowLine (dialogueIterator.CurrentLine);
	}

	public void AdvanceDialogue() {
		if (!dialogueIterator.Next ()) {
			visible = false;
			return;
		}

		ShowLine (dialogueIterator.CurrentLine);
	}

	private void ShowLine(Line newLine)
	{
		dialogueText.text = newLine.Text;
		visible = true;
	}
}
