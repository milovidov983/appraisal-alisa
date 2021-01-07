using AliceAppraisal.Engine.Services;
using AliceAppraisal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Stratagy {
	public abstract class BaseStratagy {
        protected ITextGeneratorService textGeneratorService;

        protected BaseStratagy(IServiceFactory serviceFactory) {
            textGeneratorService = serviceFactory.GetTextGeneratorService();
        }

        public bool IsSuitableStrategy(AliceRequest request, State state) {
            return Check(request, state ?? new State());
        }

        public async Task<AliceResponse> Run(AliceRequest request, State state) {
            return await CreateResponse(request, state);
        }

        protected virtual async Task<AliceResponse> CreateResponse(AliceRequest request, State state) {
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

    }
}
