
using System;
namespace BitterEnd
{
	public class DialogueAnimate : DialogueElement
	{
		public DialogueAnimate ()
		{
		}

		public override void RenderTo (System.Text.StringBuilder sb)
		{
			sb.AppendFormat ("\tanimate\n");
		}
	}
}

