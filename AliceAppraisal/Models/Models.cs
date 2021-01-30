using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace AliceAppraisal.Models {
	public class ModelMaps {
		[JsonPropertyName("similarModelNames")]
		public Dictionary<string, int[]> SimilarModelNames { get; set; } 
			= new Dictionary<string, int[]> {

		};


		[JsonPropertyName("makeModels")]
		public Dictionary<int, int[]> MakeModels { get; set; } 
			= new Dictionary<int, int[]> {
	
		};
	}
}
