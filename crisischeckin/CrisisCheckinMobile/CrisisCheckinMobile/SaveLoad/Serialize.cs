using Newtonsoft.Json;
using System;
using Xamarin.Forms;

namespace CrisisCheckinMobile.SaveLoad
{
	public static class Serialize
	{
		public static void SaveObject<T>(string filename, T obj)
		{
			string text = JsonConvert.SerializeObject(obj);
			DependencyService.Get<ISaveAndLoad>().SaveText(filename, text);
		}

		public static T LoadObject<T>(string filename)
		{
			string text = DependencyService.Get<ISaveAndLoad>().LoadText(filename);
			return JsonConvert.DeserializeObject<T>(text);
		}
	}
}