using System;

public class TransitionInteraction : AbstractInteraction
{
		public string sceneToLoad;

		public TransitionInteraction ()
		{
		}

		protected override void PerformInteraction ()
		{
				TransitionHandler.Instance.TransitionTo (sceneToLoad);
		}

		protected override float GetMaximumDistance ()
		{
				return 0.3f;
		}
}