using AliceAppraisal.Engine.Strategy;
using System;
using System.Collections.Generic;
using System.Text;

namespace AliceAppraisal.Static {
	public static class Transitions {

		public static string GetNextStep(BaseStrategy strategy) {
			var nextTransition = strategy switch {
				
				AppraisalOtherStrategy _ => typeof(GetMakeStrategy).FullName,
				GetMakeStrategy _ => typeof(GetModelStrategy).FullName,
				_ => throw new Exception($"Для типа {strategy?.GetType()?.FullName} не описан переход")
			};
			return nextTransition;
		}
	}
}
