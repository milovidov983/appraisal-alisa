namespace AliceAppraisal.Models {

	
	public class SlotData {
		public string Value { get; set; }
		public string Token { get; set; }
        public bool HasData { get; set; }


        public static SlotData Empty = new SlotData() {
            HasData = false
        };
	}
    
}
