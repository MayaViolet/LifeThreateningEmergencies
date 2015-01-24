namespace BitterEnd {
	public class Character
	{
		public string Name { get; private set; }

		public string PortraitId { get; private set; }
		
		public Character (string name, string portraitId) {
			Name = name;
			PortraitId = portraitId;
		}
	}
}