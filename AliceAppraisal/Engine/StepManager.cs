using AliceAppraisal.Engine.Strategy;
using AliceAppraisal.Static;

namespace AliceAppraisal.Engine {
	/// <summary>
	/// Управляет порядком переходов между шагами
	/// </summary>
	public class StepManager : IStepManager {
		private readonly BaseStrategy currentStep;
		private readonly IServiceFactory serviceFactory;
		private string customNextStep;
		private string NextDefaultStep { get => Transitions.GetDeafultNextStep(currentStep); }

		public StepManager(BaseStrategy currentStep, IServiceFactory serviceFactory) {
			this.currentStep = currentStep;
			this.serviceFactory = serviceFactory;
		}

		/// <summary>
		/// Получить название следующего шага
		/// </summary>
		public string GetNextStep() => customNextStep ?? NextDefaultStep;

		/// <summary>
		/// Получить обработчик следующего шага
		/// </summary>
		public BaseStrategy GetNextStrategy() {
			IStrategyFactory strategyFactory = serviceFactory.StrategyFactory;
			string nextStep = GetNextStep();
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
