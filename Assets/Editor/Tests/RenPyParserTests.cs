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
			
			CollectionAssert.AreEquivalent (new[] {"Annie", "Varus"}, dialogue.Characters.Keys);
			CollectionAssert.AreEquivalent (new[] {"start"}, dialogue.DialogueParts.Keys);

		}
	}
}