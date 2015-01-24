using UnityEngine;
using System.Collections;

public class TransitionHandler : MonoBehaviour {

	static TransitionHandler _instance;
	public static TransitionHandler Instance
	{
		get
		{
			if (_instance == null)
			{
				new GameObject("TransitionHandler", typeof(TransitionHandler));
			}
			return _instance;
		}
	}

	public delegate void TransitionListener(float duration);
	public event TransitionListener OnTransition;

	const float TRANSITION_TIME = 0.5f;

	void Awake()
	{
		if (_instance != null)
		{
			Destroy(gameObject);
			return;
		}
		_instance = this;
	}

	public void TransitionTo(string sceneName)
	{
		StartCoroutine(TransitionRoutine(sceneName));
	}

	IEnumerator TransitionRoutine(string nextScene)
	{
		if (OnTransition != null)
		{
			OnTransition(TRANSITION_TIME);
		}
		yield return new WaitForSeconds(TRANSITION_TIME);
		Application.LoadLevel(nextScene);
	}
}
