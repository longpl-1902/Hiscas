using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Controls;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Hicas.Revit.Commands.Definitions;
using Hicas.Revit.Commands.Model;
using Hicas.Revit.Setting;

namespace Hicas.Revit.Commands.Helpers
{
    public class RevitHelper
    {
        public static List<Element> GetElement(Document doc, List<ElementId> elementids, BuiltInCategory category)
        {
            List<Element> result = new List<Element>();

            foreach (var id in elementids)
            {
                var element = doc.GetElement(id);

                var elementCate = element.Category?.BuiltInCategory;
                if (elementCate != null && elementCate == category)
                    result.Add(element); 
            }

            return result;
        }

        public static List<Element> GetElement(Document doc, BuiltInCategory category)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            List<Element> elements = collector.OfCategory(category).ToElements().ToList();
            return elements;
        }

        public static List<Element> FilterElement(List<Element> elements, BuiltInCategory category)
        {
            return elements.Where(e => e.Category.BuiltInCategory == category).ToList();
        }

        public static Schema GetOrCreateSchema(string guid)
        {
            Guid schemaId = new Guid(guid);

            Schema schema = Schema.Lookup(schemaId);

            if (schema == null)
            {
                SchemaBuilder builder = new SchemaBuilder(schemaId);
                builder.SetSchemaName(Definitions.Definitions.SCHEMA_NAME);

                //Get field to store data
                var properties = typeof(StoredData).GetProperties();

                foreach (var prop in properties)
                {
                    builder.AddSimpleField(prop.Name, prop.PropertyType);
                }

                schema = builder.Finish();
            }

            return schema;
        }

        public static void AddOrUpdateEntity(Document doc, Schema schema, ElementId id, StoredData data)
        {
            Entity entity = new Entity(schema);

            //Get field to store data
            var properties = typeof(StoredData).GetProperties();

            foreach (var prop in properties)
            {
                entity.Set(prop.Name, prop.GetValue(data).ToString());
            }

            using (Transaction trans = new Transaction(doc, "assign spec"))
            {
                trans.Start();
                Element element = doc.GetElement(id);
                element.SetEntity(entity);
                trans.Commit();
            }
        }

        public static string GetSpecId(Schema schema, Element element)
        {
            try
            {
                Entity storedEntity = element.GetEntity(schema);
                var guid = storedEntity.Get<string>("GUID");

                return guid;
            }
            catch
            {
                return null;
            }
        }

        public static SpecInfo GetSpecById(string id, PipeSpec pipeSpec)
        {
            foreach (var prop in typeof(PipeSpec).GetProperties())
            {
                var value = prop.GetValue(pipeSpec);

                if (value is ICollection<SpecInfo> specs)
                {
                    var spec = specs.FirstOrDefault(s => s.GUID == id);

                    if (spec != null) return spec;
                }
            }

            return null;
        }

        public static string GetParameter(Element element, string name)
        {
            return element.LookupParameter(name)?.ToString();
        }
    }
}
