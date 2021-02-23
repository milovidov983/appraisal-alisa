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

		public override async Task<SimpleResponse> GetMessage(AliceRequest request, State state) {
			await Task.Yield();
			return new SimpleResponse {
				Text = GetMakeStrategy.Messages.GetRand()
			};
		}

		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state)
			=> new SimpleResponse {
				Text = GetMakeStrategy.GetMessageForUnknownText()
			};
		

		public override SimpleResponse GetHelp() 
			=> new SimpleResponse {
				Text = GetMakeStrategy.GetHelpText()
			};
		
	}
}