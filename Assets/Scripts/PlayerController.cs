using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using DaikonForge;
using DaikonForge.Tween;

public class PlayerController : MonoBehaviour {

	public float moveSpeed = 3;

	Animator anim;
	TweenBase currentTween;

	void Start () {
		anim = GetComponentInChildren<Animator>();
		ClickHandler.Instance.OnMovementClick += OnMovementClick;
	}

	void OnMovementClick(Vector3 location)
	{
		MoveTo (location);
	}

	public void MoveTo(Vector3 location, Action onComplete = null)
	{
		Vector3 offset = location - transform.position;
		float distance = offset.magnitude;
		float time = distance / moveSpeed;

		//face movement direction
		Vector3 scale = transform.localScale;
		if (offset.x > 0)
		{
			scale.x = -Mathf.Abs(scale.x);
		}
		else
		{
			scale.x = Mathf.Abs(scale.x);
		}
		transform.localScale = scale;

		if (currentTween != null)
		{
			currentTween.Stop();
		}

		anim.SetBool("Walking", true);
		currentTween = transform.TweenMoveTo(location)
			.SetDuration(time)
			.OnCompleted(tween => {
					anim.SetBool ("Walking", false);
					if (onComplete != null) {
						onComplete();
					}
			})
			.Play();
	}

	void OnCollisionEnter(Collision other)
	{
		print ("CollisionEnter");
		//if (other.gameObject.layer == LayerMask.NameToLayer("Default"))
		{
			anim.SetBool ("Walking", false);
			if (currentTween != null)
			{
				currentTween.Stop();
			}
		}
	}
}