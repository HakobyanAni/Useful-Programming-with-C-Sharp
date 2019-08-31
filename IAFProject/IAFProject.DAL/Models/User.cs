using System;
using System.Collections.Generic;

namespace IAFProject.DAL.Models
{
    public partial class User
    {
        public string Name { get; set; }
        public bool Deleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? LastEntryDate { get; set; }
        public bool IsRegistered { get; set; }
    }
}
