﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLDVision
{
    [AttributeUsage(AttributeTargets.Property)]
    public class InputParamAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property)]
    public class OutputParamAttribute : Attribute
    {

    }
}