﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IAFProject.BLL.Models.General;
using Microsoft.AspNetCore.Mvc;

namespace IAFProject.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public abstract class BaseIAFController : ControllerBase
    {
      
    }
}
