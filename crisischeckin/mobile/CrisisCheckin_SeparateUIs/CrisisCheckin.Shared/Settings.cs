using System;
using System.IO;

namespace CrisisCheckin.Shared
{
	public class Settings
	{
		public Settings ()
		{
		}

		public string SignedInUsername { get; set; }
		public string SignedInPassword { get; set; }

		#if PORTABLE
		public void Save() 
		{
			throw new NotImplementedException ("Not Implemented on PCL");	
		}

		public void Load()
		{
			throw new NotImplementedException ("Not Implemented on PCL");
		}
		#else
		public void Save()
		{
			var json = JsonSerializer.Serialize (this);
			var path = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "settings.json");
			File.WriteAllText(path, json);
		}

		public static Settings Load()
		{
			try {
				var path = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "settings.json");
				var json = File.ReadAllText (path);
				return JsonSerializer.Deserialize<Settings> (json);
			} catch { }
			return new Settings();
		}
		#endif
	}
}

