﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sync.DB.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class KeyPropertyAttribute : Attribute
    {
        // This attribute can be used to mark properties as key properties
    }
}
