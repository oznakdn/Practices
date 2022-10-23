namespace JwtAuthentication.Dtos.ProductDtos
{
    public class CreateProductDto
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
    }
}
