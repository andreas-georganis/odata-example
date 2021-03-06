﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public interface IJwtService
    {
        string GenerateToken(int userId, string username);

        string ValidateToken(string token);
    }
}
