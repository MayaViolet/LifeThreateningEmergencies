using System;
namespace BitterEnd
{
	public class DialogueWait : DialogueElement
	{
		public float Time { get; private set; }

		public DialogueWait (float time)
		{
			Time = time;
		}

		public override void RenderTo (System.Text.StringBuilder sb)
		{
			sb.AppendFormat ("\twait: {0}\n", Time);
		}
	}
}

