using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace AliceAppraisal.Configuration {

	public class Settings {
        public static JsonSerializerOptions JsonOptions { get; } = new JsonSerializerOptions {
            PropertyNamingPolicy = SnakeCaseNamingPolicy.Instance,
            IgnoreNullValues = true
        };



	}
}
