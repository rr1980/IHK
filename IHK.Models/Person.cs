namespace IHK.Models
{
    public class Person : BaseModel
    {
        public int Anrede { get; set; }
        public string Name { get; set; }
        public string Vorname { get; set; }
        public string Postleitzahl { get; set; }
        public string Stadt { get; set; }
        public string Strasse { get; set; }
        public string Hausnummer { get; set; }
        public string Telefon { get; set; }
    }
}

