using AliceAppraisal.Configuration;
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
			var (similarModel, modelsIdsForCurrentMake) = await GetModelMap(
				state.Request.MakeId 
				?? throw new ArgumentException($"Прежде чем выбрать модель необходимо указать марку авто."));

			/// Так как существуют модели с схожими названиями например Мазда 5 Бмв 5
			/// необходимо убедится что указанная пользователем модель является такого типа названием
			var isSimilarSoundModelName = similarModel.SimilarModelNames.TryGetValue(value, out var similarModelNameIds);

			int newModelId;
			if (isSimilarSoundModelName) {
				newModelId = similarModelNameIds.FirstOrDefault(x => modelsIdsForCurrentMake.Contains(x));

				if (newModelId == default) {
					throw new ArgumentException($"Не удалось найти модели из похожих {value}");
				}
			} else {
				newModelId = value.ExtractId()
					?? throw new ArgumentException($"Не удалось извлечь ID модели из сущности {value}");

				var isFitModel = modelsIdsForCurrentMake.Contains(newModelId);
				if (!isFitModel) {
					throw new ArgumentException($"Выбранная модель {value.ExtractName()} не принадлежит к марке {state.Request.MakeEntity}");
				}
			}

			state.UpdateModelId(newModelId, value);

			return await CreateNextStepMessage(request, state);

		}

		private async Task<(SimilarNames, HashSet<int> MakeModelsMap)> GetModelMap(int makeId) {
			using var client = new WebClient();
			var similarStream = client.OpenRead(Settings.Instance.SimilarNamesFullUrl);
			var similarReader = new StreamReader(similarStream);
			var similarContentTask = similarReader.ReadToEndAsync();

			var makeModelMapFileName = $"{makeId}.json";
			var makeModelMapStream = client.OpenRead(
				$"{Settings.Instance.MakeModelMapPartUrl}{makeModelMapFileName}"
				);
			var makeModelMapReader = new StreamReader(makeModelMapStream);
			var makeModelMapContentTask = makeModelMapReader.ReadToEndAsync();

			await Task.WhenAll(similarContentTask, makeModelMapContentTask);


			var similarContent = similarContentTask.Result;
			var makeModelMapContent = makeModelMapContentTask.Result;

			var similarNames = JsonConvert.DeserializeObject<SimilarNames>(similarContent);
			var makeModelMapNames = JsonConvert.DeserializeObject<MakeModelsMap>(makeModelMapContent)
										.ModelIds
										.ToHashSet();


			return (similarNames, makeModelMapNames);
		}
	}
}
