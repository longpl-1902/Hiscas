using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Hicas.Revit.Setting;

namespace Hicas.Revit.Commands.Actions
{
    public abstract class ActionBase
    {
        public SpecLibrary SpecLibrary { get; set; }
        public Application Application { get; set; }
        public UIApplication UIApplication { get; set; }
        public UIDocument UIDocument { get; set; }
        public Document Document { get; set; }

        public ActionBase(UIDocument uidocument)
        {
            UIDocument = uidocument;
            Document = uidocument.Document;
            UIApplication = uidocument.Application;
            Application = uidocument.Application.Application;
        }

        public abstract void Execute();
    }
}
