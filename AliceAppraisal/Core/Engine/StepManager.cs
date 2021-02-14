using AliceAppraisal.Engine.Strategy;
using AliceAppraisal.Static;

namespace AliceAppraisal.Engine {
	/// <summary>
	/// Управляет порядком переходов между шагами
	/// </summary>
	public class StepManager : IStepManager {
		private readonly IServiceFactory serviceFactory;
		private string customNextStep;
		private string NextDefaultStep(BaseStrategy currentStep)
			=> Transitions.GetDeafultNextStep(currentStep); 

		public StepManager(IServiceFactory serviceFactory) {
			this.serviceFactory = serviceFactory;
		}

		/// <summary>
		/// Получить название следующего шага
		/// </summary>
		public string GetNextStep(BaseStrategy current) => customNextStep ?? NextDefaultStep(current);

		/// <summary>
		/// Получить обработчик следующего шага
		/// </summary>
		public BaseStrategy GetNextStrategy(BaseStrategy currentStep) {
			IStrategyFactory strategyFactory = serviceFactory.StrategyFactory;
			string nextStep = GetNextStep(currentStep);
			BaseStrategy nextStepInstanse = strategyFactory.GetStrategy(nextStep);
			return nextStepInstanse;
		}

		/// <summary>
		/// Заменить кастомный шаг на дефолтный
		/// </summary>
		public void ResetCustomStep() {
			customNextStep = null;
		}

		/// <summary>
		/// Изменить следующий шаг описанный по умолчанию для текущего на другой
		/// </summary>
		public void ChangeDefaultStepTo(string nextStep) {
			customNextStep = nextStep;
		}
	}
}
