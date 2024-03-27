namespace CoronaClinic.Models
{
    public class MemberDto
    {
     
            public int Id { get; set; }
            public string Name { get; set; }
            public string LastName { get; set; }
            public string IdentityNumber { get; set; }
            public string City { get; set; }
            public string Street { get; set; }
            public string HomeNumber { get; set; }
            public DateTime Birthdate { get; set; }
            public string Telephone { get; set; }
            public string MobilePhone { get; set; }
            public bool IsImmune { get; set; }
            public List<ImmuneDto>? Immunes { get; set; }
            public int? IllnessId { get; set; }
            public DateTime PositiveDate { get; set; }
            public DateTime NegativeDate { get; set; }
            public string? Picture { get; set; }


    }
    public class ImmuneDto
    {
        public int? ImmuneId { get; set; }
        public DateTime Date { get; set; }
        public int CreatorId { get; set; }
        public string? CreatorName { get; set; }
    }
    public class MemberMinimal

    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
    }
}

