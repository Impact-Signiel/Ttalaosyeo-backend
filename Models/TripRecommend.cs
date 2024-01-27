using System;
using System.Collections.Generic;

namespace signiel.Models;

public partial class TripRecommend
{
    public ulong Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<TripRecommendItem> TripRecommendItemNextNavigations { get; set; } = new List<TripRecommendItem>();

    public virtual ICollection<TripRecommendItem> TripRecommendItemRecommendNavigations { get; set; } = new List<TripRecommendItem>();
}
