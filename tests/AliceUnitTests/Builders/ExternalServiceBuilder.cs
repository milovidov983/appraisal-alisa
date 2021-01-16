using AliceAppraisal.Engine;
using AliceAppraisal.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AliceUnitTests.Builders {
	public class ExternalServiceBuilder {
		private TextAndValue[] generationResponse = Array.Empty<TextAndValue>();


		public static ExternalServiceBuilder Create() {
			return new ExternalServiceBuilder();
		}

		public ExternalServiceBuilder WithTwoGeneration() {
			generationResponse = new[] {
					new TextAndValue(){
						Text = "Поколение 1",
						Value = 123
					},
					new TextAndValue(){
						Text = "Поколение 2",
						Value = 456
					} };
			return this;
		}

		public IExternalService Build() {
			var externalServiceMock = Mock.Of<IExternalService>(
				s => s.GetAppraisalResponse(It.IsAny<AppraisalQuoteRequest>()) == Task.FromResult(new AppraisalRawResult())
				&&
				s.GetGenerationsFor(It.IsAny<int>(), It.IsAny<int>()) == Task.FromResult(generationResponse)
				&&
				s.Token == "Mock");

			return externalServiceMock;
		}
	}
}
