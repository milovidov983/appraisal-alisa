using AliceAppraisal.Core.Engine;
using AliceAppraisal.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AliceUnitTests.Builders {
	public class ExternalServiceBuilder {
		private TextWithValue<int>[] generationResponse = Array.Empty<TextWithValue<int>>();


		public static ExternalServiceBuilder Create() {
			return new ExternalServiceBuilder();
		}

		public ExternalServiceBuilder WithTwoGeneration() {
			generationResponse = new[] {
					new TextWithValue<int>(){
						Text = "Поколение 1",
						Value = 123
					},
					new TextWithValue<int>(){
						Text = "Поколение 2",
						Value = 456
					} };
			return this;
		}

		public IAppraisalProvider Build() {
			var externalServiceMock = Mock.Of<IAppraisalProvider>(
				s => s.GetAppraisalResponse(It.IsAny<AppraisalQuoteRequest>()) == Task.FromResult(new AppraisalRawResult())
				&&
				s.GetGenerationsFor(It.IsAny<int>(), It.IsAny<int>()) == Task.FromResult(generationResponse));

			return externalServiceMock;
		}

		public ExternalServiceBuilder WithOneGeneration() {
			generationResponse = new[] {
					new TextWithValue<int>(){
						Text = "Поколение 1",
						Value = 123
					}
			};
			return this;
		}
	}
}
