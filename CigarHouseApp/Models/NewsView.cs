using System;
using System.Collections.Generic;

namespace CigarHouseApp.Models;

public partial class NewsView
{
    public int? NewsId { get; set; }

    public string? NewsText { get; set; }

    public string? AuthorName { get; set; }

    public string? AuthorFullname { get; set; }
}
