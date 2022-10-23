namespace JwtAuthentication.Dtos.ProductDtos
{
    public class UpdateProductDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
    }
}
