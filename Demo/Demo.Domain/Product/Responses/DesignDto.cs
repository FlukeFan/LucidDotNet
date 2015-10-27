namespace Demo.Domain.Product.Responses
{
    public class DesignDto
    {
        public int      DesignId;
        public string   Name;

        public DesignDto() { }

        public DesignDto(Design design)
        {
            DesignId = design.Id;
            Name = design.Name;
        }
    }
}
