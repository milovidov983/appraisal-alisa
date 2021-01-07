using AliceAppraisal.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AliceAppraisal.Static {
    public static class Phrases {
       

        public static readonly SimpleResponse Hi = new SimpleResponse(
            @"Хотите узнать цену у автомобиля?",
            new[] { "Начать оценку", "Помощь", "Выход" }
            );
   

        public static readonly SimpleResponse Exit = new SimpleResponse("До свидания.");

     

    }
}
