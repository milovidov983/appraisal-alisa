using System.Threading.Tasks;

namespace AliceAppraisal.Infrastructure {
	public interface IStorageService {
		Task Insert<T>(T data);
	}
}