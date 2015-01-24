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

		/// <remarks>If exists, the <c>Menu</c> shown at the end of this part.</remarks>
		public DialogueMenu Menu { get; set; }

		/// <remarks>Either Menu or JumpTarget(Label) may be specified, but not both.</remarks>
		public string JumpTargetLabel { get; set; }
		public DialoguePart JumpTarget { get; set; }

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

			if (JumpTarget != null) {
				sb.AppendFormat ("\n\tjump {0}\n", JumpTarget.Name);
			}

			if (Menu != null) {
				sb.AppendFormat ("\n");
				Menu.RenderTo(sb);
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
			}
			
			public DialogueLine CurrentLine {
				get {
					return (DialogueLine) DialoguePart.Elements[_currentElement];
				}
			}
			
			public bool Next() {
				while (true) {
					++_currentElement;

					if (_currentElement >= DialoguePart.Elements.Count) {
						if (!_returns.Any()) {
							return false;
						} else {
							var ourReturn = _returns.Pop ();
							DialoguePart = ourReturn.DialoguePart;
							_currentElement = ourReturn.NextElement - 1;
							continue;
						}
					}

					var element = DialoguePart.Elements[_currentElement];
					// This bit is neither object-oriented nor functional.  Sorry!

					if (element is DialogueLine) {
						return true;
					}

					var assignment = element as DialogueAssignment;
					if (assignment != null) {
						ValueStore.Store(assignment.Variable, assignment.Value);
						continue;
					}

					var conditional = element as DialogueConditional;
					if (conditional != null) {
						var value = ValueStore.Retrieve(conditional.Variable) ^ conditional.Negated;
						if (value) {
							_returns.Push (new Return(DialoguePart, _currentElement + 1));
							DialoguePart = conditional.DialoguePart;
							_currentElement = -1;
						}
						continue;
					}

					var sound = element as DialogueSound;
					if (sound != null) {
						var audioClip = Resources.Load<AudioClip>(string.Format ("Sounds/{0}", sound.ResourceId));
		
						var audioSource = Camera.main.GetComponentInChildren<AudioSource>();
						audioSource.clip = audioClip;
						audioSource.Play ();

						continue;
					}

					throw new InvalidOperationException(string.Format ("Unknown DialogueElement: {0}", element));
				}
			}
		}
	}
}