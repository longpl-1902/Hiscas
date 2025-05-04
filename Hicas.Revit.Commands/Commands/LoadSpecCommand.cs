using System;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Hicas.Revit.Commands.Actions;

namespace Hicas.Revit.Commands.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class LoadSpecCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                LoadSpecAction action = new LoadSpecAction(commandData.Application.ActiveUIDocument);
                action.Execute();
            }
            catch
            {
                TaskDialog.Show("Error", "Load spec failed!");
            }
            return Result.Succeeded;
        }
    }
}
