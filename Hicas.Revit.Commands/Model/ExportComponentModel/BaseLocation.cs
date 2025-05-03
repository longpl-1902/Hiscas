using Autodesk.Revit.DB;

namespace Hicas.Revit.Commands.Model.ExportComponentModel
{
    public abstract class BaseLocation
    {
    }

    // Location for Pipe
    public class PipeLocation : BaseLocation
    {
        public XYZ StartPoint { get; set; }
        public XYZ EndPoint { get; set; }
    }

    // Location for Fitting
    public class FittingLocation : BaseLocation
    {
        public XYZ Position { get; set; }
    }
}
