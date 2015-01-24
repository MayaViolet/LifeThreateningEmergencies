using UnityEngine;
using System.Collections;

public class ClickHandler : MonoBehaviour {

	static ClickHandler _instance;
	public static ClickHandler Instance
	{
		get
		{
			if (_instance == null)
			{
				new GameObject("ClickHandler", typeof(ClickHandler));
			}
			return _instance;
		}
	}

	public delegate void ClickReceiver(Vector3 position);
	public event ClickReceiver OnMovementClick;

	void Awake()
	{
		if (_instance != null)
		{
			Destroy(gameObject);
			return;
		}
		_instance = this;
	}

	public void CallMovementClick(Vector3 position)
	{
		if (OnMovementClick != null)
		{
			OnMovementClick(position);
		}
	}
}
