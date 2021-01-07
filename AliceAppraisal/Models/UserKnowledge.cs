using System;
using System.Collections.Generic;
using System.Text;

namespace AliceAppraisal.Models {
	public class UserKnowledge {
		public Dictionary<int, DataSet> Tarainings { get; set; } = new Dictionary<int, DataSet> {
			[1] = new DataSet {
				Id = 1,
				CompleteCounter = 0,
				Text = "Что бы узнать цену такого же авто которое вы только что оценили но с иным пробегом, " +
				"можно сказать: а оцени такое же авто но с пробегом 50000 км."
			}
		};
	}
	public class DataSet {
		public int Id { get; set; }
		public string Text { get; set; }
		public int CompleteCounter { get; set; }
	}
}
