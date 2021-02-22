using Newtonsoft.Json;
using Serilog;
using System;
using System.Text.Json;


namespace AliceAppraisal.Application.Configuration {

	public class Settings {
		public static readonly string AppId = "AliceAppraisalApp";

		public static Settings Instance = new Settings();
		public static JsonSerializerOptions JsonOptions { get; } = new JsonSerializerOptions {
			PropertyNamingPolicy = SnakeCaseNamingPolicy.Instance,
			IgnoreNullValues = true
		};

		private Settings() {
			var logger = new LoggerConfiguration()
				.WriteTo
				.Console()
				.MinimumLevel
				.Debug()
				.CreateLogger();

			try {
				Environment = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "develop";
				Domain = System.Environment.GetEnvironmentVariable(nameof(Domain));

				var _envLogLevel = System.Environment.GetEnvironmentVariable(nameof(LogLevel));
				Enum.TryParse(typeof(Serilog.Events.LogEventLevel), _envLogLevel, true, out var parsedLogLevel);
				LogLevel = (Serilog.Events.LogEventLevel)(parsedLogLevel ?? Serilog.Events.LogEventLevel.Information);


				Console.WriteLine($"{nameof(Environment)}: {Environment}");
				Console.WriteLine($"{nameof(IsProduction)}: {IsProduction}");
				Console.WriteLine($"{nameof(LogLevel)}: {LogLevel}");
			} catch(Exception e) {
				logger.Information("Error configuration initialization");
				logger.Error(e.Message);
			}
		}

		public bool IsProduction { get => Environment == "production"; }

		public Serilog.Events.LogEventLevel LogLevel { get; }
		public string Environment { get; }
		public string Domain { get; }
		/// <summary>
		/// Мапинг схожих названий у моделей
		/// </summary>
		public static string SimilarNamesFullUrl { get; } = "https://raw.githubusercontent.com/milovidov983/PublicData/master/appraisalbot/similarNames.json";
		public static string MakeModelMapPartUrl { get; } = "https://raw.githubusercontent.com/milovidov983/PublicData/master/appraisalbot/makes/";
	}
}
