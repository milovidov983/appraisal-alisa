using AliceAppraisal.Models;
using System.Threading.Tasks;

namespace AliceAppraisal.Application {
	public interface IMainHandler {
		Task<AliceResponse> HandleRequest(AliceRequest aliceRequest);
	}
}