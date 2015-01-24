using System;
using System.Collections.Generic;
using System.Text;

namespace BitterEnd
{
	public class DialogueMenu
	{
		public List<Choice> Choices { get { return _choices; } }
		private List<Choice> _choices = new List<Choice>();

		public DialogueMenu ()
		{
		}

		public void RenderTo(StringBuilder sb) {
			sb.AppendFormat ("menu:\n");
			foreach (var choice in _choices) {
				sb.AppendFormat ("\t\"{0}\":\n\t\tjump {1}\n", choice.Text, choice.JumpTarget.Name);
			}
		}

		public class Choice
		{
			public string Text { get; private set; }
			public string JumpTargetLabel { get; set; }
			public DialoguePart JumpTarget { get; set; }

			public Choice(string text) {
				Text = text;
			}

			public override string ToString ()
			{
				return string.Format ("[Choice: Text={0}, JumpTargetLabel={1}, JumpTarget={2}]", Text, JumpTargetLabel, JumpTarget);
			}

			public override bool Equals (object obj)
			{
				var choice = obj as Choice;
				if (choice == null) {
					return false;
				}

				return Text == choice.Text && JumpTargetLabel == choice.JumpTargetLabel && JumpTarget == choice.JumpTarget;
			}

			public override int GetHashCode ()
			{
				return base.GetHashCode ();
			}
		}
	}
}

