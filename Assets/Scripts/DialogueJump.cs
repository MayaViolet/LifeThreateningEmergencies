using System;
using System.Text;

namespace BitterEnd
{
	public class DialogueJump : DialogueElement
	{
		public string TargetLabel { get; private set; }
		public DialoguePart Target { get; set; }

		public DialogueJump (string targetLabel)
		{
			TargetLabel = targetLabel;
		}

		public override void RenderTo (StringBuilder sb)
		{
			sb.AppendFormat ("\tjump {0}\n", TargetLabel);
		}
	}
}

