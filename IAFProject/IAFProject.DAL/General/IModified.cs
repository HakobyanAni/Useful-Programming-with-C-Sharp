using System;
using System.Collections.Generic;
using System.Text;

namespace IAFProject.DAL.General
{
    public interface IModified
    {
        DateTime? ModifiedDate { get; set; }
    }
}
