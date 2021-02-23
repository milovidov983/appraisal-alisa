using System.Threading.Tasks;

namespace AliceAppraisal.Core.Engine {
	public interface IVehicleModelService {
		Task<(int modelId, string name)> GetModelData(string modelValue, int makeId, string makeName, string command);
	}
}
