using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.UI;
using Hicas.Revit.Commands.Model;
using Hicas.Revit.Setting;

namespace Hicas.Revit.Commands.Helpers
{
    public class AssignSpecHelper
    {
        public static void AssignSpec(Document doc, List<ElementId> ids, StoredData storeData)
        {
            foreach(var id in ids)
            {
                AssignSpec(doc, id, storeData);
            }
        }

        public static void AssignSpec(Document doc, ElementId id, StoredData storeData)
        {
            Schema schema = RevitHelper.GetOrCreateSchema(Definitions.Definitions.SCHEMA_GUID);
            RevitHelper.AddOrUpdateEntity(doc, schema, id, storeData);
        }

        public static SpecInfo GetSpec(Element element, PipeSpec pipeSpec)
        {
            Schema schema = RevitHelper.GetOrCreateSchema(Definitions.Definitions.SCHEMA_GUID);

            try
            {
                string id = RevitHelper.GetSpecId(schema, element);
                return RevitHelper.GetSpecById(id, pipeSpec);
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public static Dictionary<Element, List<Element>> GetElement(UIDocument uidoc, List<ElementId> elementIds)
        {
            var pipeCurves = RevitHelper.GetElement(uidoc.Document, elementIds.ToList(), BuiltInCategory.OST_PipeCurves);

            Dictionary<Element, List<Element>> result = new Dictionary<Element, List<Element>>();

            //From pipe get connected fitting
            foreach (MEPCurve curve in pipeCurves)
            {
                ConnectorSet connectors = curve.ConnectorManager.Connectors;

                var fittings = new List<Element>();

                foreach (Connector connector in connectors)
                {

                    if (connector.IsConnected)
                    {
                        ConnectorSet connectedConnectors = connector.AllRefs;
                        foreach (Connector connectedConnector in connectedConnectors)
                        {
                            Element connectedElement = connectedConnector.Owner;

                            if (connectedElement.Id == curve.Id)
                                continue;

                            //if element is not fitting continue
                            if (connectedElement.Category?.BuiltInCategory == BuiltInCategory.OST_PipeFitting)
                                fittings.Add(connectedElement);
                        }
                    }

                }

                result.Add(curve, fittings);
            }

            return result;
        }

        public static void IsolateElements(Document doc, List<ElementId> elementIds)
        {
            UIDocument uidoc = new UIDocument(doc);
            View activeView = doc.ActiveView;

            using (Transaction tx = new Transaction(doc, "Isolate elements"))
            {
                tx.Start();

                // Ensure the view supports temporary visibility modes

                activeView.IsolateElementsTemporary(elementIds);

                tx.Commit();
            }
        }

        public static void UnIsolateElements(Document doc)
        {
            UIDocument uidoc = new UIDocument(doc);
            View activeView = doc.ActiveView;

            using (Transaction tx = new Transaction(doc, "Unisolate elements"))
            {
                tx.Start();

                // Ensure the view supports temporary visibility modes

                activeView.DisableTemporaryViewMode(TemporaryViewMode.TemporaryHideIsolate);

                tx.Commit();
            }
        }
    }
}
