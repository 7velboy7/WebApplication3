namespace WebApplication3.RequestsModels.RequestModels
{
    public class CreateCategoryRequestModel
    {
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }
    }
}
