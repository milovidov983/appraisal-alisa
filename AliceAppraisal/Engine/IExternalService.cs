using AliceAppraisal.Models;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine {
	public interface IExternalService {
		Task<TextAndValue[]> GetGenerationsFor(int modelId, int manufactureYear);
		Task<AppraisalRawResult> GetAppraisalResponse(AppraisalQuoteRequest appraisalRequest);
	}
}
