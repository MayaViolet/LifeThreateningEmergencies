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

		public DialoguePart (string name)
		{
			Name = name;
		}

		public override string ToString ()
		{
			return string.Format ("[DialoguePart: Name={0}]", Name, Lines, Menu);
		}
	}
}