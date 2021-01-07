using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace AliceAppraisal.Models {
    public class AnaloguePriceRange {
        [JsonPropertyName("min")]
        public double Min { get; set; }

        [JsonPropertyName("max")]
        public double Max { get; set; }
    }

    public class PriceRange {
        [JsonPropertyName("min")]
        public double Min { get; set; }

        [JsonPropertyName("max")]
        public double Max { get; set; }
    }

    public class SampleByPrices {
        [JsonPropertyName("lowPriced")]
        public int LowPriced { get; set; }

        [JsonPropertyName("normal")]
        public int Normal { get; set; }

        [JsonPropertyName("highPriced")]
        public int HighPriced { get; set; }
    }

    public class AppraisalRawResult {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("analoguePriceRange")]
        public AnaloguePriceRange AnaloguePriceRange { get; set; }

        [JsonPropertyName("priceRange")]
        public PriceRange PriceRange { get; set; }

        [JsonPropertyName("oneDayPrice")]
        public double OneDayPrice { get; set; }

        [JsonPropertyName("oneMonthPrice")]
        public double OneMonthPrice { get; set; }

        [JsonPropertyName("twoMonthPrice")]
        public double TwoMonthPrice { get; set; }

        [JsonPropertyName("urgentBuyoutDiscount")]
        public double UrgentBuyoutDiscount { get; set; }

        [JsonPropertyName("sampleByPrices")]
        public SampleByPrices SampleByPrices { get; set; }

        [JsonPropertyName("userAppraisalId")]
        public int UserAppraisalId { get; set; }

        [JsonPropertyName("isOfRetailQuality")]
        public bool IsOfRetailQuality { get; set; }

        [JsonPropertyName("buyoutCity")]
        public string BuyoutCity { get; set; }
    }




}
