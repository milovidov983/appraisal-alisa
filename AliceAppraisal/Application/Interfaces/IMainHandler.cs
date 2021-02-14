using AliceAppraisal.Models;
using System;
using System.Threading.Tasks;

namespace AliceAppraisal.Application {
	public interface IMainHandler {
		Task<(AliceResponse, Exception)> HandleRequest(AliceRequest aliceRequest);
	}
}