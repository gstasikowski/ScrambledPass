using ScrambledPass.Helpers;
using ScrambledPass.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace ScrambledPass.Models
{
    public class ApplicationViewModel : ObservableObject
    {
        #region Fields
        private ICommand _changePageCommand;
        private IPageViewModel _currentPageViewModel;
        private List<IPageViewModel> _pageViewModels;
        #endregion Fields

        public ApplicationViewModel()
        {
            PageViewModels.Add(new Views.GeneratorView());
            PageViewModels.Add(new Views.SettingsView());

            CurrentPageViewModel = PageViewModels[0];
        }

        #region Commands & properties
        public ICommand ChangePageCommand
        {
            get
            {
                if (_changePageCommand == null)
                {
                    _changePageCommand = new RelayCommand(page => ChangeViewModel((IPageViewModel)page), page => page is IPageViewModel);
                }

                return _changePageCommand;
            }
        }

        public List<IPageViewModel> PageViewModels
        {
            get
            {
                if (_pageViewModels == null)
                    _pageViewModels = new List<IPageViewModel>();

                return _pageViewModels;
            }
        }

        public IPageViewModel CurrentPageViewModel
        {
            get { return _currentPageViewModel; }
            set
            {
                if (_currentPageViewModel != value)
                {
                    _currentPageViewModel = value;
                    OnPropertyChanged("CurrentPageViewModel");
                }
            }
        }
        #endregion Commands & properties

        #region Methods
        private void ChangeViewModel(IPageViewModel viewModel)
        {
            if (!PageViewModels.Contains(viewModel))
                PageViewModels.Add(viewModel);

            CurrentPageViewModel = PageViewModels.FirstOrDefault(vm => vm == viewModel);
        }
        #endregion Methods
    }
}
