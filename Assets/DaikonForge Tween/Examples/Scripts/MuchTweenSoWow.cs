using UnityEngine;
using System.Collections;

using DaikonForge.Tween;
using DaikonForge.Tween.Interpolation;

public class MuchTweenSoWow : MonoBehaviour
{

	public Transform doge;
	public TextMesh[] text;
	public TextMesh logo;

	void Start()
	{

		TweenGroup part1 = new TweenGroup()
			.SetAutoCleanup( true )
			.SetMode( TweenGroupMode.Sequential );

		// NOTE: This could be more efficiently done with Transform.TweenScale(),
		// but we wanted to show an example of using Reflection to tween any 
		// property by name.
		Tween<Vector3> dogeScale = doge.transform
			.TweenProperty<Vector3>( "localScale" )
			.SetStartValue( Vector3.zero )
			.SetDuration( 0.5f )
			.SetDelay( 0.5f )
			.SetInterpolator( EulerInterpolator.Default )
			.SetEasing( TweenEasingFunctions.Spring );

		part1.AppendTween( dogeScale );

		for( int i = 0; i < text.Length; i++ )
		{

			text[ i ].color = new Color( 1, 1, 1, 0 );

			var alphaTween = text[ i ].TweenAlpha()
				.SetAutoCleanup( true )
				.SetStartValue( 0f )
				.SetEndValue( 1f )
				.SetDuration( 0.25f )
				.SetDelay( 0.25f );

			var rotTween = text[ i ].TweenRotation()
				.SetAutoCleanup( true )
				.SetEndValue( Vector3.zero )
				.SetDuration( 0.5f )
				.SetDelay( 0.25f )
				.SetEasing( TweenEasingFunctions.Spring );

			var textPopup = new TweenGroup()
				.SetAutoCleanup( true )
				.SetMode( TweenGroupMode.Concurrent )
				.AppendTween( alphaTween, rotTween );

			part1.AppendTween( textPopup );

		}

		Tween<Vector3> cameraSlide = this.TweenPosition()
			.SetEndValue( transform.position + new Vector3( 0f, -1f, 0f ) )
			.SetDuration( 0.5f )
			.SetEasing( TweenEasingFunctions.EaseInOutQuad );

		Tween<Vector3> logoSlide = logo.TweenPosition()
			.SetStartValue( logo.transform.position - ( Vector3.up * 5 ) )
			.SetDuration( 1f )
			.SetEasing( TweenEasingFunctions.Bounce );

		Tween<float> logoAlphaTween = Tween<float>.Obtain()
				.SetStartValue( 0f )
				.SetEndValue( 1f )
				.SetDuration( 0.5f )
				.SetEasing( TweenEasingFunctions.Linear )
				.OnExecute( ( result ) => { logo.color = new Color( 1f, 1f, 1f, result ); } );

		logo.color = Color.clear;

		TweenGroup part2 = new TweenGroup()
			.SetAutoCleanup( true )
			.SetMode( TweenGroupMode.Concurrent )
			.AppendTween( cameraSlide, logoSlide, logoAlphaTween );

		part1
			.AppendDelay( 1f )
			.AppendTween( part2 )
			.Play();

	}

}