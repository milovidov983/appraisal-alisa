using AliceAppraisal.Application.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace AliceAppraisal.Application.Infrastructure.Models {
	public class TelegramBotConfig {
		public string SpreadsheetId { get; set; }
		public string ClientId { get; set; }
		public string ClientSecret { get; set; }
		public string User { get; set; }


		public TelegramBotConfig(Settings settings) {
		}
	
	}
}
