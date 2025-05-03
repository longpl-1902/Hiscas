using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace Hicas.WPF.Model
{
    public class PypeTypeModel
    {
        public ElementId Id {  get; set; }
        public string Type { get; set; }
        public bool IsSeleted { get; set; } 
    }
}
