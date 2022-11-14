namespace WebApplication3.RequestsModels.RequestModels
{
    public class UpdateCategoryRequestModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }
    }
}
