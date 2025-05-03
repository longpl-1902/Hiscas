using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Hicas.Revit.Commands
{
    public class RevitBase
    {
        public Application Application { get; set; }
        public UIApplication UIApplication { get; set; }
        public UIDocument UIDocument { get; set; }
        public Document Document { get; set; }

        public static RevitBase Instance (UIDocument uidoc)
        {
            return new RevitBase()
            {
                UIApplication = uidoc.Application,
                UIDocument = uidoc,
                Document = uidoc.Document,
                Application = uidoc.Application.Application
            };
        }
    }
}
