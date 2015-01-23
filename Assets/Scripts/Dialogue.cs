using System;
using System.Collections.Generic;

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

		var start = d.DialogueParts ["start"] = new DialoguePart ();
		start.Lines.Add(new Line(leona, "<i>~thin rice cakes~</i>\nWowdy!"));
		start.Lines.Add (new Line (ashe, "Even better than <b>thick</b> rice cakes!"));

		return d;
	}

	public Iterator Start() {
		return new Iterator(this);
	}

	public class Iterator {
		private Dialogue _dialogue;
		private DialoguePart _currentPart;
		private int _currentLine;

		public Iterator(Dialogue dialogue) {
			_dialogue = dialogue;
			_currentPart = _dialogue.DialogueParts["start"];
			_currentLine = 0;
		}

		public Line CurrentLine {
			get {
				return _currentPart.Lines[_currentLine];
			}
		}

		public bool Next() {
			++_currentLine;
			return _currentLine < _currentPart.Lines.Count;
		}
	}
}
