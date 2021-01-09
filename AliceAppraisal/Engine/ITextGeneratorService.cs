using AliceAppraisal.Engine.Strategy;
using AliceAppraisal.Models;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine {
	public interface ITextGeneratorService {
		SimpleResponse CreateNextTextRequest(BaseStrategy currentStratagy);
		Task<SimpleResponse> CreateNextTextRequest(BaseStrategy currentStratagy, State state);
		Task<SimpleResponse> CreateFinalResult(State state);
		SimpleResponse CreateAnsverForUnexpectedCommand(State state);
	}
}