using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bigprofits.Models;

public partial class Poster
{
    public int Id { get; set; }

    public string? Posterid { get; set; }

    public string? Postername { get; set; }

    public string? PosterContent { get; set; }

    public DateTime? Posterdate { get; set; }

    public int? PosterStatus { get; set; }

    public string? PosterImage { get; set; }

    public string? Field1 { get; set; }

    public decimal? Field2 { get; set; }

    public DateTime? Field3 { get; set; }
    [NotMapped]
    public IFormFile? PosteImage { get; set; }
}
