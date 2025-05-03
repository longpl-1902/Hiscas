using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hicas.Revit.Setting
{
    public class PipeSpec
    {
        public ICollection<SpecInfo> PN6 { get; set; }
        public ICollection<SpecInfo> PN8 { get; set; }
    }
}
