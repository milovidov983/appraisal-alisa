namespace AliceAppraisal.Core.Engine {
	public interface IStepManager {
		void ChangeDefaultStepTo(string nextStep);
		string GetNextStep(BaseStrategy current);
		BaseStrategy GetNextStrategy(BaseStrategy currentStep);
		void ResetCustomStep();
	}
}