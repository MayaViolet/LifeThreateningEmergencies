using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MusicLoopBehaviour : MonoBehaviour
{
	public AudioClip[] Clips;
	private AudioSource _audioSource;
	private int _clipCount;
	private int _clipIndex;
	private float _wait;
	private static MusicLoopBehaviour _instance;

	void Awake ()
	{
		if (_instance != null) {
			// TODO: replace _instance's Clips array with this's?
			Destroy (gameObject);
			return;
		}

		_instance = this;
		DontDestroyOnLoad (this);
	}

	void Start ()
	{
		_audioSource = GetComponent<AudioSource> ();
		_clipCount = Clips.Length;
		PlayClip (0);
	}

	private void PlayClip (int index)
	{
		_clipIndex = index;
		_audioSource.clip = Clips [index];
		_wait = Clips [index].length;
		_audioSource.Play ();
	}

	void Update ()
	{
		_wait -= Time.deltaTime;

		if (_wait < 0) {
			int next = _clipIndex;
			while (next == _clipIndex) {
				next = Random.Range (0, _clipCount);
			}
			PlayClip (next);
		}
	}
}
