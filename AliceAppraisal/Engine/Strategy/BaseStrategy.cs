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
        protected readonly IServiceFactory serviceFactory;
		protected readonly IExternalService externalService;
       

        protected BaseStrategy(IServiceFactory serviceFactory) {
            this.serviceFactory = serviceFactory;
            this.externalService = serviceFactory.GetExternalService();
        }

        public string NextStep { get => Transitions.GetNextStep(this); }

        protected BaseStrategy GetNextStrategy(string customNextStep = null) {
            var strategyFactory = serviceFactory.GetStrategyFactory();
            return strategyFactory.GetStrategy(customNextStep ?? NextStep);
		}

        public bool IsSuitableStrategy(AliceRequest request, State state) {
            return Check(request, state ?? new State());
        }

        public async Task<AliceResponse> Run(AliceRequest request, State state) {
            return await CreateResponse(request, state);
        }

        protected virtual async Task<AliceResponse> CreateResponse(AliceRequest request, State state) {
            var simple = await Respond(request, state);
            SetCurrentStep(state);

            var response = AliceResponseBuilder.Create()
                .WithData(request)
                .WithState(state)
                .WithText(simple)
                .Build();

            return response;
        }

        protected virtual void SetCurrentStep(State state) {
            state.SaveCurrentStep(this);
		}

        protected abstract bool Check(AliceRequest request, State state);
        protected abstract Task<SimpleResponse> Respond(AliceRequest request, State state);

        public abstract Task<SimpleResponse> GetMessage(AliceRequest request, State state);
        public abstract SimpleResponse GetHelp();
        public abstract SimpleResponse GetMessageForUnknown(AliceRequest request, State state);
    }
}
