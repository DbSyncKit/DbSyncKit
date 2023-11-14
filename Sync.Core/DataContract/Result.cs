using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sync.Core.DataContract
{
    public class Result<T>
    {
        public List<T> Added { get; set; }
        public List<T> Deleted { get; set; }

        public List<ValueTuple<T, Dictionary<string, object>>> Edited { get; set; }
    }
}
