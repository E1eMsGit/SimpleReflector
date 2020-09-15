using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using SimpleReflector.Model;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;

namespace SimpleReflector.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<AssemblyTypeNode> AssemblyTypes {
            get => _assemblyTypes;
            set => Set(ref _assemblyTypes, value);
        }

        public string MemberDescription
        {
            get => _memberDescription;
            set => Set(ref _memberDescription, value);
        }

        private string _memberDescription;
        private ObservableCollection<AssemblyTypeNode> _assemblyTypes;
        private OpenedFile _file;

        public MainViewModel()
        {
            
        }

        public void GetMemberDescription(MemberInfo member)
        {
            MemberDescription = _file.GetMemberDescription(member);
        }

        public RelayCommand OpenFileCommand => new RelayCommand(
            () =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "dll files (*.dll)|*.dll"
                };

                if ((bool)openFileDialog.ShowDialog())
                {
                    _file = new OpenedFile(openFileDialog.FileName);
                    AssemblyTypes = _file.GetAssemblyTypes();
                }
            });
    }
}