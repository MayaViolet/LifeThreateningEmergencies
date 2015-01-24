using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace BitterEnd {
	public class DialoguePart
	{
		public string Name { get; private set; }

		public List<DialogueElement> Elements { get { return _elements; } }
		private List<DialogueElement> _elements = new List<DialogueElement>();

		public DialoguePart (string name)
		{
			Name = name;
		}

		public override string ToString ()
		{
			return string.Format ("[DialoguePart: Name={0}]", Name);
		}

		public Iterator Start() {
			return new DialoguePart.Iterator (this);
		}

		public void RenderTo(StringBuilder sb, bool includeLabel = true) {
			if (includeLabel) {
				sb.AppendFormat ("label {0}:\n", Name);
			}

			foreach (var element in Elements) {
				element.RenderTo(sb);
			}

			sb.AppendFormat ("\n");
		}

		public class Iterator {
			private class Return {
				public DialoguePart DialoguePart;
				public int NextElement;

				public Return(DialoguePart dialoguePart, int nextElement) {
					DialoguePart = dialoguePart;
					NextElement = nextElement;
				}
			}

			public DialoguePart DialoguePart { get; private set; }
			private int _currentElement;
			private Stack<Return> _returns;

			public Iterator(DialoguePart dialoguePart) {
				DialoguePart = dialoguePart;
				_currentElement = 0;
				_returns = new Stack<Return>();

				ProcessUntilLine();
			}
			
			public DialogueElement CurrentElement {
				get {
					Debug.Log ("Get element " + _currentElement.ToString() + " from " + DialoguePart);
					return DialoguePart.Elements[_currentElement];
				}
			}

			public bool Next() {
				++_currentElement;
				ProcessUntilLine ();

				if (DialoguePart == null || _currentElement >= DialoguePart.Elements.Count) {
					if (!_returns.Any()) {
						return false;
					}

					Debug.Log ("Returning");
					var ourReturn = _returns.Pop ();
					DialoguePart = ourReturn.DialoguePart;
					_currentElement = ourReturn.NextElement - 1;
					return Next ();
				}

				return true;
			}

			private void ProcessUntilLine() {
				while (_currentElement < DialoguePart.Elements.Count) {
					var element = CurrentElement;
					// This bit is neither object-oriented nor functional.  Sorry!

					var sb = new StringBuilder ();
					element.RenderTo (sb);
					Debug.Log (sb.ToString ());

					if (element is DialogueLine || element is DialogueMenu || element is DialogueTransition) {
						return;
					}

					var jump = element as DialogueJump;
					if (jump != null) {
						// Jumps always clear the return stack (for conditionals).
						_returns = new Stack<Return> ();

						DialoguePart = jump.Target;
						if (DialoguePart == null) {
							return;
						}

						_currentElement = 0;
						continue;
					}

					var assignment = element as DialogueAssignment;
					if (assignment != null) {
						ValueStore.Store (assignment.Variable, assignment.Value);
						++_currentElement;
						continue;
					}

					var conditional = element as DialogueConditional;
					if (conditional != null) {
						var value = ValueStore.Retrieve (conditional.Variable) ^ conditional.Negated;
						if (!value) {
							++_currentElement;
							continue;
						}

						sb = new StringBuilder ();
						conditional.DialoguePart.RenderTo (sb);
						Debug.Log (sb.ToString ());
						_returns.Push (new Return (DialoguePart, _currentElement + 1));
						DialoguePart = conditional.DialoguePart;
						_currentElement = 0;
						continue;
					}

					var sound = element as DialogueSound;
					if (sound != null) {
						var audioClip = Resources.Load<AudioClip> (string.Format ("Sounds/{0}", sound.ResourceId));

						var audioSource = GameObject.FindGameObjectWithTag ("DialogueController").GetComponent<AudioSource> ();
						audioSource.clip = audioClip;
						audioSource.Play ();

						++_currentElement;
						continue;
					}

					throw new InvalidOperationException (string.Format ("Unknown DialogueElement: {0}", element));
				}
			}
		}
	}
}