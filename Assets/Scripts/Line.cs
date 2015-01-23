using System;

namespace BitterEnd {
	public class Line
	{
		public Character Character { get; private set; }
		public string Text { get; private set; }

		public Line (Character character, string text)
		{
			Character = character;
			Text = text;
		}
	}
}