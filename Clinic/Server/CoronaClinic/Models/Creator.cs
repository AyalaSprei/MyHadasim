using System;
using System.Collections.Generic;

namespace CoronaClinic.Models;

public partial class Creator
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Immune> Immunes { get; set; } = new List<Immune>();
}
