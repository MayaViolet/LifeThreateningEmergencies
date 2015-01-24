using System;
using System.Collections.Generic;
using System.Text;

namespace BitterEnd
{
	public class DialogueMenu : DialogueElement
	{
		public List<Choice> Choices { get { return _choices; } }
		private List<Choice> _choices = new List<Choice>();

		public DialogueMenu ()
		{
		}

		public override void RenderTo(StringBuilder sb) {
			sb.AppendFormat ("menu:\n");
			foreach (var choice in _choices) {
				sb.AppendFormat ("\t\"{0}\":\n\t\tjump {1}\n", choice.Text, choice.DialogueJump.Target.Name);
			}
		}

		public class Choice
		{
			public string Text { get; private set; }
			public DialogueJump DialogueJump { get; set; }

			public Choice(string text) {
				Text = text;
			}
		}
	}
}

