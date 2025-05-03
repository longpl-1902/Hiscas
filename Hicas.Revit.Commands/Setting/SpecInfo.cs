using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hicas.Revit.Setting
{
    public class SpecInfo
    {
        public string Size {  get; set; }
        public string Desc { get; set; }
        public string Type {  get; set; }
        public string Comptype {  get; set; }
        public string Material {  get; set; }
        public DataTable DataTable {  get; set; }
        public string Schedule {  get; set; }
        public string Standard { get; set; }
        public string CompCode {  get; set; }
        public string GUID { get; set; }
    }
}
