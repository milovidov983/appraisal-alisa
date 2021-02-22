using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace AliceAppraisal.Models {
	public class CityDto {
		[JsonPropertyName("city")]

		public string City { get; set; }
	}
}
