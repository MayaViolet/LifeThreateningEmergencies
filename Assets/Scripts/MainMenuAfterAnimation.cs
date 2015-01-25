using UnityEngine;
using System.Collections;

public class MainMenuAfterAnimation : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		if (!animation.isPlaying) {
			TransitionHandler.Instance.TransitionTo("Title");
		}
	}
}
