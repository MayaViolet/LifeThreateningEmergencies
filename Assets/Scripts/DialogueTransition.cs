using System;
using System.Text;

namespace BitterEnd
{
	public class DialogueTransition : DialogueElement
	{
		public string SceneId { get; private set; }

		public DialogueTransition (string sceneId)
		{
			SceneId = sceneId;
		}

		public override void RenderTo (StringBuilder sb)
		{
			sb.AppendFormat ("\ttransition: \"{0}\"\n", SceneId);
		}
	}
}

