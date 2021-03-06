using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using DaikonForge;
using DaikonForge.Tween;
using BitterEnd;

[RequireComponent(typeof(AudioSource))]
public class DialogueController : MonoBehaviour
{
	public enum CharacterChoice { Enok, Nalini };
	public CharacterChoice Character = CharacterChoice.Enok;

	public Text dialogueText;
	public Text dialogueTitle;
	public Image portraitImage;
	public RectTransform dialogueBase;
	public Button menuButton;
	public string enterDialogue;
	public DialoguePart.Iterator dialogueIterator;

	Vector3 showPosition;
	Vector3 hidePosition;
	private bool _visible;
	private List<Button> _menuButtons;

	public event Action<DialogueLine> OnDisplayedLine;

	public bool visible {
		get {
			return _visible;
		}

		set {
			if (_visible == value) {
				return;
			}

			_visible = value;

			if (value) {
				dialogueBase.TweenMoveTo (showPosition, true)
					.SetDuration (0.5f)
					.SetEasing (TweenEasingFunctions.EaseOutBack)
					.Play ();
			} else {
				dialogueBase.TweenMoveTo (hidePosition, true)
					.SetDuration (0.5f)
					.SetEasing (TweenEasingFunctions.EaseInBack)
					.Play ();
			}

			Camera.main.GetComponent<PhysicsRaycaster>().enabled = !value;
		}
	}

	void Start ()
	{
		showPosition = dialogueBase.localPosition;
		hidePosition = showPosition + dialogueBase.rect.height * 2 * Vector3.down;
		dialogueBase.localPosition = hidePosition;

		if (!string.IsNullOrEmpty(enterDialogue)) {
			BeginDialogue (RenPyParser.ReadDialogueFromResources(Character, enterDialogue), gameObject);
		}
	}

	void Update ()
	{
		if (!_visible || _menuButtons != null) {
			return;
		}

		if (Input.GetKeyDown (KeyCode.Space) || Input.GetMouseButtonDown (0)) {
			AdvanceDialogue ();
		}
	}

	public void BeginDialogue (Dialogue dialogue, GameObject hostGO)
	{
		dialogueIterator = dialogue.Start (hostGO);
		if (!dialogueIterator.HasCurrent) {
			return;
		}

		ShowCurrent ();
	}

	public void AdvanceDialogue ()
	{
		if (_menuButtons != null) {
			return;
		}

		if (!dialogueIterator.Next ()) {
			visible = false;
			return;
		}

		ShowCurrent ();
	}

	private void ShowCurrent() {
		var element = dialogueIterator.CurrentElement;
		
		var line = element as DialogueLine;
		if (line != null) {
			ShowLine (line);
			return;
		}
		
		var menu = element as DialogueMenu;
		if (menu != null) {
			ShowMenu (menu);
			return;
		}

		var transition = element as DialogueTransition;
		if (transition != null) {
			TransitionHandler.Instance.TransitionTo (transition.SceneId);
			return;
		}

		var fade = element as DialogueFade;
		if (fade != null) {
			var gameCamera = Camera.main.GetComponent<GameCamera>();

			TweenBase tween;
			if (fade.Mode == DialogueFade.FadeMode.IN) {
				tween = gameCamera.FadeIn (1f);
			} else {
				tween = gameCamera.FadeOut (1f);
				visible = false;
			}

			tween.OnCompleted(sender => AdvanceDialogue());
			return;
		}

		var wait = element as DialogueWait;
		if (wait != null) {
			Invoke ("AdvanceDialogue", wait.Time);
			return;
		}

		var moveTo = element as DialogueMoveTo;
		if (moveTo != null) {
			var destination = GameObject.FindGameObjectWithTag(moveTo.TargetGO);
			GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>().MoveTo(destination.transform.position);
			AdvanceDialogue();
			return;
		}
		
		throw new FormatException (string.Format ("Couldn't show a {0}.", element));
	}

	private void ShowLine (DialogueLine newLine)
	{
		if (OnDisplayedLine != null) {
			OnDisplayedLine (newLine);
		}

		dialogueText.text = newLine.Text;
		
		string portraitId = null;
		var character = newLine.Character;
		
		if (character != null) {
			portraitId = newLine.Character.PortraitId;
			dialogueTitle.text = character.Friendly;
		} else {
			dialogueTitle.text = "";
		}
		
		if (portraitId != null) {
			var texture = Resources.Load<Sprite> (string.Format ("Portraits/{0}", portraitId));
			if (texture == null) {
				Debug.LogError (string.Format ("Portrait '{0}' not found", portraitId));
				portraitImage.enabled = false;
			} else {
				portraitImage.sprite = texture;
				portraitImage.enabled = true;
			}
		} else {
			portraitImage.enabled = false;
		}
		
		visible = true;
	}

	private void ShowMenu(DialogueMenu menu) {
		// Check for menu display, otherwise we're done.
		if (_menuButtons != null) {
			// Don't double-show menu.
			return;
		}

		_menuButtons = new List<Button> ();

		var cumulativeHeight = 0f;
		var cumulativeDelay = 0f;
		foreach (var choice in menu.Choices) {
			var button = (Button)Instantiate (menuButton);
			button.transform.SetParent (this.transform, false);
			button.GetComponentInChildren<Text> ().text = choice.Text;
			//animate buttons appearing
			button.TweenScaleFrom(Vector3.zero)
				.SetDelay(cumulativeDelay)
				.SetDuration(0.25f)
				.SetEasing(TweenEasingFunctions.EaseOutQuad)
				.Play();
			cumulativeDelay += 0.125f;

			var selectedChoice = choice;
			button.onClick.AddListener (() => MenuChoiceSelected (selectedChoice));

			cumulativeHeight += button.GetComponent<RectTransform> ().rect.height;
			_menuButtons.Add (button);
		}

		var top = cumulativeHeight / 2f;
		foreach (var button in _menuButtons) {
			button.transform.localPosition = new Vector3 (0, top, 0);
			top -= button.GetComponent<RectTransform> ().rect.height;
		}
	}

	private void MenuChoiceSelected (DialogueMenu.Choice choice)
	{
		foreach (var button in _menuButtons) {
			var otherButton = button;
			button.TweenScaleTo(Vector3.zero)
				.SetDuration(0.25f)
				.SetEasing(TweenEasingFunctions.EaseInQuad)
				.OnCompleted((TweenBase tween) => Destroy(otherButton))
				.Play();
		}

		_menuButtons = null;

		if (choice.DialogueJump == null) {
			visible = false;
			return;
		}

		dialogueIterator = choice.DialogueJump.Target.Start (dialogueIterator.HostGO);
		ShowCurrent ();
	}
}
