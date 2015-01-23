using System;
using System.IO;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace BitterEnd
{
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

		private static readonly Regex _parseMenu =
			new Regex (@"^menu:$", RegexOptions.IgnoreCase);

		private static readonly Regex _parseMenuChoice =
			new Regex (@"^(['""])((?:\\\1|.)*?)\1:$");

		private static readonly Regex _parseJump =
			new Regex (@"^jump\s+(\w+)$", RegexOptions.IgnoreCase);

		private Dialogue Parse() {
			var dialogue = new Dialogue ();
			DialoguePart currentPart = null;
			Menu.Choice currentChoice = null;

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
					currentPart = dialogue.DialogueParts[name] = new DialoguePart(name);
					continue;
				}

				if ((match = _parseText.Match (line)).Success) {
					if (currentPart == null) {
						throw new FormatException(string.Format ("Found text line {0}, but not in a dialogue part.", line));
					}

					if (currentPart.Menu != null) {
						throw new FormatException(string.Format("Found text line {0}, but in a menu part.", line));
					}

					var hasCharacter = match.Groups[1].Success;
					var characterLabel = match.Groups[1].Value;
					if (hasCharacter && !dialogue.Characters.ContainsKey(characterLabel)) {
						throw new FormatException(string.Format("Found character label {0}, but no such character in dialogue.", characterLabel));
					}

					var text = match.Groups[3].Value;

					var dialogueLine = hasCharacter ? new Line(dialogue.Characters[characterLabel], text) : new Line(text);
					currentPart.Lines.Add (dialogueLine);

					continue;
				}

				if ((match = _parseMenu.Match (line)).Success) {
					if (currentPart.Menu != null) {
						throw new FormatException("Found menu declaration, but already in a menu part.");
					}

					currentPart.Menu = new Menu();
					continue;
				}

				if ((match = _parseMenuChoice.Match (line)).Success) {
					if (currentPart.Menu == null) {
						throw new FormatException(string.Format ("Found menu choice {0}, but not in a menu part.", line));
					}

					currentChoice =	new Menu.Choice(match.Groups[2].Value);
					currentPart.Menu.Choices.Add (currentChoice);
					continue;
				}

				if ((match = _parseJump.Match (line)).Success) {
					if (currentPart.Menu == null) {
						throw new FormatException(string.Format ("Found jump {0}, but not in a menu choice.", line));
					}

					currentChoice.JumpTargetLabel = match.Groups[1].Value;
					continue;
				}

				throw new FormatException(string.Format("Couldn't parse line {0}.", line));
			}

			// Compile jumps.
			foreach (var pair in dialogue.DialogueParts) {
				if (pair.Value.Menu == null) {
					continue;
				}

				foreach (var choice in pair.Value.Menu.Choices) {
					DialoguePart jumpTarget;
					if (!dialogue.DialogueParts.TryGetValue(choice.JumpTargetLabel, out jumpTarget)) {
						throw new FormatException(string.Format("Couldn't find target for jump {0}.", choice.JumpTargetLabel));
					}

					choice.JumpTarget = jumpTarget;
				}
			}

			return dialogue;
		}	
	}
}