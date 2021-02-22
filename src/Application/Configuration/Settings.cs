using Newtonsoft.Json;
using Serilog;
using Serilog.Events;
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
				Console.OutputEncoding = System.Text.Encoding.UTF8;
				Environment = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "develop";

				Domain = System.Environment.GetEnvironmentVariable(nameof(Domain));
				TelegramBotToken = System.Environment.GetEnvironmentVariable(nameof(TelegramBotToken));
				TelegramChatId = System.Environment.GetEnvironmentVariable(nameof(TelegramChatId));

				var _envLogLevel = System.Environment.GetEnvironmentVariable(nameof(LogLevel));
				Enum.TryParse(typeof(LogEventLevel), _envLogLevel, true, out var parsedLogLevel);
				LogLevel = (LogEventLevel)(parsedLogLevel ?? LogEventLevel.Information);


				Console.WriteLine($"{nameof(Environment)}: {Environment}");
				Console.WriteLine($"{nameof(IsProduction)}: {IsProduction}");
				Console.WriteLine($"{nameof(LogLevel)}: {LogLevel}");
			} catch(Exception e) {
				logger.Information("Error configuration initialization");
				logger.Error(e.Message);
			}
		}

		public bool IsProduction { get => Environment == "production"; }

		public LogEventLevel LogLevel { get; }
		public string Environment { get; }
		public string Domain { get; }
		public string TelegramBotToken { get; }
		public string TelegramChatId { get; }

		/// <summary>
		/// Мапинг схожих названий у моделей
		/// </summary>
		public static string SimilarNamesFullUrl { get; } = "https://raw.githubusercontent.com/milovidov983/PublicData/master/appraisalbot/similarNames.json";
		public static string MakeModelMapPartUrl { get; } = "https://raw.githubusercontent.com/milovidov983/PublicData/master/appraisalbot/makes/";
	}
}
