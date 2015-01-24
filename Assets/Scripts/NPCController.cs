using UnityEngine;
using System.Collections;
using BitterEnd;

public class NPCController : MonoBehaviour {

	public string characterName;

	SpriteRenderer sprite;
	SpriteRenderer child;

	Transform player;

	void Start () {
		sprite  = renderer as SpriteRenderer;
		child = transform.GetChild(0).renderer as SpriteRenderer;
		GameObject playerGO = GameObject.FindGameObjectWithTag("PlayerController");
		player = playerGO.transform;

		GameObject dialogueGO = GameObject.FindGameObjectWithTag("DialogueController");
		DialogueController dialogue = dialogueGO.GetComponent<DialogueController>();
		dialogue.OnDisplayedLine += OnDialogue;
	}

	void LateUpdate () {
		child.sortingOrder = sprite.sortingOrder+1;
	}

	void OnDialogue(DialogueLine nextLine)
	{
		if (nextLine.Character.Name == characterName)
		{
			animation.Play();
		}

		Vector3 scale = transform.localScale;
		if (player.position.x < transform.position.x)
		{
			scale.x = Mathf.Abs(scale.x);
		}
		else
		{
			scale.x = -Mathf.Abs(scale.x);
		}
		transform.localScale = scale;
	}
}
