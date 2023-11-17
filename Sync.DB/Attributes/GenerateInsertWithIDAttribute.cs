using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sync.DB.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class GenerateInsertWithIDAttribute : Attribute
    {
        // if insert querry generation will be with id or without id
    }
}
