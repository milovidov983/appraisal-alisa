using AliceAppraisal.Engine.Services;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Strategy {
	public class GetModelStrategy : BaseStrategy {
		public GetModelStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}

		public override SimpleResponse GetHelp() {
			return new SimpleResponse {
				Text = $"Для оценки автомобиля мне необходимо знать его модель, если мне не удается распознать " +
					$"название модели которое вы говорите, то возможно этой модели у меня просто нету в базе. " +
					$"Попробуйте произнести название приблизив микрофон ближе."
			};
		}

		public override async Task<SimpleResponse> GetMessage(AliceRequest request, State state) {
			await Task.Yield();
			var randGiveWord = WordsCollection.GET_VERB.GetRand();
			return new SimpleResponse {
				Text = $"{randGiveWord} пожалуйста модель вашего автомобиля?"
			};
		}

		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state) {
			return new SimpleResponse {
				Text = $"Не удалось распознать модель вашего авто," +
				$" попробуйте повторить ваш запрос или попросите у меня подсказку."
			};
		}

		protected override bool Check(AliceRequest request, State state) {
			return request.HasIntent(Intents.ModelName) && state.NextAction.Is(this.GetType());
		}

		protected override async Task<SimpleResponse> Respond(AliceRequest request, State state) {
			var value = request.GetSlot(Intents.ModelName, Slots.Model);

			if (value.IsNullOrEmpty()) {
				return await GetMessageForUnknown(request, state).FromTask();
			}

			if (value.IsNullOrEmpty()) {
				throw new ArgumentException($"Не удалось извлечь ID модели из сущности {value}");
			}
			var modelMap = await GetModelMap();
			var isSimilarSoundModelName = modelMap.SimilarModelNames
											.TryGetValue(value, out var similarModelNames);

			int newModelId;
			if (isSimilarSoundModelName) {
				var legalModels = modelMap.MakeModels[state.Request.MakeId
					?? throw new ArgumentException($"Не указана марка")];

				newModelId = similarModelNames.FirstOrDefault(x => legalModels.Any(y => y == x));

				if(newModelId == default) {
					throw new ArgumentException($"Не удалось найти модели из похожих {value}");
				}
			} else {
				newModelId = value.ExtractId()
					?? throw new ArgumentException($"Не удалось извлечь ID модели из сущности {value}");
			}

			state.UpdateModelId(newModelId, value);

			return await CreateNextStepMessage(request, state);

		}

		private async Task<ModelMaps> GetModelMap() {
			using var client = new WebClient();
			var stream =  client.OpenRead(
				"https://raw.githubusercontent.com/milovidov983/PublicData/master/modelsmap.json"
				);
			var reader = new StreamReader(stream);
			var content = await reader.ReadToEndAsync();
			var res = JsonConvert.DeserializeObject<ModelMaps>(content);
			return res;
		}
	}
}
