using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace IAFProject.BLL.Models.General
{
    public class ModelBase : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Fields

        private ModelStateEnum _modelState;

        #endregion

        #region Properties

        #endregion

        #region Methods

        protected void Notify([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void BaseModel_PropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != nameof(HasChanges) && args.PropertyName != nameof(ModelState))
            {
                if (ModelState == ModelStateEnum.Unchanged)
                {
                    ModelState = ModelStateEnum.Changed;
                }
            }
        }

        #endregion

        #region ModelState 

        public bool HasChanges
        {
            get { return _modelState != ModelStateEnum.Unchanged; }
        }

        public ModelStateEnum ModelState
        {
            get { return _modelState; }
            set
            {
                if (_modelState != value)
                {
                    _modelState = value;
                    Notify();
                    Notify(nameof(HasChanges));
                }
            }
        }

        #endregion

        #region Constructors

        public ModelBase()
        {
            ModelState = ModelStateEnum.Unchanged;
            PropertyChanged += BaseModel_PropertyChanged;
        }

        #endregion
    }
}
