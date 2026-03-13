using System;
using System.Collections.Generic;

namespace CigarHouseApp.Models;

public partial class News
{
    public int NewsId { get; set; }

    public string NewsText { get; set; } = null!;

    public string? NewsAuthor { get; set; }

    public int? AuthorId { get; set; }

    public virtual User? Author { get; set; }
}
