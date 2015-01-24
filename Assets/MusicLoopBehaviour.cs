﻿using UnityEngine;
using System.Collections;

public class MusicLoopBehaviour : MonoBehaviour {
	public AudioSource AudioSource;
	public AudioClip[] Clips;
	private int _clipCount;
	private int _clipIndex;
	private float _wait;

	void Start () {
		_clipCount = Clips.Length;
		PlayClip (0);
	}

	private void PlayClip(int index) {
		_clipIndex = index;
		AudioSource.clip = Clips [index];
		_wait = Clips [index].length;
		AudioSource.Play ();
	}
	
	// Update is called once per frame
	void Update () {
		_wait -= Time.deltaTime;
		if (_wait < 0) {
			PlayClip ((_clipIndex + 1) % _clipCount);
		}
	}
}
