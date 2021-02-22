using System.Threading.Tasks;

namespace AliceAppraisal {
	class Program {
		static async Task Main(string[] args) {
			await Task.Yield();
		}
	}
}
