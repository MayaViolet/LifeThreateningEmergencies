using System;

namespace BitterEnd
{
	public class DialogueMoveTo : DialogueElement
	{
		public string TargetGO { get; private set; }

		public DialogueMoveTo (string targetGO)
		{
			TargetGO = targetGO;
		}

		public override void RenderTo (System.Text.StringBuilder sb)
		{
			sb.AppendFormat ("\tmove_to: \"{0}\"\n", TargetGO);
		}
	}
}

