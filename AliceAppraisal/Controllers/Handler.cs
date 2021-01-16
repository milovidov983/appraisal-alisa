using AliceAppraisal.Application;
using AliceAppraisal.Configuration;
using AliceAppraisal.Engine;
using AliceAppraisal.Engine.Services;
using AliceAppraisal.Engine.Strategy;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AliceAppraisal.Controllers {
	public class Handler {
		public IHandlerFactory HandlerFactory { get; } = new HandlerFactory();

		public async Task<AliceResponse> FunctionHandler(AliceRequest request) {
			if(request is null) {
				return AliceResponse.CreateMissResponse();
			}

			if (request.IsPing()) {
				return new AliceResponse(request).ToPong();
			}

			IMainHandler handler = HandlerFactory.Create();
			return await handler.HandleRequest(request);
		}

	}
}
