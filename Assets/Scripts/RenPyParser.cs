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

		private static readonly Regex _parseLabel =
			new Regex (@"^label\s+(\w+):$", RegexOptions.IgnoreCase);

		private static readonly Regex _parseText =
			new Regex (@"^(?:(\w+)\s+)?(['""])((?:\\\2|.)*?)\2$");

		private Dialogue Parse() {
			var dialogue = new Dialogue ();
			DialoguePart currentPart = null;

			foreach (string line in _source.Split ('\n').Select(s => s.Trim()).Where (s => s.Length > 0)) {
				// I can't believe I'm doing this all with regular expressions but it's a game jam so WHATEVER.
				Match match;

				if ((match = _parseDefine.Match (line)).Success) {
					var name = match.Groups[1].Value;
					var friendly = match.Groups[3].Value;
					dialogue.Characters[name] = new Character(friendly);
					continue;
				}

				if ((match = _parseLabel.Match (line)).Success) {
					var name = match.Groups[1].Value;
					currentPart = dialogue.DialogueParts[name] = new DialoguePart();
					continue;
				}

				if ((match = _parseText.Match (line)).Success) {
					if (currentPart == null) {
						throw new FormatException(string.Format ("Found text line {0} but not in a dialogue part.", line));
					}

					var hasCharacter = match.Groups[1].Success;
					var characterLabel = match.Groups[1].Value;
					if (hasCharacter && !dialogue.Characters.ContainsKey(characterLabel)) {
						throw new FormatException(string.Format("Found character label {0} but no such character in dialogue.", characterLabel));
					}

					var text = match.Groups[3].Value;

					var dialogueLine = hasCharacter ? new Line(dialogue.Characters[characterLabel], text) : new Line(text);
					currentPart.Lines.Add (dialogueLine);

					continue;
				}
			}

			return dialogue;
		}	
	}
}