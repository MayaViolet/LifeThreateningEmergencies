using System;
using System.Text;

namespace BitterEnd
{
	public class DialogueLine : DialogueElement
	{
		/// <remarks>May be <c>null</c> if ambient text.</remarks>
		public Character Character { get; private set; }

		public string Text { get; private set; }

		public DialogueLine (string text)
		{
			Text = text;
		}

		public DialogueLine (Character character, string text)
		{
			Character = character;
			Text = text;
		}

		public override string ToString ()
		{
			return string.Format ("[DialogueLine: Character={0}, Text={1}]", Character, Text);
		}

		public override bool Equals (object obj)
		{
			var line = obj as DialogueLine;
			if (line == null) {
				return false;
			}

			return Character == line.Character && Text == line.Text;
		}

		public override int GetHashCode ()
		{
			return base.GetHashCode ();
		}

		public override void RenderTo (StringBuilder sb)
		{
			sb.AppendFormat ("\t");

			if (Character != null) {
				sb.AppendFormat ("{0} ", Character.Name);
			}

			sb.AppendFormat ("\"{0}\"\n", Text);
		}
	}
}