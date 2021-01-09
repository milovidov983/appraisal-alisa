using AliceAppraisal.Engine.Services;
using AliceAppraisal.Models;
using AliceAppraisal.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Strategy {
	public abstract class BaseStrategy {
        protected readonly ITextGeneratorService textGeneratorService;
        protected readonly IExternalService externalService;
        private readonly IStrategyFactory strategyFactory;

        protected BaseStrategy(IServiceFactory serviceFactory) {
            textGeneratorService = serviceFactory.GetTextGeneratorService();
            strategyFactory = serviceFactory.GetStrategyFactory();
            externalService = serviceFactory.GetExternalService();
        }

        public string NextStep { get => Transitions.GetNextStep(this); }

        protected BaseStrategy GetNextStrategy(string customNextStep = null) {
            return strategyFactory.GetStrategy(customNextStep ?? NextStep);
		}

        public bool IsSuitableStrategy(AliceRequest request, State state) {
            return Check(request, state ?? new State());
        }

        public async Task<AliceResponse> Run(AliceRequest request, State state) {
            return await CreateResponse(request, state);
        }

        protected virtual async Task<AliceResponse> CreateResponse(AliceRequest request, State state) {
            state.SaveCurrentStep(this);
            var simple = await Respond(request, state);

            var response = AliceResponseBuilder.Create()
                .WithData(request)
                .WithState(state)
                .WithText(simple)
                .Build();

            return response;
        }


        protected abstract bool Check(AliceRequest request, State state);
        protected abstract Task<SimpleResponse> Respond(AliceRequest request, State state);

        public abstract Task<SimpleResponse> GetMessage(AliceRequest request, State state);
        public abstract SimpleResponse GetHelp();
        public abstract SimpleResponse GetMessageForUnknown(AliceRequest request, State state);
    }
}
