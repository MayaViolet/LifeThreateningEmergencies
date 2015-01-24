using System;

namespace BitterEnd
{
	public class DialogueFade : DialogueElement
	{
		public enum FadeMode { IN, OUT };

		public FadeMode Mode { get; private set; }

		public DialogueFade (FadeMode mode)
		{
			Mode = mode;
		}

		public override void RenderTo (System.Text.StringBuilder sb)
		{
			sb.AppendFormat("\tfade: \"{0}\"\n", Mode == FadeMode.IN ? "in" : "out");
		}
	}
}

