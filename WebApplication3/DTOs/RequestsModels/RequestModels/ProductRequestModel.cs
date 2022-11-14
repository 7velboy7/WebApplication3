namespace WebApplication3.RequestsModels.RequestModels
{
    public class ProductRequestModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int? ParentCategoryId { get; set; }
    }
}
