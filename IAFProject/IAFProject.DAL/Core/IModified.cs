using System;
using System.Collections.Generic;
using System.Text;

namespace IAFProject.DAL.Core
{
    public interface IModified
    {
        DateTime? ModifiedDate { get; set; }
    }
}