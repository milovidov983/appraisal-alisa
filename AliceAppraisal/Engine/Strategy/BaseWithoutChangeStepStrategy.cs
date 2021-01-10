using AliceAppraisal.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Strategy {
	/// <summary>
	/// Наследуем стратегию от него если не хотим менять текущее и предыдущее состояние шага
	/// </summary>
	public abstract class BaseWithoutChangeStepStrategy : BaseStrategy {
		protected BaseWithoutChangeStepStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}
		protected override void SetCurrentStep(State state) {
			// Do nothing!
		}
	}
}