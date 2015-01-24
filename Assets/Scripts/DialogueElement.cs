using System;
using System.Text;

namespace BitterEnd
{
	public abstract class DialogueElement
	{
		public abstract void RenderTo(StringBuilder sb);
	}
}

