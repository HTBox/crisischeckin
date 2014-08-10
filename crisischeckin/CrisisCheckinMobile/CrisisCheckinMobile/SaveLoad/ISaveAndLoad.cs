namespace CrisisCheckinMobile.SaveLoad
{
	public interface ISaveAndLoad
	{
		void SaveText(string filename, string text);
		string LoadText(string filename);
	}
}