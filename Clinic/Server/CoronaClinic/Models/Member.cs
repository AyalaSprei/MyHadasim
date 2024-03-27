using System;
using System.Collections.Generic;

namespace CoronaClinic.Models;

public partial class Member
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string IdentityNumber { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Street { get; set; } = null!;

    public string HomeNumber { get; set; } = null!;

    public DateTime Birthdate { get; set; }

    public string? Telephone { get; set; }

    public string? MobilePhone { get; set; }

    public bool IsImmune { get; set; }

    public string? Picture { get; set; }

    public virtual ICollection<Illness> Illnesses { get; set; } = new List<Illness>();

    public virtual ICollection<Immune> Immunes { get; set; } = new List<Immune>();
}
