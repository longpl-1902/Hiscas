using System;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Hicas.Revit.Commands.Actions;

namespace Hicas.Revit.Commands.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class AssignSpecCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                AssignSpecAction action = new AssignSpecAction(commandData.Application.ActiveUIDocument);
                action.Execute();
                return Result.Succeeded;
            }
            catch(Exception ex) 
            {
                TaskDialog.Show("Error", ex.Message);
                return Result.Succeeded;
            }
        }
    }
}
