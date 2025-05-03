using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hicas.Revit.Commands.Model.ExportComponentModel
{
    public class ComponentInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string PipeType { get; set; }
        public BaseLocation Location { get; set; } 
        public string Size { get; set; }
        public string Desc { get; set; }
        public string Wall_Thickness { get; set; }
        public string Length { get; set; }
        public string Weight { get; set; }
        public string Schedule { get; set; }
        public string Standard { get; set; }
        public string CompCode { get; set; }
        public string GUID { get; set; }
    }
}
