using AliceAppraisal.Models;
using System.Text.Json.Serialization;

namespace AliceAppraisal.Application.Models {

	public class PopularList {
		[JsonPropertyName("popular")]
		public TextWithValue<int>[] Popular { get; set; }
		[JsonPropertyName("all")]
		public TextWithValue<int>[] All { get; set; }
	}
}
