using AliceAppraisal.Models;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Stratagy {
	public interface ITextGeneratorService {
		SimpleResponse CreateNextTextRequest(BaseStratagy currentStratagy);
		Task<SimpleResponse> CreateNextTextRequest(BaseStratagy currentStratagy, State state);
		Task<SimpleResponse> CreateFinalResult(State state);
		SimpleResponse CreateAnsverForUnexpectedCommand();
	}
}