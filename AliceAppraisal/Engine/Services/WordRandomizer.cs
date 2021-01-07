using System;
using System.Collections.Generic;
using System.Text;

namespace AliceAppraisal.Engine.Services {
	public class WordRandomizer {
		private string[] GET_VERB = new[] {
			"Назовите",
			"Подскажите",
			"Укажите",
			"Скажите"
		};
		/// <summary>
		/// Синонимы "назовите" или "укажите"
		/// </summary>
		/// <returns></returns>
		public string Get() {
			var rand = new Random((int)DateTime.UtcNow.Ticks);

			var index = rand.Next(0, GET_VERB.Length - 1);

			return GET_VERB[index];

		}
	}
}
