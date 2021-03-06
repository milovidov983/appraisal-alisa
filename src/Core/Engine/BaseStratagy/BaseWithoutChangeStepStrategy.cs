using AliceAppraisal.Core.Models;
using AliceAppraisal.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Core.Engine.Strategy {
	/// <summary>
	/// Наследуем стратегию от него если не хотим менять текущее и предыдущее состояние шага
	/// </summary>
	public abstract class BaseStrategyWithoutChangeStep : BaseStrategy {
		protected BaseStrategyWithoutChangeStep(IServiceFactory serviceFactory) : base(serviceFactory) {
		}
		protected override void UpdateState(State state) {
			// Do nothing!
		}
	}
}