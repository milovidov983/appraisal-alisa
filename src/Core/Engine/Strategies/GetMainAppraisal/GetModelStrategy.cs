using AliceAppraisal.Application.Configuration;
using AliceAppraisal.Core.Engine.Services;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AliceAppraisal.Core.Engine.Strategy {
	public class GetModelStrategy : BaseStrategy {
		private readonly IVehicleModelService modelService;

		public GetModelStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
			this.modelService = serviceFactory.GetVehicleModelService();
		}

		public override SimpleResponse GetHelp() {
			return new SimpleResponse {
				Text = $"Для оценки автомобиля мне необходимо знать его модель, если мне не удается распознать " +
					$"возможно Вы произнесите модель с посторонними словами и мне не удается выделить среди них модель. " +
					$"Попробуйте произнести название модели отдельным словом без названия поколения Вашего автомобиля."	
			};
		}

		public override async Task<SimpleResponse> GetMessage(AliceRequest request, State state) {
			await Task.Yield();
			var randGiveWord = WordsCollection.GET_VERB.GetRand();
			return new SimpleResponse {
				Text = $"{randGiveWord} пожалуйста модель вашего автомобиля"
			};
		}

		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state) {
			return new SimpleResponse {
				Text = $"Не удалось распознать модель вашего авто, " +
				$"попробуйте произнести название модели отдельным " +
				$"словом без упоминания её поколения или попросите у меня подсказку."
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

			var makeId = state.Request.MakeId
				?? throw new ArgumentException($"Прежде чем выбрать модель необходимо указать марку авто.");

			var (modelId, name) = await modelService.GetModelData(value, makeId, state.Request.MakeEntity, request.Request.Command);

			state.UpdateModelId(modelId, name);

			return await CreateNextStepMessage(request, state);

		}
	}
}
