using UnityEngine;
using System.Collections;

public class SceneMusic : MonoBehaviour {

	public AudioClip track;

	// Use this for initialization
	void Start () {
		MusicPlayer.PlayTrack(track);
	}
}
