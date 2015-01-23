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

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Ray clickRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;
			if (Physics.Raycast(clickRay, out hitInfo, 100))
			{
				if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Walkable"))
				{
					if (OnMovementClick != null)
					{
						OnMovementClick(hitInfo.point);
					}
				}
			}
		}
	}
}
