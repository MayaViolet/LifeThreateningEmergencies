using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
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

		public static Dialogue ReadDialogueFromResources(string filename) {
			TextAsset textFile = Resources.Load<TextAsset>("Dialogues/"+filename);
			if (textFile == null)
			{
				UnityEngine.Debug.LogError("Dialogue '"+filename+"' not found");
				return null;
			}
			return ReadDialogueFromString(textFile.text);
		}

		public static Dialogue ReadDialogueFromString(string source) {
			return new RenPyParser (source).Parse ();
		}

		private static readonly Regex _parseDefine =
			new Regex(@"^define\s+(\w+)\s*=\s*Character\((['""])((?:\\\2|.)*?)\2(?:\s*,\s*(['""])((?:\\\4|.)*?)\4)?\)$", RegexOptions.IgnoreCase);

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

		private static readonly Regex _parseAssign =
			new Regex (@"^\$\s*(\w+)\s*=\s*(True|False)$", RegexOptions.IgnoreCase);

		private static readonly Regex _parseIf =
			new Regex (@"^if\s+(not\s+)?(\w+)\s*:$", RegexOptions.IgnoreCase);

		private static readonly Regex _parseEndIf =
			new Regex (@"^endif$", RegexOptions.IgnoreCase);

		private static readonly Regex _parseSound =
			new Regex (@"^sound: (['""])((?:\\\1|.)*?)\1$", RegexOptions.IgnoreCase);

		private static readonly Regex _parseReturn =
			new Regex (@"^return$", RegexOptions.IgnoreCase);

		private static readonly Regex _parseTransition =
			new Regex (@"^transition: (['""])((?:\\\1|.)*?)\1$", RegexOptions.IgnoreCase);

		private enum ParserState {
			PROLOGUE,
			LINES,
			JUMPED,
			MENU,
			MENU_CHOICE,
		}

		private Dialogue _dialogue;
		private ParserState _state;
		private DialoguePart _currentPart;
		private DialogueMenu _currentMenu;
		private DialogueMenu.Choice _currentChoice;
		private Stack<DialoguePart> _partStack;
		private List<DialogueJump> _dialogueJumps;

		private Dialogue Parse() {
			_dialogue = new Dialogue ();
			_state = ParserState.PROLOGUE;
			_currentPart = null;
			_currentMenu = null;
			_currentChoice = null;
			_partStack = new Stack<DialoguePart> ();
			_dialogueJumps = new List<DialogueJump> ();

			var lines = _source.Split ('\n').Select (s => s.Trim ()).Where (s => s.Length > 0);

			foreach (string line in lines) {
				// I can't believe I'm doing this all with regular expressions but it's a game jam so WHATEVER.

				switch (_state) {
				case ParserState.PROLOGUE:
					if (ParseDefine (line)) {
						continue;
					}

					if (ParseLabel (line)) {
						_state = ParserState.LINES;
						continue;
					}

					break;
						
				case ParserState.LINES:
					if (ParseText (line)) {
						continue;
					}

					if (ParseMenu(line)) {
						_state = ParserState.MENU;
						continue;
					}

					if (ParseJump (line)) {
						_state = ParserState.JUMPED;
						continue;
					}

					if (ParseAssign(line)) {
						continue;
					}

					if (ParseSound (line)) {
						continue;
					}

					if (ParseIf (line)) {
						continue;
					}

					if (ParseEndIf (line)) {
						continue;
					}

					if (ParseLabel (line)) {
						continue;
					}

					if (ParseTransition (line)) {
						_state = ParserState.JUMPED;
						continue;
					}

					break;

				case ParserState.MENU:
					if (ParseMenuChoice(line)) {
						_state = ParserState.MENU_CHOICE;
						continue;
					}

					if (ParseLabel(line)) {
						_state = ParserState.LINES;
						continue;
					}

					break;

				case ParserState.MENU_CHOICE:
					if (ParseJump (line)) {
						_state = ParserState.MENU;
						continue;
					}

					if (ParseReturn (line)) {
						_state = ParserState.MENU;
						continue;
					}

					break;

				case ParserState.JUMPED:
					if (ParseLabel (line)) {
						_state = ParserState.LINES;
						continue;
					}

					if (ParseEndIf (line)) {
						_state = ParserState.LINES;
						continue;
					}

					break;
				}
					
				throw new FormatException(string.Format("Couldn't parse line {0} in state {1}.", line, _state));
			}

			// Compile jumps.
			foreach (var dialogueJump in _dialogueJumps) {
				if (dialogueJump.TargetLabel != null) {
					DialoguePart jumpTarget;
					if (!_dialogue.DialogueParts.TryGetValue (dialogueJump.TargetLabel, out jumpTarget)) {
						throw new FormatException(
							string.Format (
							"Couldn't find target for jump {0}. Candidates were: {1}",
							dialogueJump.TargetLabel,
							string.Join(", ", _dialogue.DialogueParts.Keys.ToArray())));
					}

					dialogueJump.Target = jumpTarget;
				}
			}

			return _dialogue;
		}

		private bool ParseDefine(string line) {
			var match = _parseDefine.Match (line);
			if (!match.Success) {
				return false;
			}
			
			var name = match.Groups[1].Value;
			var friendly = match.Groups[3].Value;
			var portraitId = match.Groups[5].Success ? match.Groups[5].Value : friendly;
			_dialogue.Characters[name] = new Character(name, friendly, portraitId);
			return true;
		}
		
		private bool ParseLabel(string line) {
			var match = _parseLabel.Match (line);
			if (!match.Success) {
				return false;
			}
			
			var name = match.Groups[1].Value;
			_currentPart = _dialogue.DialogueParts[name] = new DialoguePart(name);
			_currentMenu = null;
			_dialogue.DialoguePartOrder.Add (name);
			return true;
		}
		
		private bool ParseText(string line) {
			var match = _parseText.Match (line);
			if (!match.Success) {
				return false;
			}
			
			var hasCharacter = match.Groups[1].Success;
			var characterLabel = match.Groups[1].Value;
			if (hasCharacter && !_dialogue.Characters.ContainsKey(characterLabel)) {
				throw new FormatException(string.Format("Found character label {0}, but no such character in dialogue.", characterLabel));
			}
			
			var text = match.Groups[3].Value;
			
			var dialogueLine = hasCharacter ? new DialogueLine(_dialogue.Characters[characterLabel], text) : new DialogueLine(text);
			_currentPart.Elements.Add (dialogueLine);
			
			return true;
		}
		
		private bool ParseMenu(string line) {
			var match = _parseMenu.Match (line);
			if (!match.Success) {
				return false;
			}
			
			_currentPart.Elements.Add (_currentMenu = new DialogueMenu());
			return true;
		}
		
		private bool ParseMenuChoice(string line) {
			var match = _parseMenuChoice.Match (line);
			if (!match.Success) {
				return false;
			}
			
			_currentChoice = new DialogueMenu.Choice(match.Groups[2].Value);
			_currentMenu.Choices.Add (_currentChoice);
			return true;
		}
		
		private bool ParseJump(string line) {
			var match = _parseJump.Match (line);
			if (!match.Success) {
				return false;
			}
			
			// Implementation differs depending on if called from LINES or MENU_CHOICE.

			DialogueJump jump;
			if (_currentMenu == null) {
				_currentPart.Elements.Add (jump = new DialogueJump(match.Groups [1].Value));
			} else {
				jump = _currentChoice.DialogueJump = new DialogueJump(match.Groups [1].Value);
			}

			_dialogueJumps.Add (jump);
			
			return true;
		}

		private bool ParseReturn(string line) {
			var match = _parseReturn.Match (line);
			if (!match.Success) {
				return false;
			}

			return true;
		}
		
		private bool ParseAssign(string line) {
			var match = _parseAssign.Match (line);
			if (!match.Success) {
				return false;
			}
			
			_currentPart.Elements.Add (new DialogueAssignment(match.Groups[1].Value, match.Groups[2].Value == "True"));
			return true;
		}
		
		private bool ParseIf(string line) {
			var match = _parseIf.Match (line);
			if (!match.Success) {
				return false;
			}

			var subPart = new DialoguePart ("if subpart");
			var conditional = new DialogueConditional (match.Groups[1].Success, match.Groups[2].Value, subPart);
			_currentPart.Elements.Add (conditional);
			_partStack.Push (_currentPart);
			_currentPart = subPart;
			
			return true;
		}
		
		private bool ParseEndIf(string line) {
			var match = _parseEndIf.Match (line);
			if (!match.Success) {
				return false;
			}

			_currentPart = _partStack.Pop ();
			
			return true;
		}

		private bool ParseSound(string line) {
			var match = _parseSound.Match (line);
			if (!match.Success) {
				return false;
			}

			_currentPart.Elements.Add (new DialogueSound (match.Groups [2].Value));

			return true;
		}

		private bool ParseTransition(string line) {
			var match = _parseTransition.Match (line);
			if (!match.Success) { 
				return false;
			}

			_currentPart.Elements.Add (new DialogueTransition(match.Groups[2].Value));

			return true;
		}
	}
}