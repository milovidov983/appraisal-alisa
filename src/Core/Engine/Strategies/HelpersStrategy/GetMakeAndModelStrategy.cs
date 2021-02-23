using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AliceAppraisal.Core.Engine.Strategy {
	public class GetMakeAndModelStrategy : BaseStrategy {
		private readonly IVehicleModelService modelService;
		public GetMakeAndModelStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
			this.modelService = serviceFactory.GetVehicleModelService();
		}

		protected override bool Check(AliceRequest request, State state) 
			=> (
				request.HasIntent(Intents.MakeAndModel)
				&& 
				(
					state.NextAction.Is(typeof(GetMakeStrategy)) 
					|| 
					state.NextAction.Is(typeof(InitStrategy))
				)
			);
		

		protected override async Task<SimpleResponse> Respond(AliceRequest request, State state) {
			var makeValue = request.GetSlot(Intents.MakeAndModel, Slots.Make);
			var modelValue = request.GetSlot(Intents.MakeAndModel, Slots.Model);

			if (makeValue.IsNullOrEmpty() || modelValue.IsNullOrEmpty()) {
				return GetMessageForUnknown(request, state);
			}

			var makeId = makeValue.ExtractId()
				?? throw new ArgumentException($"Не удалось извлечь ID марки из сущности {makeValue}");

			var (modelId, name) = await modelService.GetModelData(
				modelValue, 
				makeId, 
				state.Request.MakeEntity, 
				request.Request.Command.Split(" ").LastOrDefault());

			state.UpdateMake(makeId, makeValue);
			state.UpdateModelId(modelId, name);
			return await CreateNextStepMessage(request, state);
		}


		public static readonly string[] Messages = new[] {
			$"Назовите марку авто, который вы хотите оценить.",
			$"Назовите марку автомобиля, который вы хотите оценить.",
			$"Скажите мне пожалуйста название марки автомобиля, который вы хотите оценить.",
			$"Скажите название марки автомобиля, который вы хотите оценить.",
			$"Укажите марку авто, которое вы хотите оценить"
		};
		

		public override async Task<SimpleResponse> GetMessage(AliceRequest request, State state) {
			await Task.Yield();
			return new SimpleResponse {
				Text = Messages.GetRand()
			};
		}

		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state)
			=> new SimpleResponse {
				Text = $"Что бы оценить авто мне надо знать его марку," +
				$" пожалуйста назовите отдельно марку Вашего авто или попросите у меня подсказку."
			};
		

		public override SimpleResponse GetHelp() 
			=> new SimpleResponse {
				Text = $"Для оценки автомобиля мне необходимо знать его марку, если мне не удается распознать " +
				$"название марки которое вы говорите, попробуйте указать её название отдельным словом без указания модели года или поколения. "
			};
		
	}
}