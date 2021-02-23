using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AliceAppraisal.Core.Engine.Strategy {
	public class MakeModelYearStratagy : BaseStrategy {
		private readonly IVehicleModelService modelService;
		private readonly IManufactureYearService yearService;

		public MakeModelYearStratagy(IServiceFactory serviceFactory) : base(serviceFactory) {
			this.modelService = serviceFactory.GetVehicleModelService();
			this.yearService = serviceFactory.GetManufactureYearService();
		}

		protected override bool Check(AliceRequest request, State state) 
			=> (
				request.HasIntent(Intents.FirstComplexRequest)
				&& 
				(
					state.NextAction.Is(typeof(MakeStrategy)) 
					|| 
					state.NextAction.Is(typeof(InitStrategy))
				)
			);
		

		protected override async Task<SimpleResponse> Respond(AliceRequest request, State state) {
			var makeValue = request.GetSlot(Intents.FirstComplexRequest, Slots.Make);
			var modelValue = request.GetSlot(Intents.FirstComplexRequest, Slots.Model);
			var yearValue = request.GetSlot(Intents.FirstComplexRequest, Slots.ManufacureYear);

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

			string nextStep = null;
			var manufactureYear = yearService.GetYearFromUserInputOrNull(yearValue);
			if (manufactureYear != null) {
				state.UpdateManufactureYear(manufactureYear.Value);
				nextStep = typeof(GenerationStrategy).FullName;
			}

			state.UpdateMake(makeId, makeValue);
			state.UpdateModelId(modelId, name);

			return await CreateNextStepMessage(request, state, nextStep);
		}

		public override async Task<SimpleResponse> GetMessage(AliceRequest request, State state) {
			await Task.Yield();
			return new SimpleResponse {
				Text = MakeStrategy.Messages.GetRand()
			};
		}

		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state)
			=> new SimpleResponse {
				Text = MakeStrategy.GetMessageForUnknownText()
			};
		

		public override SimpleResponse GetHelp() 
			=> new SimpleResponse {
				Text = MakeStrategy.GetHelpText()
			};
		
	}
}