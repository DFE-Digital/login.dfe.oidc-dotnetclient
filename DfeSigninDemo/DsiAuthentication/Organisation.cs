using System;

namespace DfeSigninDemo.DsiAuthentication
{
    public class Organisation
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Category Category { get; set; }

        public string Urn { get; set; }

        public string Uid { get; set; }

        public string UkPrn { get; set; }

        public string LegacyId { get; set; }

        public string Sid { get; set; }

        public string DistrictAdministrative_Code { get; set; }
    }
}
