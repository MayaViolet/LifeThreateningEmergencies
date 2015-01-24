using UnityEngine;
using System.Collections;
using DaikonForge;
using DaikonForge.Tween;

public class GameCamera : MonoBehaviour {

	ExposureEffect exposure;

	void Start () {
		TransitionHandler.Instance.OnTransition += OnSceneTransition;
		//fade in
		exposure = GetComponent<ExposureEffect>();
		exposure.TweenProperty<float>("exposure")
			.SetStartValue(0)
			.SetEndValue(1)
			.SetDuration(0.5f)
			.SetEasing(TweenEasingFunctions.EaseOutQuad)
			.Play();
	}

	void FadeOut(float duration)
	{
		exposure.TweenProperty<float>("exposure")
			.SetStartValue(1)
			.SetEndValue(0)
			.SetDuration(duration)
			.SetEasing(TweenEasingFunctions.EaseInQuad)
			.Play();
	}

	void OnSceneTransition(float duration)
	{
		FadeOut(duration);
	}

}
