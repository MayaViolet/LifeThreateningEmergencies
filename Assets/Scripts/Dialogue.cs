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

		public DialoguePart.Iterator Start() {
			return this.DialogueParts ["start"].Start ();
		}
	}
}
