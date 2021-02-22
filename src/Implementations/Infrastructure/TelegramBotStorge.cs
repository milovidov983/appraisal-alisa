using AliceAppraisal.Application.Infrastructure.Models;
using AliceAppraisal.Infrastructure;
using System.Threading.Tasks;

namespace AliceAppraisal.Implementations.Infrastructure {


	public class TelegramBotStorge : IStorageService {
		
		public TelegramBotStorge(TelegramBotConfig config) {
			
		}

		public Task Insert<T>(T data) {
			//var json = JsonConvert.SerializeObject(data);
			return Task.FromResult<object>(null);//db.CreateItem(json);
		}
	}
}
