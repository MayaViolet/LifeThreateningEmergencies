using System;
using System.IO;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace BitterEnd {
	public class RenPyParser {
		private string _source;

		private RenPyParser(string source) {
			_source = source;
		}

		public static Dialogue ReadDialogueFromString(string source) {
			return new RenPyParser (source).Parse ();
		}

		private static readonly Regex _parseDefine =
			new Regex(@"^define\s+(\w+)\s+=\s+Character\((['""])((?:\\\2|.)*?)\2\)$", RegexOptions.IgnoreCase);

		private Dialogue Parse() {
			var dialogue = new Dialogue ();

			foreach (string line in _source.Split ('\n').Select(s => s.Trim()).Where (s => s.Length > 0)) {
				// I can't believe I'm doing this all with regular expressions but it's a game jam so WHATEVER.
				var match = _parseDefine.Match(line);
				if (match.Success) {
					var name = match.Groups[3].Value;
					Console.WriteLine (string.Format ("Parsed {0} as {1}", line, name));
					dialogue.Characters[name] = new Character(name);
					continue;
				}
			}

			return dialogue;
		}	
	}
}