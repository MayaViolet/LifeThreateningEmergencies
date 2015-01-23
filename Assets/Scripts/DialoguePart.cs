using System;
using System.Collections.Generic;

public class DialoguePart
{
	public List<Line> Lines { get { return _lines; } }
	private List<Line> _lines = new List<Line>();

	public DialoguePart ()
	{
	}
}
