using AliceAppraisal.Models;
using System.Threading.Tasks;

namespace AliceAppraisal.Core.Engine {
	public interface IDataProvider {
		Task<TextWithValue<int>[]> GetGenerationsFor(int modelId, int manufactureYear);
		Task<AppraisalRawResult> GetAppraisalResponse(AppraisalQuoteRequest appraisalRequest);
		Task<string[]> GetPupularModels(int makeId);
		Task<string[]> GetPupularMakes();
		Task<Core.Models.AvailableCharacteristics> GerAvailableCharacteristics(int generationId);
	}
}
