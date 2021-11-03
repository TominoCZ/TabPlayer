namespace TabPlayer
{
	public class JSONInstrument
	{
		public string ID;
		public string Name;
		public string[] Tuning;

		public override string ToString()
		{
			return Name;
		}
	}
}
