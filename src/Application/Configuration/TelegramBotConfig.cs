using AliceAppraisal.Application.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace AliceAppraisal.Application.Infrastructure.Models {
	public class TelegramBotConfig {
		public string Token { get; set; }
		public string ChatId { get; set; }

		public TelegramBotConfig() { }

		public TelegramBotConfig(Settings settings) {
			Token = settings.TelegramBotToken;
			ChatId = settings.TelegramChatId;
		}
	
	}
}
