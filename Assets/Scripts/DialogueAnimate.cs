
using System;
namespace BitterEnd
{
	public class DialogueAnimate : DialogueElement
	{
		public string GOTag { get; private set; }

		public DialogueAnimate (string goTag)
		{
			GOTag = goTag;
		}

		public override void RenderTo (System.Text.StringBuilder sb)
		{
			sb.AppendFormat ("\tanimate: \"{0}\"\n", GOTag);
		}
	}
}

