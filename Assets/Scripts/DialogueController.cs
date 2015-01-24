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
	public DialoguePart.Iterator dialogueIterator;

	Vector3 showPosition;
	Vector3 hidePosition;

	private bool _visible;
	private List<Button> _menuButtons;
	
	public bool visible
	{
		get {
			return _visible;
		}

		set
		{
			_visible = value;

			if (value)
			{
				dialogueBase.TweenMoveTo(showPosition, true)
					.SetDuration(0.5f)
					.SetEasing(TweenEasingFunctions.EaseOutBack)
					.Play();
			}
			else
			{
				dialogueBase.TweenMoveTo(hidePosition, true)
					.SetDuration(0.5f)
					.SetEasing(TweenEasingFunctions.EaseInBack)
					.Play();
			}
		}
	}

	void Start()
	{
		showPosition = dialogueBase.localPosition;
		hidePosition = showPosition + dialogueBase.rect.height * 2 * Vector3.down;
		dialogueBase.localPosition = hidePosition;
	}

	public void BeginDialogue(Dialogue dialogue) {
		dialogueIterator = dialogue.Start ();
		ShowLine (dialogueIterator.CurrentLine);
	}

	public void AdvanceDialogue() {
		if (dialogueIterator.Next ()) {
			ShowLine (dialogueIterator.CurrentLine);
			return;
		}

		// Check for menu display, otherwise we're done.
		var menu = dialogueIterator.DialoguePart.Menu;
		if (menu == null) {
			visible = false;
			return;
		}

		if (_menuButtons != null) {
			// Don't double-show menu.
			return;
		}

		_menuButtons = new List<Button>();

		var cumulativeHeight = 0f;
		foreach (var choice in menu.Choices) {
			var button = (Button)Instantiate (menuButton);
			button.transform.SetParent (this.transform, false);
			button.transform.localPosition = new Vector3 (0, cumulativeHeight, 0);

			button.GetComponentInChildren<Text> ().text = choice.Text;

			var selectedChoice = choice;
			button.onClick.AddListener (() => MenuChoiceSelected(selectedChoice));

			cumulativeHeight += button.GetComponent<RectTransform> ().rect.height;
			_menuButtons.Add (button);

		}
	}

	private void MenuChoiceSelected(Menu.Choice choice) {
		Debug.Log (string.Format ("Choice was selected: {0}", choice.Text));

		foreach (var button in _menuButtons) {
			Destroy (button.gameObject);
		}

		_menuButtons = null;

		dialogueIterator = choice.JumpTarget.Start ();
		ShowLine (dialogueIterator.CurrentLine);
	}
	
	private void ShowLine(Line newLine)
	{
		dialogueText.text = newLine.Text;
		visible = true;
	}
}
