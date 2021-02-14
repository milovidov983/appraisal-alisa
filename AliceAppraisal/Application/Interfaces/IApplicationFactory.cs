using AliceAppraisal.Infrastructure;
using Serilog;

namespace AliceAppraisal.Application {
	public interface IApplicationFactory {
		IMainHandler CreateHandler();
		IStorageService GetStorage();
		ILogger CreateLogger();
	}
}