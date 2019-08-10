using System;
using System.Collections.Generic;

namespace IAFProject.DAL.Models
{
    public partial class RolesClaim
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
