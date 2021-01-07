using AliceAppraisal.Engine.Services;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Stratagy {
	public class AppraisalOtherStratagy : BaseStratagy {
		public AppraisalOtherStratagy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}

		protected override bool Check(AliceRequest request, State state) {
            return request.HasIntent(Intents.AppraisalOther);
		}

        protected override async Task<SimpleResponse> Respond(AliceRequest request, State state) {
			await Task.Yield();
			state.Clear();
			return new SimpleResponse {
				Text = $"Назовите марку авто которое вы хотите оценить.",
				Buttons = new[] { "Оценить другой авто", "Выйти" }
			};
		}
    }
}
