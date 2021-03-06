﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MusicLoopBehaviour : MonoBehaviour
{
	public AudioClip[] Clips;
	private AudioSource _audioSource;
	private int _clipIndex;
	private float _wait;
	private static MusicLoopBehaviour _instance;

	void Awake ()
	{
		if (_instance != null) {
			_instance.Clips = Clips;
			_instance.NextClip();
			Destroy (gameObject);
			return;
		}

		_instance = this;
		DontDestroyOnLoad (this);
	}

	void Start ()
	{
		_audioSource = GetComponent<AudioSource> ();
		PlayClip (0);
	}

	private void PlayClip (int index)
	{
		_clipIndex = index;
		_audioSource.clip = Clips [index];
		_wait = Clips [index].length;
		_audioSource.Play ();
	}

	private void NextClip()
	{
		_wait = 0f;
	}

	void Update ()
	{
		_wait -= Time.deltaTime;

		if (_wait < 0f) {
			int next = _clipIndex;
			if (Clips.Length <= 1) {
				PlayClip(0);
				return;
			}

			while (next == _clipIndex) {
				next = Random.Range (0, Clips.Length);
			}
			PlayClip (next);
		}
	}
}
