using AliceAppraisal.Application.Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;


namespace AliceAppraisal.Application.Configuration {

	public class Settings {
		public static readonly string AppId = "AliceAppraisalApp";
		public static Settings Instance = new Settings();
		public static JsonSerializerOptions JsonOptions { get; } = new JsonSerializerOptions {
			PropertyNamingPolicy = SnakeCaseNamingPolicy.Instance,
			IgnoreNullValues = true
		};

		/// <summary>
		/// Мапинг схожих названий у моделей
		/// </summary>
		public static string SimilarNamesFullUrl { get; } = "https://raw.githubusercontent.com/milovidov983/PublicData/master/appraisalbot/similarNames.json";
		public static string MakeModelMapPartUrl { get; } = "https://raw.githubusercontent.com/milovidov983/PublicData/master/appraisalbot/makes/";

		private Settings() {
			var logger = new LoggerConfiguration()
				.WriteTo
				.Console()
				.MinimumLevel
				.Debug()
				.CreateLogger();
			try {
				var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
				if (env is null) {
					Env = "develop";
				}

				var configuration = new ConfigurationBuilder()
					.AddEnvironmentVariables($"{AppId}:")
					.Build();

				configuration.Bind(Instance);
				logger.Information("Settings Ok");
			} catch(Exception e) {
				logger.Information(e.Message);
			}


			
		}


		public string Env { get; set; }

		public SheetConfig SheetConfig { get => new SheetConfig {
			ClientId = ClientId,
			ClientSecret = ClientSecret,
			SpreadsheetId = SpreadsheetId,
			User = User
		}; }
		public string SpreadsheetId { get; set; }
		public string ClientId { get; set; }
		public string ClientSecret { get; set; }
		public string User { get; set; }
	}
}
