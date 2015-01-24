using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour {

	static MusicPlayer _instance;

	public float fadeTime;
	float fadeTimer;
	float startVolume;
	AudioClip nextTrack;

	void Awake() {
		if (_instance)
		{
			Destroy(gameObject);
			return;
		}
		_instance = this;
		DontDestroyOnLoad(gameObject);
		startVolume = audio.volume;
	}

	void Update () {
		if (nextTrack != null)
		{
			fadeTimer -= Time.deltaTime;
			float volumeMultiplier = fadeTimer / fadeTime;
			audio.volume = volumeMultiplier * startVolume;
			if (fadeTimer <= 0)
			{
				audio.clip = nextTrack;
				audio.Play();
				nextTrack = null;
				audio.volume = startVolume;
			}
		}
	}

	public static void PlayTrack(AudioClip track)
	{
		_instance.nextTrack = track;
		_instance.fadeTimer = _instance.fadeTime;
	}
}
