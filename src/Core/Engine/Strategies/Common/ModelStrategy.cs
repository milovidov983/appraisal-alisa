﻿using AliceAppraisal.Core.Engine.Services;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Threading.Tasks;
using AliceAppraisal.Core.Models;

namespace AliceAppraisal.Core.Engine.Strategy {
	public class ModelStrategy : BaseStrategy {
		private readonly IVehicleModelService modelService;
		private readonly IDataProvider dataService;

		public ModelStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
			this.modelService = serviceFactory.GetVehicleModelService();
			this.dataService = serviceFactory.GetDataProvider();
		}

		public override SimpleResponse GetHelp() {
			return new SimpleResponse {
				Text = $"Для оценки автомобиля мне необходимо знать его модель, если мне не удается распознать " +
					$"возможно Вы произнесите модель с посторонними словами и мне не удается выделить среди них модель. " +
					$"Попробуйте произнести название модели отдельным словом без названия поколения Вашего автомобиля."	
			};
		}

		public override async Task<SimpleResponse> GetMessage(AliceRequest request, State state) {
			var randGiveWord = WordsCollection.GET_VERB.GetRand();
			var selectedMakeId = request.State.Session.Request.MakeId;
			var modelsNames = request.HasScreen() && selectedMakeId.HasValue
				? await dataService.GetPupularModels(selectedMakeId.Value)
				: Array.Empty<string>();


			var additionalText = request.HasScreen()
				? "Или выберите из наиболее популярных моделей."
				: "";

			return new SimpleResponse {
				Text = $"{randGiveWord} пожалуйста модель вашего автомобиля. {additionalText}",
				Buttons = modelsNames
			};
		}

		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state) {
			return new SimpleResponse {
				Text = GetMessageForUnknownText()
			};
		}

		public static string GetMessageForUnknownText() {
			return $"Что бы оценить авто мне надо знать его модель, " +
				$"пожалуйста назовите только модель Вашего авто, без указания его поколения и года выпуска " +
				$"или попросите у меня подсказку.";
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
