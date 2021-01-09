using AliceAppraisal.Engine.Strategy;
using AliceAppraisal.Models;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine {
	public interface ITextGeneratorService {

		string GetRandTakeVerb();
	}
}