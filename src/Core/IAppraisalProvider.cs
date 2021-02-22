using AliceAppraisal.Models;
using System.Threading.Tasks;

namespace AliceAppraisal.Core.Engine {
	public interface IAppraisalProvider {
		Task<TextAndValue[]> GetGenerationsFor(int modelId, int manufactureYear);
		Task<AppraisalRawResult> GetAppraisalResponse(AppraisalQuoteRequest appraisalRequest);
	}
}
