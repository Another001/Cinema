using Microsoft.AspNetCore.Components.Web;

namespace MyApi.DTOs{
  public class MovieCreateReqDto
  {
    public string Name {get; set;} = null!;
    public string Title {get; set;} = null!;
    public string Describe {get; set;} = string.Empty;
    public long Duration {get; set;} = 30;
    public bool Used {get; set;} = true;
    public long MovieStatusId {get; set;} = 1;
  }
  public class MovieGetResDto
  {
    public long? Id {get; set;}
    public string Name {get; set;} = string.Empty;
    public string Title {get; set;} = string.Empty;
    public long? Duration {get; set;}
    public string? MovieStatus {get; set;}
  }
  public class MovieFilterDto
  {
    public string? Name {set; get;}
    public long? Duration {set; get;}
  }
  public class MovieUpdateDto
  {
    public string? Name {set; get;}
    public string? Title {set; get;}
    public string? Describe {set; get;}
    public long? Duration {set; get;}
    public long? MovieStatusId {get; set;}
  }
}