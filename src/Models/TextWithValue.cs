using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace AliceAppraisal.Models {
	public class TextWithValue<T> where T : struct {
		[JsonPropertyName("value")]
		public T Value { get; set; }
		[JsonPropertyName("text")]
		public string Text { get; set; }
	}
}
