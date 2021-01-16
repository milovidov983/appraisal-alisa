using AliceAppraisal.Engine.Strategy;
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
		private Request request;


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
		private Request CreateRequest() {
			return new Request {
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

		public RequestBuilder WithIntentMake() {
			var command = "пежо";
			request.Command = command;
			request.OriginalUtterance = command;
			request.Nlu.Intents["make_name"] = new IntentSlot() {
				Slots = new Dictionary<string, Entity> {
					["make"] = new Entity {
						Tokens = new Tokens { End = 1, Start = 0},
						Type = "EMakes",
						Value = "peugeot_135"
					}
				}
			};
			request.Nlu.Tokens.Add(command);
			return this;
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
		
		public RequestBuilder WithIntentManufactureYear() {
			var command = "2012";
			request.Command = command;
			request.OriginalUtterance = command;
			request.Nlu.Intents["digit_input"] = new IntentSlot() {
				Slots = new Dictionary<string, Entity> {
					["number"] = new Entity {
						Tokens = new Tokens { End = 1, Start = 0 },
						Type = "YANDEX.NUMBER",
						Value = 2012
					}
				}
			};
			request.Nlu.Tokens.Add(command);
			return this;
		}

		public RequestBuilder WithModelId(int id = 123) {
			state.Session.Request.ModelId = id;
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
	}
}
