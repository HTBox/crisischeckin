using System;
using System.Text;
using System.IO;

namespace CrisisCheckin.Shared
{
	public class JsonSerializer
	{
		public JsonSerializer ()
		{
		}

		public static T Deserialize<T>(string json)
		{
			var result = default(T);

			var data = Encoding.UTF8.GetBytes (json);

			using (var ms = new MemoryStream (data)) {

				var serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer (typeof(T));
				result = (T)serializer.ReadObject (ms);
			}

			return result;
		}

		public static string Serialize<T>(T obj)
		{
			string result = null;

			using (var ms = new MemoryStream ()) {
				var serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer (typeof(T));
				serializer.WriteObject (ms, obj);

				result = Encoding.UTF8.GetString(ms.ToArray ());
			}

			return result;
		}
	}
}

