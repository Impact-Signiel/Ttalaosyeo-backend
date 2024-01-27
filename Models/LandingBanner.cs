using System;
using System.Collections.Generic;

namespace signiel.Models;

public partial class LandingBanner
{
    public ulong Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Image { get; set; } = null!;

    public string Query { get; set; } = null!;
}
