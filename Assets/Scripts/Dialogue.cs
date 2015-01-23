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
		d.Characters ["Ashe"] = new Character ("Ashe");

		var start = d.DialogueParts ["start"] = new DialoguePart ();
		start.Lines.Add(new Line(leona, "~thin rice cakes~\nWowdy!"));

		return d;
	}
}
