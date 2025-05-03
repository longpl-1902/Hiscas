using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hicas.Revit.Setting
{
    public class SystemLibrary
    {
        public string Library {  get; set; }
        public PipeSpec Pipe_Spec { get; set; }
        public PipeSpec Fittings_Spec { get; set; }
    }
}
