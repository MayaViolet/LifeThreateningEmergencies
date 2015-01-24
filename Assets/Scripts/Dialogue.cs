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

		public static Dialogue GetTestDialogue() {

			var d = new Dialogue ();

			var leona = d.Characters ["Leona"] = new Character ("Leona");
			var ashe = d.Characters ["Ashe"] = new Character ("Ashe");

			var thin = d.DialogueParts ["thin"] = new DialoguePart ("thin");
			thin.Lines.AddRange (new[] {
				new Line (leona, "SEEMS FINE."),
			});

			var thick = d.DialogueParts ["thick"] = new DialoguePart ("thick");
			thick.Lines.AddRange (new[] {
				new Line (ashe, "~wowdy~"),
			});

			var start = d.DialogueParts ["start"] = new DialoguePart ("start");
			start.Lines.AddRange (new [] {
				new Line (leona, "<i>~thin <b>rice</b> cakes~</i>\nWowdy!"),
				new Line (ashe, "Even better than <b>thick</b> rice cakes!"),
			});

			start.Menu = new Menu ();
			start.Menu.Choices.AddRange (new[] {
				new Menu.Choice ("I prefer thin rice cakes.") {JumpTargetLabel = "thin", JumpTarget = thin},
				new Menu.Choice ("Thick rice cakes are ethically superior.") {JumpTargetLabel = "thick", JumpTarget = thick},
			});

			return d;
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
