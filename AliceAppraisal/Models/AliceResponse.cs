using AliceAppraisal.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AliceAppraisal.Models {
    public class Button {
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("payload")]
        public Dictionary<string, string> Payload { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
        [JsonPropertyName("hide")]
        public bool Hide { get; set; } = true;
    }

    public class Response {
        [JsonPropertyName("text")]
        public string Text { get; set; }
        [JsonPropertyName("tts")]
        public string Tts { get; set; }
        [JsonPropertyName("buttons")]
        public List<Button> Buttons { get; set; }
        [JsonPropertyName("end_session")]
        public bool EndSession { get; set; }
    }

    public class AliceResponse {
        [JsonPropertyName("start_account_linking")]
        public AliceEmpty StartAccountLinking { get; set; }

        [JsonPropertyName("response")]
        public Response Response { get; set; } = new Response();

        [JsonPropertyName("session")]
        public Session Session { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; } = "1.0";

        [JsonPropertyName("session_state")]
        public State State { get; set; }

        public AliceResponse(AliceRequest request) {
            Session = request.Session;
            Version = request.Version ?? "1.0";
        }

        public AliceResponse ToAuthorizationResponse() {
            Response = null;
            StartAccountLinking = new AliceEmpty();
            return this;
        }

        public AliceResponse ToPong() {
            Response = new Response {
                Text = "pong"
            };
            return this;
        }

        public string ToJson() {
            return JsonSerializer.Serialize(this, Settings.JsonOptions).Replace(@"\", "");
        }

        public static AliceResponse CreateMissResponse() {
            return new AliceResponse(new AliceRequest()) {
                Response = new Response {
                    Text = "У меня нет обработчика этого случая. " +
                        "Я не понимаю что вы мне говорите. Приносим извинения.",
                    EndSession = false
                }
            };
        }
    }
}
