using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace BitterEnd
{
	public class Dialogue
	{
		private Dictionary<string, Character> _characters = new Dictionary<string, Character>();
		private Dictionary<string, DialoguePart> _dialogueParts = new Dictionary<string, DialoguePart> ();
		private List<string> _dialoguePartOrder = new List<string>();

		public Dictionary<string, Character> Characters { get { return _characters; } }
		public Dictionary<string, DialoguePart> DialogueParts { get { return _dialogueParts; } }
		public List<string> DialoguePartOrder { get { return _dialoguePartOrder; } }

		public Dialogue ()
		{
		}

		public DialoguePart.Iterator Start(GameObject hostGO) {
			return this.DialogueParts ["start"].Start (hostGO);
		}

		public string Render() {
			var sb = new StringBuilder ();

			foreach (var characterKey in _characters.Keys.OrderBy(k => k)) {
				var character = _characters[characterKey];
				sb.AppendFormat ("define {0} = Character(\"{1}\"", character.Name, character.Friendly);
				if (character.PortraitId != null) {
					sb.AppendFormat (", \"{0}\"", character.PortraitId);
				}
				sb.AppendFormat (")\n");
			}

			sb.AppendFormat ("\n");

			foreach (var dialoguePartKey in _dialoguePartOrder) {
				_dialogueParts[dialoguePartKey].RenderTo(sb);
			}

			return sb.ToString ();
		}
	}
}
