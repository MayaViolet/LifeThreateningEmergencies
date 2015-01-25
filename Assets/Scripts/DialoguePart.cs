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

		public Iterator Start(GameObject hostGO) {
			return new DialoguePart.Iterator (this, hostGO);
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
			public GameObject HostGO { get; private set; }
			private int _currentElement;
			private Stack<Return> _returns;

			public Iterator(DialoguePart dialoguePart, GameObject hostGO) {
				DialoguePart = dialoguePart;
				_currentElement = 0;
				_returns = new Stack<Return>();
				HostGO = hostGO;

				ProcessUntilLine();
			}
			
			public DialogueElement CurrentElement {
				get {
					return DialoguePart.Elements[_currentElement];
				}
			}

			public bool HasCurrent {
				get {
					return DialoguePart != null && _currentElement < DialoguePart.Elements.Count;
				}
			}

			public bool Next() {
				++_currentElement;
				return ProcessUntilLine ();
			}

			private bool ProcessUntilLine() {
				while (true) {
					while (!HasCurrent) {
						if (!_returns.Any()) {
							return false;
						}

						var ourReturn = _returns.Pop ();
						DialoguePart = ourReturn.DialoguePart;
						_currentElement = ourReturn.NextElement;
					}

					var element = CurrentElement;
					// This bit is neither object-oriented nor functional.  Sorry!

					if (element is DialogueLine || element is DialogueMenu || element is DialogueTransition
					    || element is DialogueFade || element is DialogueWait) {
						return true;
					}

					var jump = element as DialogueJump;
					if (jump != null) {
						// Jumps always clear the return stack (for conditionals).
						_returns = new Stack<Return> ();

						DialoguePart = jump.Target;
						if (DialoguePart == null) {
							return false;
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

					var animate = element as DialogueAnimate;
					if (animate != null) {
						GameObject.FindGameObjectWithTag(animate.GOTag).animation.Play ();

						++_currentElement;
						continue;
					}

					throw new InvalidOperationException (string.Format ("Unknown DialogueElement: {0}", element));
				}
			}
		}
	}
}