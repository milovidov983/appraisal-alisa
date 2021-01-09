﻿using AliceAppraisal.Engine.Services;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Strategy {
	public class AppraisalOtherStrategy : BaseStrategy {
		public AppraisalOtherStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}

		protected override bool Check(AliceRequest request, State state) {
            return request.HasIntent(Intents.AppraisalOther);
		}

        protected override async Task<SimpleResponse> Respond(AliceRequest request, State state) {
			//state.Clear(); пока не будет очищать

			return await GetMessage(request, state);
		}

		public override async Task<SimpleResponse> GetMessage(AliceRequest request, State state) {
			var nextAction = GetNextStrategy();
			return await nextAction.GetMessage(request, state);
		}

		public override SimpleResponse GetHelp() {
			var nextAction = GetNextStrategy();
			return nextAction.GetHelp();
		}

		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state) {
			var nextAction = GetNextStrategy();
			return nextAction.GetMessageForUnknown(request, state);
		}
	}
}
