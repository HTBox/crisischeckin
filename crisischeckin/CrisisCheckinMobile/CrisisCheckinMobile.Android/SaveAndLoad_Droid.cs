using CrisisCheckinMobile.Droid;
using CrisisCheckinMobile.SaveLoad;
using System;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency (typeof (SaveAndLoad_Droid))]
namespace CrisisCheckinMobile.Droid
{
	public class SaveAndLoad_Droid : ISaveAndLoad
	{
		public SaveAndLoad_Droid() {}

		public void SaveText(string filename, string text)
		{
			var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var filePath = Path.Combine(documentsPath, filename);
			System.IO.File.WriteAllText(filePath, text);
		}

		public string LoadText(string filename)
		{
			var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var filePath = Path.Combine(documentsPath, filename);
			return System.IO.File.ReadAllText(filePath);
		}
	}
}