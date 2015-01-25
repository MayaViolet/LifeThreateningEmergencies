using UnityEngine;
using System.Collections;

public class MainMenuAfterAnimation : MonoBehaviour {

	public string DestinationScene = "Title";

	// Update is called once per frame
	void Update () {
		if (!animation.isPlaying) {
			TransitionHandler.Instance.TransitionTo(DestinationScene);
		}
	}
}
