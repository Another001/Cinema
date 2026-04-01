using Microsoft.AspNetCore.Components.Web;

namespace MyApi.DTOs{
  public class MovieCreateReqDto
  {
    public required string Name {get; set;} = null!;
    public required string Title {get; set;} = null!;
    public string Describe {get; set;} = string.Empty;
    public long Duration {get; set;} = 30;
    public required DateTime ReleaseDate { get; set; }
    public required DateTime EndDate { get; set; }
    public required string Genre { get; set; } = null!;
    public required string Director { get; set; } = null!;
    public required string Cast { get; set; } = null!;
    public required string Figure{set; get;}
    public string? Language{set; get;}
    public string? Trailer{set; get;}
    public bool Used {get; set;} = true;
    public long MovieStatusId {get; set;} = 1;
  }
  public class MovieListResDto
  {
    public long? Id{set; get;}
    public string Name{set;get;} = string.Empty;
    public long? Duration{set; get;}
    public string Genre {set; get;} = string.Empty;
    public DateTime? ReleaseDate{set; get;}
    public string? Figure{set; get;}
  }
  public class MovieGetResDto
  {
    public long? Id {get; set;}
    public string Name {get; set;} = string.Empty;
    public string Title {get; set;} = string.Empty;
    public long Duration {get; set;}
    public string Genre{set; get;} = string.Empty;
    public string Cast{set; get;} = string.Empty;
    public string Director{set; get;} = string.Empty;
    public string Figure{set; get;} = string.Empty;
    public DateTime? ReleaseDate{set; get;}
    public string Language{set; get;} = string.Empty;
    public string Trailer{set; get;} = string.Empty;
    public string Describe{set; get;} = string.Empty;
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
    public DateTime? ReleaseDate{get; set;}
    public DateTime? EndDate{set; get;}
    public string? Cast {set; get;}
    public string? Director {set; get;}
    public string? Genre{set; get;}
    public string? Language{set; get;}
    public string? Trailer{set; get;}
  }
}