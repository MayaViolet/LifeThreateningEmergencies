using UnityEngine;
using System.Collections;

public class BindVisibilityToValueBehaviour : MonoBehaviour {
	public enum VisibleWhen	{
		True,
		False
	};
	
	public VisibleWhen VisibleWhenFlagIs = VisibleWhen.False;
	public string Flag;
	
	void Start () {
		ValueStore.OnValueChanged (Flag, value => {
			gameObject.GetComponent<SpriteRenderer>().enabled = (VisibleWhenFlagIs == VisibleWhen.False) ? !value : value;
		});
	}
	
	void Update () {
	
	}
}
