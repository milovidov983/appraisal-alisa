using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace AliceAppraisal.Configuration {

	public class Settings {
		public static Settings Instance = new Settings();
		public static JsonSerializerOptions JsonOptions { get; } = new JsonSerializerOptions {
			PropertyNamingPolicy = SnakeCaseNamingPolicy.Instance,
			IgnoreNullValues = true
		};

		/// <summary>
		/// Мапинг схожих названий у моделей
		/// </summary>
		public string SimilarNamesFullUrl { get; set; } = "https://raw.githubusercontent.com/milovidov983/PublicData/master/appraisalbot/similarNames.json";
		public string MakeModelMapPartUrl { get; set; } = "https://raw.githubusercontent.com/milovidov983/PublicData/master/appraisalbot/makes/";
	}
}
