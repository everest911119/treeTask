namespace Backend.Model
{
    public class BoM
    {
        public int Id { get; set; }
        public string name { get; set; }
        public List<BoM> children { get; set; }

    }


    public class BoMFlat
    {
        public string COMPONENT_NAME { get; set; }
        public string PARENT_NAME { get; set; }
       
    }
}
