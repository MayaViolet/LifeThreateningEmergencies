using NUnit.Framework;
using System.Reflection;
using System.Collections;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace BitterEnd
{
	[TestFixture]
	public class RenPyParserTests {
		[Test]
		public void TestSimpleDialogue() {
			var dialogueText = GetResourceContent ("Editor.Tests.TestDialogue.txt");
			var dialogue = RenPyParser.ReadDialogueFromString (dialogueText);

			Assert.AreEqual (CollapseWhitespace(dialogueText), CollapseWhitespace(dialogue.Render ()));
		}

		private static readonly Regex _whitespace = 
			new Regex (@"\s+");

		private static string CollapseWhitespace(string text) {
			return _whitespace.Replace (text, " ").Trim ();
		}

		private static string GetResourceContent(string resourceName) {
			var assembly = Assembly.GetExecutingAssembly ();
			var resource = string.Format (resourceName);
			using (var stream = assembly.GetManifestResourceStream(resource)) {
				Debug.Assert(stream != null);
				using (var reader = new StreamReader(stream)) {
					return reader.ReadToEnd ();
				}
			}
		}
	}
}