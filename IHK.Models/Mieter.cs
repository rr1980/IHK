namespace IHK.Models
{
    public class Mieter : Person
    {
        public Wohnung Wohnung { get; set; }
        public string WbsNummer { get; set; }
    }
}

