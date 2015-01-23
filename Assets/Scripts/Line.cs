using System;

namespace BitterEnd
{
	public class Line
	{
		/// <remarks>May be <c>null</c> if ambient text.</remarks>
		public Character Character { get; private set; }

		public string Text { get; private set; }

		public Line (string text)
		{
			Text = text;
		}

		public Line (Character character, string text)
		{
			Character = character;
			Text = text;
		}

		public override string ToString ()
		{
			return string.Format ("[Line: Character={0}, Text={1}]", Character, Text);
		}

		public override bool Equals (object obj)
		{
			var line = obj as Line;
			if (line == null) {
				return false;
			}

			return Character == line.Character && Text == line.Text;
		}
	}
}