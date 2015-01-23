using NUnit.Framework;
using System.Collections;

namespace BitterEnd {
	[TestFixture]
	public class RenPyParserTests {
		[Test]
		public void TestSimpleDialogue() {
			var dialogue = RenPyParser.ReadDialogueFromString (
				"define annie = Character('Annie')\n" +
				"define varus = Character(\"Varus\")\n" +
				"\n" +
				"label start:\n" +
				"\t\"Ambient text ...\"\n" +
				"\n" +
				"\tannie \"Something about rice cakes.\"");
			
			CollectionAssert.AreEquivalent (new[] {"annie", "varus"}, dialogue.Characters.Keys);
			var annie = dialogue.Characters ["annie"];
			var varus = dialogue.Characters ["varus"];

			Assert.AreEqual ("Annie", annie.Name);
			Assert.AreEqual ("Varus", varus.Name);

			CollectionAssert.AreEquivalent (new[] {"start"}, dialogue.DialogueParts.Keys);
			CollectionAssert.AreEqual (
				new[] {
					new Line ("Ambient text ..."),
					new Line (annie, "Something about rice cakes."),
				},
				dialogue.DialogueParts ["start"].Lines);
		}
	}
}