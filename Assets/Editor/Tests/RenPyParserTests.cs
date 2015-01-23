using NUnit.Framework;
using System.Reflection;
using System.Collections;
using System.IO;
using System.Diagnostics;

namespace BitterEnd
{
	[TestFixture]
	public class RenPyParserTests {
		[Test]
		public void TestSimpleDialogue() {
			var assembly = Assembly.GetExecutingAssembly ();
			var resource = string.Format ("Editor.Tests.TestDialogue.txt");
			string s;
			using (var stream = assembly.GetManifestResourceStream(resource)) {
				Debug.Assert(stream != null);
				using (var reader = new StreamReader(stream)) {
					s = reader.ReadToEnd();
				}
			}

			var dialogue = RenPyParser.ReadDialogueFromString (s);
			
			CollectionAssert.AreEquivalent (new[] {"annie", "varus"}, dialogue.Characters.Keys);
			var annie = dialogue.Characters ["annie"];
			var varus = dialogue.Characters ["varus"];

			Assert.AreEqual ("Annie", annie.Name);
			Assert.AreEqual ("Varus", varus.Name);

			CollectionAssert.AreEquivalent (new[] {"start", "thin", "thick"}, dialogue.DialogueParts.Keys);
			CollectionAssert.AreEqual (
			new[] {
				new Line ("Ambient text ..."),
				new Line (annie, "Something about rice cakes."),
				new Line (varus, "Sounds good."),
			},
			dialogue.DialogueParts ["start"].Lines);

			CollectionAssert.AreEqual (
			new[] {
				new Menu.Choice("Thin rice cakes.") {JumpTargetLabel = "thin", JumpTarget = dialogue.DialogueParts["thin"]},
				new Menu.Choice("Thick rice cakes.") {JumpTargetLabel = "thick", JumpTarget = dialogue.DialogueParts["thick"]},
			},
			dialogue.DialogueParts ["start"].Menu.Choices);
		}
	}
}