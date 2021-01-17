using AliceAppraisal.Engine.Strategy;

namespace AliceAppraisal.Engine {
	public interface IStepManager {
		void ChangeDefaultStepTo(string nextStep);
		string GetNextStep();
		BaseStrategy GetNextStrategy();
		void ResetCustomStep();
	}
}