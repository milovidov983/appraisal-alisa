using AliceAppraisal.Application.Configuration;
using AliceAppraisal.Core;
using Serilog;

namespace AliceAppraisal.Application.Infrastructure {
	public class LoggerFactory : ILoggerFactory {
		public readonly static LoggerFactory Instanse = new LoggerFactory();
		private Serilog.Core.Logger _logger;
		private readonly object _lock = new object();

		static LoggerFactory() { }
		public static ILoggerFactory Create() {
			return Instanse;
		}

		public Serilog.Core.Logger GetLogger() {
			if (_logger is null) {
				lock (_lock) {
					if (_logger is null) {
						_logger = new LoggerConfiguration()
							.WriteTo
							.Console()
							.MinimumLevel
							.Is(Settings.Instance.LogLevel)
							.CreateLogger();
					}
				}
			}
			return _logger;
		}
	}
}
