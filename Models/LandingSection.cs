using System;
using System.Collections.Generic;

namespace signiel.Models;

public partial class LandingSection
{
    public ulong Id { get; set; }

    public string Label { get; set; } = null!;

    public string Image { get; set; } = null!;

    public string Query { get; set; } = null!;
}
