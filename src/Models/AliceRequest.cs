using AliceAppraisal.Core.Engine;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace AliceAppraisal.Models {
	public class AliceEmpty {
	}

	public class Interfaces {
		[JsonPropertyName("screen")]
		public AliceEmpty Screen { get; set; }

		[JsonPropertyName("payments")]
		public AliceEmpty Payments { get; set; }

		[JsonPropertyName("account_linking")]
		public AliceEmpty AccountLinking { get; set; }
	}

	public class Meta {
		[JsonPropertyName("locale")]
		public string Locale { get; set; }

		[JsonPropertyName("timezone")]
		public string Timezone { get; set; }

		[JsonPropertyName("client_id")]
		public string ClientId { get; set; }

		[JsonPropertyName("interfaces")]
		public Interfaces Interfaces { get; set; }
	}

	public class Markup {
		/// <summary>
		/// Признак реплики, которая содержит криминальный подтекст 
		/// (самоубийство, разжигание ненависти, угрозы).
		/// Вы можете настроить навык на определенную реакцию 
		/// для таких случаев — например, отвечать «Не понимаю, 
		/// о чем вы. Пожалуйста, переформулируйте вопрос.»
		/// Возможно только значение true. Если признак
		/// не применим, это свойство не включается в ответ.
		/// </summary>
		[JsonPropertyName("dangerous_context")]
		public bool DangerousContext { get; set; }
	}

	public class Tokens {
		[JsonPropertyName("start")]
		public int Start { get; set; }

		[JsonPropertyName("end")]
		public int End { get; set; }
	}

	public class Entity {
		[JsonPropertyName("token")]
		public Tokens Tokens { get; set; }

		[JsonPropertyName("type")]
		public string Type { get; set; }

		[JsonPropertyName("value")]
		public object Value { get; set; }
	}

	public class IntentSlot {
		[JsonPropertyName("slots")]
		public Dictionary<string, Entity> Slots { get; set; }
	}

	public class Nlu {
		[JsonPropertyName("tokens")]
		public List<string> Tokens { get; set; }

		[JsonPropertyName("entities")]
		public List<Entity> Entities { get; set; }

		[JsonPropertyName("intents")]
		public Dictionary<string, IntentSlot> Intents { get; set; }
	}

	public class RequestInfo {
		/// <summary>
		/// Служебное поле: запрос пользователя, преобразованный для внутренней 
		/// обработки Алисы. В ходе преобразования текст, в частности, 
		/// очищается от знаков препинания, а числительные преобразуются в числа.
		/// Чтобы получить точный текст запроса, используйте 
		/// свойство original_utterance.
		/// </summary>
		[JsonPropertyName("command")]
		public string Command { get; set; }
		/// <summary>
		/// Полный текст пользовательского запроса, максимум 1024 символа.
		/// </summary>
		[JsonPropertyName("original_utterance")]
		public string OriginalUtterance { get; set; }
		/// <summary>
		/// Тип ввода, обязательное свойство. Возможные значения:
		/// "SimpleUtterance" — голосовой ввод;
		/// "ButtonPressed" — нажатие кнопки.
		/// </summary>
		[JsonPropertyName("type")]
		public string Type { get; set; }
		/// <summary>
		/// Формальные характеристики реплики, которые удалось выделить Яндекс.Диалогам. 
		/// Отсутствует, если ни одно из вложенных свойств не применимо.
		/// </summary>
		[JsonPropertyName("markup")]
		public Markup Markup { get; set; }

		[JsonPropertyName("payload")]
		public Dictionary<string, string> Payload { get; set; }

		[JsonPropertyName("nlu")]
		public Nlu Nlu { get; set; }
	}

	public class Session {
		/// <summary>
		/// Признак новой сессии. Возможные значения:
		/// true — пользователь начинает новый разговор с навыком;
		/// false — запрос отправлен в рамках уже начатого разговора.
		/// </summary>
		[JsonPropertyName("new")]
		public bool New { get; set; }

		[JsonPropertyName("message_id")]
		public int MessageId { get; set; }

		[JsonPropertyName("session_id")]
		public string SessionId { get; set; }

		[JsonPropertyName("skill_id")]
		public string SkillId { get; set; }

		[JsonPropertyName("user_id")]
		public string UserId { get; set; }

		[JsonPropertyName("user")]
		public UserInfo User { get; set; }

		[JsonPropertyName("application")]
		public ApplicationInfo Application { get; set; }

	}

	public class SessionState {

		[JsonPropertyName("session")]
		public State Session { get; set; }
	}

	public class UserInfo {
		/// <summary>
		/// Идентификатор пользователя Яндекса, единый для всех приложений 
		/// и устройств.
		/// Этот идентификатор уникален для пары «пользователь — навык»: 
		/// в разных навыках значение свойства user_id для одного и того
		/// же пользователя будет различаться.
		/// </summary>
		[JsonPropertyName("user_id")]
		public string UserId { get; set; }
	}

	public class ApplicationInfo {
		/// <summary>
		/// Идентификатор экземпляра приложения, в котором пользователь общается 
		/// с Алисой, максимум 64 символа.
		/// Например, даже если пользователь авторизован с одним и тем же аккаунтом
		/// в приложениях Яндекс для Android и iOS, Яндекс.Диалоги присвоят отдельный application_id каждому из этих приложений.
		/// Этот идентификатор уникален для пары «приложение — навык»: 
		/// в разных навыках значение свойства application_id для одного
		/// и того же пользователя будет различаться.
		/// </summary>
		[JsonPropertyName("application_id")]
		public string ApplicationId { get; set; }
	}

	public class AliceRequest {
		[JsonPropertyName("meta")]
		public Meta Meta { get; set; }

		[JsonPropertyName("account_linking_complete_event")]
		public AliceEmpty AccountLinkingCompleteEvent { get; set; }

		[JsonPropertyName("request")]
		public RequestInfo Request { get; set; }

		[JsonPropertyName("session")]
		public Session Session { get; set; }

		[JsonPropertyName("version")]
		public string Version { get; set; } = "1.0";

		[JsonPropertyName("state")]
		public SessionState State { get; set; }

		public bool HasScreen() {
			return Meta?.Interfaces?.Screen != null;
		}

		public bool IsAccountLinking() {
			return AccountLinkingCompleteEvent != null;
		}

		public bool IsPing() {
			if (Request == null) {
				return false;
			}

			return Request.Command?.ToLower() == "ping";
		}

		public IntentSlot GetIntent(string name) {
			if (Request?.Nlu?.Intents == null) {
				return null;
			}

			if (!Request.Nlu.Intents.ContainsKey(name)) {
				return null;
			}

			return Request.Nlu.Intents[name];
		}

		public bool HasIntent(string name, bool withSlots = false) {
			return withSlots ? GetIntent(name)?.Slots != null : GetIntent(name) != null;
		}

		public bool HasSlot(string intentName, string slotName) {
			return !string.IsNullOrEmpty(GetSlot(intentName, slotName));
		}

		public bool HasOneOfSlots(string intentName, params string[] slotNames) {
			return slotNames.Any(s => HasSlot(intentName, s));
		}

		public bool HasAllSlots(string intentName, params string[] slotNames) {
			return slotNames.All(s => HasSlot(intentName, s));
		}

		public string GetSlot(string intentName, string slotName) {
			var intent = GetIntent(intentName);
			if (intent?.Slots == null || !intent.Slots.ContainsKey(slotName)) {
				return "";
			}

			return intent.Slots[slotName].Value.ToString();
		}

		public bool IsOutsideCommand() {
			return Request != null && !Request.Command.IsNullOrEmpty() && Session != null && Session.New;
		}
	}
}
