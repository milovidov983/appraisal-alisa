using AliceAppraisal.Application.Infrastructure.Models;
using AliceAppraisal.Infrastructure;
using Serilog;
using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace AliceAppraisal.Implementations.Infrastructure {
	public sealed class  TelegramBotStorge : IStorageService, IDisposable {
		private readonly TelegramBotConfig config;
		private readonly TelegramBotClient botClient;
		private readonly ILogger logger;


		public TelegramBotStorge(TelegramBotConfig config, ILogger logger) {
			this.logger = logger;
			try {
				this.config = config;
				botClient = new TelegramBotClient(config.Token);

				var me = botClient.GetMeAsync().Result;
				logger.Information(
				  $"Телеграм бот инициализирован id: {me.Id} name is {me.FirstName}."
				);
				
				botClient.StartReceiving();
			} catch(Exception e) {
				logger.Error(e, $"Ошибка при инициализации телеграм бота {e.Message}");
			}
		}

		public async Task Insert<T>(T data) {
			try {
				var json = data.ToJson();
				await Send(json);
			} catch(Exception e) {
				logger.Error(e, $"Ошибка при отправке данных боту {e.Message}");
			}
		}
		public void Dispose() {
			botClient?.StopReceiving();
		}
		private async Task Send(string data) {
			await botClient.SendTextMessageAsync(
				chatId: config.ChatId,
				text: data
			);

		}

	}
}
