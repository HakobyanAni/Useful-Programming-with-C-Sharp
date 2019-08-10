using System;
using System.Collections.Generic;
using System.Text;

namespace IAFProject.DAL.General
{
    public interface IEntity
    {
        int Id { get; set; }
        DateTime CreateDate { get; set; }
    }
}
