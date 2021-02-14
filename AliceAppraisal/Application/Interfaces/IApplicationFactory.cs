using AliceAppraisal.Infrastructure;

namespace AliceAppraisal.Application {
	public interface IApplicationFactory {
		IMainHandler CreateHandler();
		IStorageService GetStorage();
	}
}