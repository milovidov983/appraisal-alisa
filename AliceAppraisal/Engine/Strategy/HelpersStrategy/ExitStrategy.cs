﻿using AliceAppraisal.Engine.Services;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Strategy {
	public class ExitStrategy : BaseStrategy {
		public ExitStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}

		protected override bool Check(AliceRequest request, State state) {
            return request.HasIntent(Intents.Exit);
		}
	
        protected override async Task<AliceResponse> CreateResponse(AliceRequest request, State state) {
            var resp = await base.CreateResponse(request, state);
            resp.Response.EndSession = true;
            return resp;
        }

        protected override Task<SimpleResponse> Respond(AliceRequest request, State state) {
            state.Clear();
            return Task.FromResult( new SimpleResponse {
                Text = "Выхожу. Хорошего дня.",
                Tts = "Выхожу - - хорошего дня"
            });
        }
    }
}