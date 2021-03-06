using AliceAppraisal.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace AliceAppraisal.Application.Models {

	public class AvailableCharacteristics {
		[JsonPropertyName("driveTypes")]
		public TextWithValue<int>[] DriveTypes { get; set; }
		[JsonPropertyName("engineTypes")]
		public TextWithValue<int>[] EngineTypes { get; set; }
		[JsonPropertyName("gearboxTypes")]
		public TextWithValue<int>[] GearboxTypes { get; set; }
		[JsonPropertyName("bodyTypes")]
		public TextWithValue<int>[] BodyTypes { get; set; }
	}
}
