// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;
public class TransitionInteraction : AbstractInteraction
{

	public string sceneToLoad;

	public TransitionInteraction ()
	{
	}

	protected override void PerformInteraction() {
		TransitionHandler.Instance.TransitionTo(sceneToLoad);
	}
}