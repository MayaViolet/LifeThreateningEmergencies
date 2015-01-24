using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DaikonForge;
using DaikonForge.Tween;
using BitterEnd;

public class DialogueController : MonoBehaviour {

	public Text dialogueText;
	public Image portraitImage;
	public RectTransform dialogueBase;
	public Button menuButton;
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
			// Check for menu display, otherwise we're done.
			var menu = dialogueIterator.CurrentPart.Menu;
			if (menu != null) {
				var buttons = new List<Button>();
				var cumulativeHeight = 0f;
				foreach (var choice in menu.Choices) {
					var button = (Button) Instantiate (menuButton);
					button.transform.SetParent (this.transform, false);
					button.transform.position += new Vector3(0, cumulativeHeight, 0);

					button.GetComponentInChildren<Text>().text = choice.Text;
					var choiceText = choice.Text;

					button.onClick.AddListener(() => {
						Debug.Log (string.Format ("Choice was selected: {0}", choiceText));
					});

					cumulativeHeight += button.GetComponent<RectTransform>().rect.height;
					buttons.Add (button);
				}

				return;
			}

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
