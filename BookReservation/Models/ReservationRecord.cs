namespace BookApi.Models
{
    public class ReservationRecord
    {
        public ReservationRecord() { }

        public ReservationRecord(long id, string title, string author)
        {
            this.id = id;
            this.title = title;
            this.author = author;
        }

        public long id { get; set; }
        public string title { get; set; }
        public string author { get; set; }
    }
}
