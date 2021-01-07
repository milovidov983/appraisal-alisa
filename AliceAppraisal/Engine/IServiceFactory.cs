using AliceAppraisal.Engine.Stratagy;

namespace AliceAppraisal.Engine.Services {
	public interface IServiceFactory {
		IExternalService GetExternalService();
		ITextGeneratorService GetTextGeneratorService();
	}
}