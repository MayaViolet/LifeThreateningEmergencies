using System;
using System.Text;

namespace BitterEnd
{
	public class DialogueAssignment : DialogueElement
	{
		public string Variable { get; private set; }
		public bool Value { get; private set; }

		public DialogueAssignment (string variable, bool value)
		{
			Variable = variable;
			Value = value;
		}

		public override void RenderTo(StringBuilder sb) {
			sb.AppendFormat ("\t$ {0} = {1}\n", Variable, Value ? "True" : "False");
		}
	}
}

