using System.IO;
using System.Collections;
using System.Linq;

public class RenPyParser {

	private string _source;

	private RenPyParser(Stream input) {
		// HACK: just read the whole thing in right now.
		_source = new StreamReader (input).ReadToEnd ();
	}

	public static Dialogue ReadDialogueFromStream(Stream input) {
		return new RenPyParser (input).Parse ();
	}

	private Dialogue Parse() {
		var dialogue = new Dialogue ();

		foreach (string line in _source.Split ('\n').Select(s => s.Trim()).Where (s => s.Length > 0)) {

		}

		return dialogue;
	}
	
}
