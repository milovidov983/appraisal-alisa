namespace AliceAppraisal.Core.Engine {
	public interface IStepService {
		void ChangeDefaultStepTo(string nextStep);
		string GetNextStep(BaseStrategy current);
		BaseStrategy GetNextStrategy(BaseStrategy currentStep);
		void ResetCustomStep();
	}
}