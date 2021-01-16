using AliceAppraisal.Controllers;
using AliceAppraisal.Engine;
using AliceAppraisal.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AliceUnitTests.Mocks {
	public class MainHandlerMock : MainHandler {
		public MainHandlerMock(AliceRequest request) : base(request) {
		}

		public void SetServiceFactory(IServiceFactory serviceFactory) {
			ReInit(serviceFactory);
		}
	}
}
