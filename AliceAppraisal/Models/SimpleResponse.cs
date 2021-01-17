using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliceAppraisal.Models {
    public class SimpleResponse {
        private string[] _buttons;

        public string Text { get; set; }
        public string Tts { get; set; }
        public string[] Buttons {
			get {
                return (_buttons ?? Array.Empty<string>()).Union(Static.Buttons.BaseSet).ToArray();
			}
            set {
                _buttons = value;
            } 
        }

        public SimpleResponse() { }
        public SimpleResponse(string text, string[] buttons = null) {
            Text = text;
            Buttons = buttons;
        }

        public AliceResponse Generate(AliceRequest request) {
            return new AliceResponse(request) {
                Response =
                {
                    Text = Text,
                    Tts = Tts,
                    Buttons = Buttons?.Select(b => new Button { Title = b }).ToList()
                }
            };
        }

        public static SimpleResponse Empty { get => new SimpleResponse() {
            Text = "[Пустой ответ] (теоретически такое не возможно)"
        }; }

        public Task<SimpleResponse> FromTask() {
            return Task.FromResult(this);
		}
    }
}
