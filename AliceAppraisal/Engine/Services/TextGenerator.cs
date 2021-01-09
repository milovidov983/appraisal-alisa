using AliceAppraisal.Engine.Services;
using AliceAppraisal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliceAppraisal.Engine.Strategy {
	public class TextGenerator : ITextGeneratorService {
	
		private readonly WordRandomizer randWords = new WordRandomizer();

		public string GetRandTakeVerb() {
			return randWords.Get();
		}
	}
}