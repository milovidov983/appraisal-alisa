using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace AliceAppraisal.Models {
	public class SimilarNames {
		[JsonPropertyName("similarModelNames")]
		public Dictionary<string, int[]> SimilarModelNames { get; set; } 
			= new Dictionary<string, int[]> {

		};

	}


	public class MakeModelsMap {
		[JsonPropertyName("modelIds")]
		public int[] ModelIds { get; set; }
	}
}
