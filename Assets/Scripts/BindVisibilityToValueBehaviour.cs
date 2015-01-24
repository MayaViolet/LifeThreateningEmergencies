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
	public bool UseAnimationIfAvailable = true;
	private Action<bool> _callback;
	
	void Start () {
		var val = ValueStore.Retrieve (Flag);
		SetVisibility ((VisibleWhenFlagIs == VisibleWhen.False) ? !val : val, true);

		_callback = value => {
			SetVisibility ((VisibleWhenFlagIs == VisibleWhen.False) ? !value : value);
		};
		ValueStore.OnValueChanged (Flag, _callback);
	}

	void OnDestroy() {
		ValueStore.RemoveValueChanged (Flag, _callback);
	}

	private void SetVisibility(bool value, bool force = false) {
		if (!UseAnimationIfAvailable || value || force || gameObject.GetComponent<Animation>() == null) {
			gameObject.GetComponent<SpriteRenderer> ().enabled = value;
		} else if (!value) {
			gameObject.animation.Play ();
		}
	}
}
