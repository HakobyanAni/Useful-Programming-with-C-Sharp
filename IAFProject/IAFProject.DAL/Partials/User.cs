using System;
using System.Collections.Generic;
using System.Text;
using IAFProject.DAL.General;
using Microsoft.AspNetCore.Identity;

namespace IAFProject.DAL.Models
{
    public partial class User : IdentityUser<int>, IBaseEntity, IModified
    {
    }
}