using System;
using System.Collections.Generic;
using System.Text;

namespace AliceAppraisal.Models {
	public class AppraisalQuoteRequest {


		public int? MakeId { get;set;}
		public string MakeToken { get; set; }

		public int? ModelId {get;set;}
		public int? ManufactureYear {get;set;}


		/// <summary>
		/// https://automama.ru/api/v2/taxonomy/generations?modelId=956&manufactureYear=2017
		/// { text: VI value: 123 }
		/// </summary>
		public int? GenerationId { get; set; }
		public string EngineType { get; set; }
		public int? HorsePower { get; set; }
		public string BodyType { get; set; }


		public string Gearbox { get; set; }
		public int? Run { get; set; }
		public string Drive { get; set; }
		public int? RegionId { get; set; }
		public string EquipmentType { get; set; }




		public void ResetModelId() {
			ModelId = null;
		}
		public void ResetGenerationId() {
			GenerationId = null;
		}
	}
}
