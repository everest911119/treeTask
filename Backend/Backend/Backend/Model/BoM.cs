namespace Backend.Model
{
    public class BoM
    {
        public int Id { get; set; }
        public string name { get; set; }
        public List<BoM> children { get; set; }

    }
}
