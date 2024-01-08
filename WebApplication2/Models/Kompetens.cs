using WebApplication2.Models;

public class Kompetens
{
    public int Id { get; set; }
    public string Titel { get; set; }
    public string Beskrivning { get; set; }

    public virtual ICollection<CVKompetens> CVKompetenser { get; set; }
}
