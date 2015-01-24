namespace BitterEnd {
	public class Character
	{
		public string Name { get; private set; }

		public string Friendly { get; private set; }

		public string PortraitId { get; private set; }
		
		public Character (string name, string friendly, string portraitId) {
			Name = name;
			Friendly = friendly;
			PortraitId = portraitId;
		}
	}
}