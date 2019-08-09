using System;
using System.Collections.Generic;
using System.Text;
using IAFProject.DAL.Core;
using Microsoft.AspNetCore.Identity;

namespace IAFProject.DAL.Partials
{
    public partial class User : IdentityUser<int>, IEntity, IModified
    {
    }
}