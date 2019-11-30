namespace Domain.Entities
{
    public class Parameter
    {
        public string Title { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return $"{Title}, {Value}";
        }
    }
}