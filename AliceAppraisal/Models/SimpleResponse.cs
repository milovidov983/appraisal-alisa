using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AliceAppraisal.Models {
    public class SimpleResponse {
        public string Text { get; set; }
        public string Tts { get; set; }
        public string[] Buttons { get; set; } = Static.Buttons.Base;

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
    }


}
