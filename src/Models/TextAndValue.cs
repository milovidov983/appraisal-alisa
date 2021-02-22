using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace AliceAppraisal.Models {
	public class TextAndValue {
		[JsonPropertyName("text")]
		public string Text { get; set; }
		[JsonPropertyName("value")]
		public int Value { get; set; }
	}
}
