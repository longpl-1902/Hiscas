using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Hicas.Revit.Model;
using Hicas.Revit.Setting;
using Hicas.WPF.Model;
using MVVMCore;


namespace Hicas.WPF.ViewModels
{
    public class AssignSpecViewModel : NotifyPropertyChanged
    {
        private ObservableCollection<ReviewSpecModel> _reviews = new ObservableCollection<ReviewSpecModel>();
        public ObservableCollection<ReviewSpecModel> Reviews
        {
            get => _reviews;
            set
            {
                _reviews = value;
                OnPropertyChanged(nameof(Reviews));
            }
        }

        private ObservableCollection<SpecInfo> _pipeSpecs = new ObservableCollection<SpecInfo>();
        public ObservableCollection<SpecInfo> PipeSpecs
        {
            get => _pipeSpecs;
            set
            {
                _pipeSpecs = value;
                OnPropertyChanged(nameof(PipeSpecs));
            }
        }

        private ObservableCollection<SpecInfo> _fittingSpecs = new ObservableCollection<SpecInfo>();
        public ObservableCollection<SpecInfo> FittingSpecs
        {
            get => _fittingSpecs;
            set
            {
                _fittingSpecs = value;
                OnPropertyChanged(nameof(FittingSpecs));
            }
        }

        public ObservableCollection<PypeTypeModel> _pipeTypes = new ObservableCollection<PypeTypeModel>();
        public ObservableCollection<PypeTypeModel> PipeTypes
        {
            get => _pipeTypes;
            set
            {
                _pipeTypes = value;
                OnPropertyChanged(nameof(PipeTypes));
            }
        }

        private SpecInfo _selectedPipeSpec;
        public SpecInfo SelectedPipeSpec
        {
            get => _selectedPipeSpec;
            set
            {
                _selectedPipeSpec = value;
                OnPropertyChanged(nameof(SelectedPipeSpec));
            }
        }

        private SpecInfo _selectedFittingsSpec;
        public SpecInfo SelectedFittingSpec
        {
            get => _selectedFittingsSpec;
            set
            {
                _selectedPipeSpec = value;
                OnPropertyChanged(nameof(SelectedFittingSpec));
            }
        }

        private bool _isIsolate;
        public bool IsIsolate
        {
            get => _isIsolate;
            set
            {
                _isIsolate = value;
                OnPropertyChanged(nameof(IsIsolate));
            }
        }

        public ICommand ToggleIsolateCmd { get; set; }
        public ICommand AssignSpecCmd { get; set; }
        public ICommand ReloadViewCmd { get; set; }
        public ICommand OKCmd { get; set; }
        public ICommand CancelCmd { get; set; }
    }
}
