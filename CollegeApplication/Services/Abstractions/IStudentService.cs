﻿using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollegeApplication.Services.Abstractions
{
    public interface IStudentService
    {
        void RegisterStudent(StudentRegistryDto dto);
    }
}