﻿using System;
using System.Collections.Generic;

namespace signiel.Models;

public partial class Trip
{
    public ulong Id { get; set; }

    public ulong Author { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string Location { get; set; } = null!;

    public string Thumbnail { get; set; } = null!;

    public uint Personnel { get; set; }

    public long Price { get; set; }

    public uint Nights { get; set; }

    public uint Days { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User AuthorNavigation { get; set; } = null!;

    public virtual ICollection<TripImage> TripImages { get; set; } = new List<TripImage>();

    public virtual ICollection<TripSchedule> TripSchedules { get; set; } = new List<TripSchedule>();

    public virtual ICollection<TripTag> TripTags { get; set; } = new List<TripTag>();

    public virtual ICollection<UserTrip> UserTrips { get; set; } = new List<UserTrip>();
}
