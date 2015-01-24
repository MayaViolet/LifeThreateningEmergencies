using System;
using System.Text;

namespace BitterEnd
{
	public class DialogueSound : DialogueElement
	{
		public string ResourceId { get; private set; }

		public DialogueSound (string resourceId)
		{
			ResourceId = resourceId;
		}

		public override void RenderTo (StringBuilder sb)
		{
			sb.AppendFormat ("\tsound: \"{0}\"\n", ResourceId);
		}
	}
}

