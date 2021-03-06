using AliceAppraisal;
using AliceAppraisal.Core.Engine.Strategy;
using AliceAppraisal.Core.Models;
using AliceAppraisal.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AliceUnitTests.BaseCommandTests {
	public class RequestBuilder {
		private AliceRequest aliceRequest;
		private Meta meta;
		private Session session;
		private SessionState state;
		private RequestInfo request;


		private Meta CreateMeta() {
			return new Meta {
				Locale = "ru-RU",
				Timezone = "UTC",
				ClientId = "ru.yandex.searchplugin/7.16 (none none; android 4.4.2)",
				Interfaces = new Interfaces {
					Screen = new AliceEmpty(),
					Payments = new AliceEmpty(),
					AccountLinking = new AliceEmpty()
				}
			};
		}
		private Session CreateSession() {
			return new Session {
				MessageId = 1,
				SessionId = "test_session_id",
				SkillId = "test_skill_id",
				User = new UserInfo {
					UserId = "test_uesr_id_1"
				},
				Application = new ApplicationInfo {
					ApplicationId = "test_id"
				},
				UserId = "test_id",
				New = false

			};
		}



		private SessionState CreateState() {
			return new SessionState {
				Session = new State {
					GenerationChoise = new Dictionary<string, IdAndName>(),
					Request = new AppraisalQuoteRequest { }
				}
			};
		}
		private RequestInfo CreateRequest() {
			return new RequestInfo {
				Command = string.Empty,
				OriginalUtterance = string.Empty,
				Type = "SimpleUtterance",
				Markup = new Markup {
					DangerousContext = false
				},
				Nlu = new Nlu {
					Tokens = new List<string>(),
					Entities = new List<Entity>(),
					Intents = new Dictionary<string, IntentSlot>()
				}
			};
		}
		private RequestBuilder() {
			InitBuilder();
		}
		public static RequestBuilder Create() {
			return new RequestBuilder();
		}

		public AliceRequest Build() {
			try {
				return aliceRequest;
			} finally {
				InitBuilder();
			}
		}

		private void InitBuilder() {
			meta = CreateMeta();
			session = CreateSession();
			state = CreateState();
			request = CreateRequest();
			aliceRequest = new AliceRequest {
				Meta = meta,
				Request = request,
				Session = session,
				State = state,
			};
		}

		public RequestBuilder WithActions(string prev, string next) {
			state.Session.PrevAction = prev;
			state.Session.NextAction = next;
			return this;

		}
		


		public RequestBuilder WithStoredMake(int makeId) {
			state.Session.Request.MakeId = makeId;
			return this;

		}
		public RequestBuilder WithHelp() {
			var helpStr = "помощь";
			request.Command = helpStr;
			request.OriginalUtterance = helpStr;
			request.Nlu.Intents["YANDEX.HELP"] = new IntentSlot();
			request.Nlu.Tokens.Add(helpStr);
			return this;
		}


		public RequestBuilder WithConfirm() {
			var command = "да";
			request.Command = command;
			request.OriginalUtterance = command;
			request.Nlu.Intents["YANDEX.CONFIRM"] = new IntentSlot();
			request.Nlu.Tokens.Add(command);
			return this;
		}

		public RequestBuilder WithIntentMake(string make = null) {
			var command = make ?? "пежо";
			request.Command = command;
			request.OriginalUtterance = command;
			request.Nlu.Intents["make_name"] = new IntentSlot() {
				Slots = new Dictionary<string, Entity> {
					["make"] = new Entity {
						Tokens = new Tokens { End = 1, Start = 0},
						Type = "EMakes",
						Value = make ?? "peugeot_135"
					}
				}
			};
			request.Nlu.Tokens.Add(command);
			
			aliceRequest.State ??= new SessionState {
				Session = new State {
					Request = new AppraisalQuoteRequest {
						MakeId = make.ExtractIdOrNull()
					}
				}
			};
			if(aliceRequest.State.Session is null) {
				aliceRequest.State.Session = new State {
					Request = new AppraisalQuoteRequest {
						MakeId = make.ExtractIdOrNull()
					}
				};
			}
			return this;
		}

		public RequestBuilder WithMakeId(int id) {
			var sessionState = GetState();
			sessionState.Session.Request.MakeId = id;
			return this;
		}



		private SessionState GetState() {
			if (aliceRequest.State is null) {
				aliceRequest.State = new SessionState();

			}
			if (aliceRequest.State.Session is null) {
				aliceRequest.State.Session = new State();

			}
			if(aliceRequest.State.Session.Request is null) {
				aliceRequest.State.Session.Request = new AppraisalQuoteRequest();
			}
			return aliceRequest.State;
		}

		public RequestBuilder WithIntentModel() {
			var command = "астра";
			request.Command = command;
			request.OriginalUtterance = command;
			request.Nlu.Intents["model_name"] = new IntentSlot() {
				Slots = new Dictionary<string, Entity> {
					["model"] = new Entity {
						Tokens = new Tokens { End = 1, Start = 0 },
						Type = "EModels",
						Value = "astra_1427"
					}
				}
			};
			request.Nlu.Tokens.Add(command);
			return this;
		}
		public RequestBuilder WithIntentModelSimilar() {
			var command = "астра";
			request.Command = command;
			request.OriginalUtterance = command;
			request.Nlu.Intents["model_name"] = new IntentSlot() {
				Slots = new Dictionary<string, Entity> {
					["model"] = new Entity {
						Tokens = new Tokens { End = 1, Start = 0 },
						Type = "EModels",
						Value = "five"
					}
				}
			};
			request.Nlu.Tokens.Add(command);
			return this;
		}

		public RequestBuilder WithIntentManufactureYear(int? year = null) {
			var command = $"{year ?? 2012}";
			request.Command = command;
			request.OriginalUtterance = command;
			request.Nlu.Intents["digit_input"] = new IntentSlot() {
				Slots = new Dictionary<string, Entity> {
					["number"] = new Entity {
						Tokens = new Tokens { End = 1, Start = 0 },
						Type = "YANDEX.NUMBER",
						Value = year ?? 2012
					}
				}
			};
			request.Nlu.Tokens.Add(command);
			return this;
		}

		public RequestBuilder WithModelId(int id = 123) {
			state.Session.Request.ModelId = id;
			state.Session.Request.ModelEntity = "test_model";
			return this;
		}

		public RequestBuilder SetNew() {
			session.New = true;
			return this;
		}
		public RequestBuilder WithMessageId(int id) {
			session.MessageId = id;
			return this;
		}

		public RequestBuilder WithGenerationChoise(bool single = false) {
			state.Session.GenerationChoise = new Dictionary<string, IdAndName> {
				["1"] = new IdAndName {
					Id = 18,
					Name = "TestGen"
				} 
			};
			if (!single) {
				state.Session.GenerationChoise.Add(
					"2", 
					new IdAndName { 
						Id = 18, 
						Name = "TestGen2" 
					}
				);
			}

			return this;
		}

		public RequestBuilder WithNumberIntent(int number) {
			var command = $"{number}";
			request.Command = command;
			request.OriginalUtterance = command;
			request.Nlu.Intents["digit_input"] = new IntentSlot() {
				Slots = new Dictionary<string, Entity> {
					["number"] = new Entity {
						Tokens = new Tokens { End = 1, Start = 0 },
						Type = "YANDEX.NUMBER",
						Value = number
					}
				}
			};
			request.Nlu.Tokens.Add(command);
			return this;
		}
	}
}
