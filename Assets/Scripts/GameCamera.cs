using UnityEngine;
using System.Collections;
using DaikonForge;
using DaikonForge.Tween;

public class GameCamera : MonoBehaviour {

	public Transform target;
	public Vector3 offset;
	public SpriteRenderer background;
	public float deadZone = 0.5f;
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

	void LateUpdate()
	{
		if (target == null)
		{
			return;
		}
		Vector2 targetDirection = target.position + offset - transform.position;
		if (targetDirection.magnitude > deadZone)
		{
			Vector3 position = transform.position;
			targetDirection.Normalize();
			Vector3 newPosition = (target.position + offset) - (Vector3)(targetDirection * deadZone);
			newPosition.z = position.z;
			transform.position = newPosition;
		}
		if (background != null)
		{
			ConstrainToBackground();
		}
	}

	void ConstrainToBackground()
	{
		float camWidth = Screen.width / (float)Screen.height;
		camWidth *= camera.orthographicSize;

		Bounds spriteRect = background.sprite.bounds;
		float maxX = spriteRect.size.x/2 - camWidth;
		float maxY = spriteRect.size.y/2 - camera.orthographicSize;

		Vector3 position = transform.position;
		position.x = Mathf.Clamp(position.x, -maxX, maxX);
		position.y = Mathf.Clamp(position.y, -maxY, maxY);
		transform.position = position;
	}

}
