using Newtonsoft.Json;
using System;
using Xamarin.Forms;

namespace CrisisCheckinMobile.SaveLoad
{
	public class Serialize
	{
		public Serialize() { }

		public void SaveObject(string filename, Object obj)
		{
			string text = JsonConvert.SerializeObject(obj);
			DependencyService.Get<ISaveAndLoad>().SaveText(filename, text);
		}

		public Object LoadObject(string filename)
		{
			string text = DependencyService.Get<ISaveAndLoad>().LoadText(filename);
			return JsonConvert.DeserializeObject<Object>(text);
		}
	}
}
