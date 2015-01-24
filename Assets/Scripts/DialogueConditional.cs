using System;
using System.Text;

namespace BitterEnd
{
	public class DialogueConditional : DialogueElement
	{
		public bool Negated { get; private set; }
		public string Variable { get; private set; }
		public DialoguePart DialoguePart { get; private set; }

		public DialogueConditional (bool negated, string variable, DialoguePart dialoguePart)
		{
			Negated = negated;
			Variable = variable;
			DialoguePart = dialoguePart;
		}

		public override void RenderTo (StringBuilder sb)
		{
			sb.AppendFormat ("\tif ");

			if (Negated) {
				sb.AppendFormat ("not ");
			}

			sb.AppendFormat ("{0}:\n", Variable);

			DialoguePart.RenderTo (sb, false);
			sb.AppendFormat ("\tendif\n");
		}
	}
}

