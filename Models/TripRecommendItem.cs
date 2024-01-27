using System;
using System.Collections.Generic;

namespace signiel.Models;

public partial class TripRecommendItem
{
    public ulong Id { get; set; }

    public string Text { get; set; } = null!;

    public string? Image { get; set; }

    public string Query { get; set; } = null!;

    public ulong? Next { get; set; }

    public ulong Recommend { get; set; }

    public virtual TripRecommend? NextNavigation { get; set; }

    public virtual TripRecommend RecommendNavigation { get; set; } = null!;
}
