using AliceAppraisal.Engine;
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

    public class Request {
        [JsonPropertyName("command")]
        public string Command { get; set; }

        [JsonPropertyName("original_utterance")]
        public string OriginalUtterance { get; set; }
        
        [JsonPropertyName("type")]
        public string Type { get; set; }
        
        [JsonPropertyName("markup")]
        public Markup Markup { get; set; }
        
        [JsonPropertyName("payload")]
        public Dictionary<string, string> Payload { get; set; }
        
        [JsonPropertyName("nlu")]
        public Nlu Nlu { get; set; }
    }

    public class Session {
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
    }

    public class SessionState {

        [JsonPropertyName("session")]
		public State Session { get; set; }
	}

    public class AliceRequest {
        [JsonPropertyName("meta")]
        public Meta Meta { get; set; }

        [JsonPropertyName("account_linking_complete_event")]
        public AliceEmpty AccountLinkingCompleteEvent { get; set; }

        [JsonPropertyName("request")]
        public Request Request { get; set; }

        [JsonPropertyName("session")]
        public Session Session { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }

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
