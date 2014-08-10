using CrisisCheckinMobile.WinPhone;
using CrisisCheckinMobile.SaveLoad;
using System;
using Xamarin.Forms;

[assembly: Dependency(typeof(SaveAndLoad_WinPhone))]
namespace CrisisCheckinMobile.WinPhone
{
	public class SaveAndLoad_WinPhone : ISaveAndLoad
	{
		public SaveAndLoad_WinPhone() { }

		public void SaveText(string filename, string text)
		{
			throw new NotImplementedException("Saving files not yet supported for Windows Phone");
		}

		public string LoadText(string filename)
		{
			throw new NotImplementedException("Loading files not yet supported for Windows Phone");
		}
	}
}