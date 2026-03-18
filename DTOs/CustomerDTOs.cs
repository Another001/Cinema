namespace MyApi.DTOs
{
  public class CustomerCreateReqDto
  {
      public required string Name { get; set; }
      public required string Phone { get; set; }
      public required string Email { get; set; } = null!;
      public long UserStatusId { get; set; } = 1;
  }
  public class CustomerGetResDto
  {
  public string Name {get; set;} = string.Empty;
  public string Phone {get; set;} = string.Empty;
  public string Email {get; set;} = string.Empty;
  public string? Status {get; set;}
  }
  public class CustomerFilterDto
  {
    public string? Name {get; set;}
    public string? Email {get; set;}
    public string? Phone {get; set;}
  }
}