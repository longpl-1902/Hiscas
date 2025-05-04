using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Hicas.Revit.Commands.Definitions;
using Hicas.Revit.Commands.Helpers;
using Hicas.Revit.Commands.Model;
using Hicas.Revit.Setting;
using Hicas.WPF.Model;
using Hicas.WPF.View.AssignSpecView;
using Hicas.WPF.ViewModels;
using MVVMCore;
using Newtonsoft.Json;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;

namespace Hicas.Revit.Commands.Actions
{
    public class AssignSpecAction : ActionBase
    {
        private Dictionary<Element, List<Element>> _selectedPipes;
        private AssignSpecViewModel _viewModel;
        private Window _assignSpecView;

        public AssignSpecAction(UIDocument uidocument) : base(uidocument)
        {
            _viewModel = new AssignSpecViewModel();
            _viewModel.ToggleIsolateCmd = new RelayCommand(IsolateElementInvoke);
            _viewModel.AssignSpecCmd = new RelayCommand(AssignSpecInvoke);
            _viewModel.OKCmd = new RelayCommand(ReviewSpecInvoke);
            _viewModel.ReloadViewCmd = new RelayCommand(ReloadViewInvoke);
            _viewModel.CancelCmd = new RelayCommand(CancelInvoke);
            _viewModel.LoadSpecCmd = new RelayCommand(LoadSpecInvoke);

            _assignSpecView = new AssignSpectWiew();
        }

        public void AssignSpecInvoke()
        {
            if (RevitBaseModel.SpecLibrary is null)
            {
                throw new Exception("User hasn't load spec library!");
            }

            var selectedFittingSpec = _viewModel.SelectedFittingSpec;
            var selectedPipeSpec = _viewModel.SelectedPipeSpec;

            List<PypeTypeModel> selectedPipeTypes = new List<PypeTypeModel>();
            if (_viewModel.IsAssignAll)
            {
                selectedPipeTypes = _viewModel.PipeTypes.ToList();
            }
            else
            {
                if (selectedPipeTypes.Count == 0)
                {
                    TaskDialog.Show("Warning", "User hasn't select any pipe type!");
                    return;
                }

                selectedPipeTypes = _viewModel.PipeTypes.Where(p => p.IsSeleted).ToList();
            }

            StoredData fittingData = new StoredData { GUID = selectedFittingSpec.GUID };

            StoredData pipeData = new StoredData { GUID = selectedPipeSpec.GUID };

            foreach (var pipeType in selectedPipeTypes)
            {
                var pipe = _selectedPipes.Where(p => p.Key.Id == pipeType.Id).FirstOrDefault();
                if (pipe.Key != null)
                {
                    AssignSpecHelper.AssignSpec(Document, pipe.Key.Id, pipeData);

                    AssignSpecHelper.AssignSpec(Document, pipe.Value.Select(e => e.Id).ToList(), fittingData);
                }
            }

            TaskDialog.Show("Success", "Assign spec successfully!");
        }

        public void LoadSpecInvoke()
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Title = "Select a JSON file",
                Filter = Definitions.Definitions.FILE_FILTER_JSON,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Multiselect = false
            };

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = fileDialog.FileName;
                string json = File.ReadAllText(selectedFilePath);
                RevitBaseModel.SpecLibrary = JsonConvert.DeserializeObject<SpecLibrary>(json);

                _viewModel.PipeSpecs = new ObservableCollection<SpecInfo>(RevitBaseModel.SpecLibrary.Pipe_Spec.SummarySpec());
                _viewModel.FittingSpecs = new ObservableCollection<SpecInfo>(RevitBaseModel.SpecLibrary.Fittings_Spec.SummarySpec());
            }

            TaskDialog.Show("Success", "Load spec successfully!");
        }

        public void CancelInvoke()
        {
            _assignSpecView.Close();
        }

        public void ReviewSpecInvoke()
        {
            //Clear all item before update
            _viewModel.Reviews.Clear();

            var reviews = new List<ReviewSpecModel>();

            foreach (var element in _selectedPipes.Keys)
            {
                var pipeSpec = AssignSpecHelper.GetSpec(element, RevitBaseModel.SpecLibrary.Pipe_Spec);
                var fittingSpec = AssignSpecHelper.GetSpec(_selectedPipes[element].FirstOrDefault(), RevitBaseModel.SpecLibrary.Fittings_Spec);
                var type = Document.GetElement(element.GetTypeId()).Name;

                if (pipeSpec is null) continue;

                reviews.Add(new ReviewSpecModel
                {
                    PypeType = Document.GetElement(element.GetTypeId()).Name,
                    PypeSpec = pipeSpec is null ? "-" : pipeSpec.Desc,
                    FittingSpec = fittingSpec is null ? "-" : fittingSpec.Desc,
                });
            }

            reviews = reviews.GroupBy(r => r.PypeType).Select(g => g.First()).ToList();

            _viewModel.Reviews = new ObservableCollection<ReviewSpecModel>(reviews);
        }

        public void IsolateElementInvoke()
        {
            bool isIsolating = _viewModel.IsIsolate;
            if (isIsolating)
            {
                var elementIds = new List<ElementId>();

                foreach (var key in _selectedPipes.Keys)
                {
                    elementIds.Add(key.Id);

                    foreach (var value in _selectedPipes[key])
                    {
                        elementIds.Add(value.Id);
                    }
                }

                AssignSpecHelper.IsolateElements(Document, elementIds);
            }
            else
            {
                AssignSpecHelper.UnIsolateElements(Document);
            }

            _viewModel.IsIsolate = isIsolating;
        }

        public void ReloadViewInvoke()
        {
            _viewModel.PipeTypes = new ObservableCollection<PypeTypeModel>(ReloadPipeType());
            _viewModel.Reviews.Clear();
        }

        private List<PypeTypeModel> ReloadPipeType()
        {
            return _selectedPipes.Keys
                .Select(p => new PypeTypeModel
                {
                    Id = p.Id,
                    Type = Document.GetElement(p.GetTypeId()).Name,
                    IsSeleted = false
                })
                .GroupBy(p => p.Type)
                .Select(g => g.First())
                .ToList();
        }

        public override void Execute()
        {
            var element = UIDocument.Selection.PickObjects(ObjectType.Element).Select(e => e.ElementId);

            _selectedPipes = AssignSpecHelper.GetElement(UIDocument, element.ToList());

            _viewModel.FittingSpecs = new ObservableCollection<SpecInfo>();

            _viewModel.PipeSpecs = new ObservableCollection<SpecInfo>();

            _viewModel.PipeTypes = new ObservableCollection<PypeTypeModel>(ReloadPipeType());

            _viewModel.Reviews = new ObservableCollection<ReviewSpecModel>();

            _assignSpecView.DataContext = _viewModel;

            _assignSpecView.ShowDialog();
        }
    }
}
