using System;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Hicas.Revit.Commands.Actions;

namespace Hicas.Revit.Commands.Commands
{
    [Transaction(TransactionMode.ReadOnly)]
    public class ExportSpecCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                ExportSpecAction action = new ExportSpecAction(commandData.Application.ActiveUIDocument);
                action.Execute();
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
            }

            return Result.Succeeded;
        }
    }
}
