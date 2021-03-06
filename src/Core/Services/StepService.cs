﻿using AliceAppraisal.Core.Engine;
using AliceAppraisal.Static;

namespace AliceAppraisal.Core {
	/// <summary>
	/// Управляет порядком переходов между шагами
	/// </summary>
	public class StepService : IStepService {
		private readonly IServiceFactory serviceFactory;
		private string customNextStep;
		private string NextDefaultStep(BaseStrategy currentStep)
			=> DefaultTransitions.GetDeafultNextStep(currentStep); 

		public StepService(IServiceFactory serviceFactory) {
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
