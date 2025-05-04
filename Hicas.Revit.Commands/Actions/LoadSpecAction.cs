using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Hicas.Revit.Setting;
using Newtonsoft.Json;

namespace Hicas.Revit.Commands.Actions
{
    public class LoadSpecAction : ActionBase
    {
        public LoadSpecAction(UIDocument uidocument) : base(uidocument)
        {
        }

        public override void Execute()
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Title = "Select a JSON file",
                Filter = "JSON Files (*.json)|*.json",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Multiselect = false
            };

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = fileDialog.FileName;
                string json = File.ReadAllText(selectedFilePath);
                RevitBaseModel.SpecLibrary = JsonConvert.DeserializeObject<SpecLibrary>(json);
            }

            TaskDialog.Show("Success", "Load spec successfully!");
        }
    }
}
