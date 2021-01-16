using AliceAppraisal.Engine;
using Serilog;

namespace AliceAppraisal.Application {
	public interface IHandlerFactory {
		IMainHandler Create();
	}
}