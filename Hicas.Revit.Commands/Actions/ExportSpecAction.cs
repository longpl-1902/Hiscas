using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Hicas.Revit.Commands.Helpers;
using Hicas.Revit.Commands.Model.ExportComponentModel;
using Hicas.Revit.Setting;
using Newtonsoft.Json;

namespace Hicas.Revit.Commands.Actions
{
    public class ExportSpecAction : ActionBase
    {
        private List<ComponentInfo> _componentInfo;

        public ExportSpecAction(UIDocument uidocument) : base(uidocument)
        {
        }

        public override void Execute()
        {
            if (RevitBaseModel.SpecLibrary is null)
            {
                TaskDialog.Show("Error", "User must load library before export spec");
                return;
            }

            _componentInfo = GetSpecData();

            var saveDialog = new SaveFileDialog
            {
                Title = "Save JSON File",
                Filter = Definitions.Definitions.FILE_FILTER_JSON,
                DefaultExt = ".json",
                FileName = "output.json"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveDialog.FileName;
                string jsonContent = JsonConvert.SerializeObject(_componentInfo);

                File.WriteAllText(filePath, jsonContent);
            }
        }

        public List<ComponentInfo> GetSpecData()
        {
            List<ComponentInfo> result = new List<ComponentInfo>();

            FilteredElementCollector collector = new FilteredElementCollector(Document);

            ElementCategoryFilter pipeFilter = new ElementCategoryFilter(BuiltInCategory.OST_PipeCurves);
            ElementCategoryFilter fittingFilter = new ElementCategoryFilter(BuiltInCategory.OST_PipeFitting);

            LogicalOrFilter combinedFilter = new LogicalOrFilter(pipeFilter, fittingFilter);

            ICollection<Element> elements = collector
                            .WherePasses(combinedFilter)
                            .WhereElementIsNotElementType()
                            .ToElements();

            var schema = RevitHelper.GetOrCreateSchema(Definitions.Definitions.SCHEMA_GUID);

            foreach (var element in elements)
            {
                SpecInfo specInfo = null;
                BaseLocation location = null;
                string specId = RevitHelper.GetSpecId(schema, element);

                if (specId == null) continue;

                if (element.Category.BuiltInCategory == BuiltInCategory.OST_PipeCurves)
                {
                    specInfo = RevitHelper.GetSpecById(specId, RevitBaseModel.SpecLibrary.Pipe_Spec);
                    var locationCurve = element.Location as LocationCurve;

                    location = new PipeLocation
                    {
                        StartPoint = locationCurve.Curve.GetEndPoint(0),
                        EndPoint = locationCurve.Curve.GetEndPoint(1)
                    };
                }
                else if (element.Category.BuiltInCategory == BuiltInCategory.OST_PipeFitting)
                {
                    specInfo = RevitHelper.GetSpecById(specId, RevitBaseModel.SpecLibrary.Fittings_Spec);
                    var position = element.Location as LocationPoint;

                    location = new FittingLocation
                    {
                        Position = position.Point
                    };
                }

                if (specInfo == null) continue;
                else
                {
                    result.Add(new ComponentInfo
                    {
                        Id = element.Id.ToString(),
                        Name = element.Name,
                        CompCode = specInfo.CompCode,
                        Desc = specInfo.Desc,
                        GUID = specInfo.GUID,
                        Length = specInfo.DataTable.Length,
                        Location = location,
                        PipeType = specInfo.Type,
                        Schedule = specInfo.Schedule,
                        Size = specInfo.Size,
                        Standard = specInfo.Standard,
                        Wall_Thickness = specInfo.DataTable.Wall_Thickness,
                        Weight = specInfo.DataTable.Weight,
                    });
                }
            }

            return result;
        }
    }
}
