using UnityEngine;
using System;
using System.Collections;

public class BindVisibilityToValueBehaviour : MonoBehaviour {
	public enum VisibleWhen	{
		True,
		False
	};
	
	public VisibleWhen VisibleWhenFlagIs = VisibleWhen.False;
	public string Flag;
	private Action<bool> _callback;
	
	void Start () {
		SetVisibility (VisibleWhenFlagIs == VisibleWhen.False);

		_callback = value => {
			SetVisibility ((VisibleWhenFlagIs == VisibleWhen.False) ? !value : value);
		};
		ValueStore.OnValueChanged (Flag, _callback);
	}

	void OnDestroy() {
		ValueStore.RemoveValueChanged (Flag, _callback);
	}

	private void SetVisibility(bool value) {
		gameObject.GetComponent<SpriteRenderer> ().enabled = value;
	}
}
