﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaste_API.Business.Interfaces
{
    public interface IEmailTemplateRepository
    {
        string GetEmailTemplate(string name);
    }
}
