namespace Crawler.Jobs
{
    public class RawParameter
    {
        public string Title { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return $"{Title}, {Value}";
        }
    }
}