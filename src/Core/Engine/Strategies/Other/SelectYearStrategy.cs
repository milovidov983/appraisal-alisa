using AliceAppraisal.Core.Engine.Services;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Core.Engine.Strategy {
	/// <summary>
	/// Позволяет изменить год выпуска авто
	/// </summary>
	public class SelectYearStrategy : BaseStrategy {
		public SelectYearStrategy(IServiceFactory serviceFactory) : base(serviceFactory) {
		}

		protected override bool Check(AliceRequest request, State state) 
			=> request.HasIntent(Intents.SelectYear) 
				&& state.Request.MakeId.HasValue 
				&& state.Request.ModelId.HasValue
				&& 
				( 
					state.NextAction.Is(typeof(GenerationStrategy))
					||
					state.NextAction.Is(typeof(ManufactureYearStrategy))
				);
		

		protected override Task<SimpleResponse> Respond(AliceRequest request, State state) 
			=> GetMessage(request, state);

		public override  Task<SimpleResponse> GetMessage(AliceRequest request, State state) 
			=> CreateNextStepMessage(request, state);
		
		public override SimpleResponse GetHelp() 
			=> CreateNextStepHelp();
		
		public override SimpleResponse GetMessageForUnknown(AliceRequest request, State state) 
			=> CreateNextStepMessageForUnknown(request, state);
	}
}
