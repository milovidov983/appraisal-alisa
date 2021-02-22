﻿using System.Text.Json;

namespace AliceAppraisal.Application.Configuration {
	public class SnakeCaseNamingPolicy : JsonNamingPolicy {
		public static SnakeCaseNamingPolicy Instance { get; } = new SnakeCaseNamingPolicy();

		public override string ConvertName(string name) {
			// Conversion to other naming convention goes here. Like SnakeCase, KebabCase etc.
			return name.ToSnakeCase();
		}
	}
}
