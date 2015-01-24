using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using DaikonForge;
using DaikonForge.Tween;

public class PlayerController : MonoBehaviour {

	public float moveSpeed = 3;
	public float maxY;

	Animator anim;
	TweenBase currentTween;

	void Start () {
		anim = GetComponentInChildren<Animator>();
		ClickHandler.Instance.OnMovementClick += OnMovementClick;
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.magenta;
		Gizmos.DrawLine (new Vector3 (-10, maxY, 0), new Vector3 (10, maxY, 0));
	}

	void OnMovementClick(Vector3 location)
	{
		MoveTo (location);
	}

	public void MoveTo(Vector3 location, Action onComplete = null)
	{
		location = new Vector3 (location.x, Math.Min (location.y, maxY), transform.position.z);

		//raycast to obstacles
		{
			Vector3 moveOffset = location - transform.position;
			Ray moveRay = new Ray(transform.position, moveOffset.normalized);
			RaycastHit hit;
			if (Physics.Raycast(moveRay, out hit, moveOffset.magnitude, 1 << LayerMask.NameToLayer("Obstacles")))
			{
				location = hit.point - moveRay.direction * 0.1f;
			}
		}

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

	void NopeOnTriggerEnter(Collider other)
	{
		print ("CollisionEnter");
		if (other.gameObject.layer == LayerMask.NameToLayer("Default"))
		{
			anim.SetBool ("Walking", false);
			if (currentTween != null)
			{
				currentTween.Stop();
			}
		}
	}
}