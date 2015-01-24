using System;
using System.Collections.Generic;

namespace BitterEnd
{
	public class Dialogue
	{
		private Dictionary<string, Character> _characters = new Dictionary<string, Character>();
		private Dictionary<string, DialoguePart> _dialogueParts = new Dictionary<string, DialoguePart> ();

		public Dictionary<string, Character> Characters { get { return _characters; } }
		public Dictionary<string, DialoguePart> DialogueParts { get { return _dialogueParts; } }

		public Dialogue ()
		{
		}

		public Iterator Start() {
			return new Iterator(this);
		}

		public class Iterator {
			private Dialogue _dialogue;
			public DialoguePart CurrentPart { get; private set; }
			private int _currentLine;

			public Iterator(Dialogue dialogue): this(dialogue, "start") {
			}

			public Iterator(Dialogue dialogue, string part) {
				_dialogue = dialogue;
				CurrentPart = _dialogue.DialogueParts[part];
				_currentLine = 0;
			}

			public Line CurrentLine {
				get {
					return CurrentPart.Lines[_currentLine];
				}
			}

			public bool Next() {
				++_currentLine;
				return _currentLine < CurrentPart.Lines.Count;
			}
		}
	}
}
