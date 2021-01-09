using AliceAppraisal.Engine.Services;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Strategy {
	public class UnexpectedInputStrategy : BaseStrategy {
		public UnexpectedInputStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}

		protected override bool Check(AliceRequest request, State state) {
			return false;
		}

        protected override async Task<SimpleResponse> Respond(AliceRequest request, State state) {
			await Task.Yield();

			var prevAaction = state?.PrevAction;

			state.Clear();
			state.SaveCurrentStep(this);
			return new SimpleResponse {
				Text = $"Назовите марку авто которую вы хотите оценить.",
				Buttons = new[] { "Оценить другой авто", "Выйти" }
			};
		}

		//private Func<SimpleResponse> CreateRequestFactoryMethod(string actionName) {
		//	var allAtions = serviceFactory.
		//}


    }
}
