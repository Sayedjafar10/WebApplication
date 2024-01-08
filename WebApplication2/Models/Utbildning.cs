namespace WebApplication2.Models
{
	public class Utbildning
	{
        public int Id { get; set; }

        public string Namn { get; set; }
        public string Beskrivning { get; set; }

        public virtual ICollection<CVUtbildning> CVUtbildningar { get; set; }
    }
}
