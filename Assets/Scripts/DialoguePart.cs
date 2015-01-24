using System;
using System.Collections.Generic;

namespace BitterEnd {
	public class DialoguePart
	{
		public string Name { get; private set; }

		public List<Line> Lines { get { return _lines; } }
		private List<Line> _lines = new List<Line>();

		/// <remarks>If exists, the <c>Menu</c> shown at the end of this part.</remarks>
		public Menu Menu { get; set; }

		/// <remarks>Either Menu or JumpTarget(Label) may be specified, but not both.</remarks>
		public string JumpTargetLabel { get; set; }
		public DialoguePart JumpTarget { get; set; }

		public DialoguePart (string name)
		{
			Name = name;
		}

		public override string ToString ()
		{
			return string.Format ("[DialoguePart: Name={0}]", Name, Lines, Menu);
		}

		public Iterator Start() {
			return new DialoguePart.Iterator (this);
		}

		public class Iterator {
			public DialoguePart DialoguePart { get; private set; }
			private int _currentLine;

			public Iterator(DialoguePart dialoguePart) {
				DialoguePart = dialoguePart;
				_currentLine = 0;
			}
			
			public Line CurrentLine {
				get {
					return DialoguePart.Lines[_currentLine];
				}
			}
			
			public bool Next() {
				++_currentLine;
				return _currentLine < DialoguePart.Lines.Count;
			}
		}
	}
}