using AliceAppraisal.Infrastructure;
using Serilog;

namespace AliceAppraisal.Application {
	public interface IMainHandlerFactory {
		IMainHandler GetHandler();
	}
}