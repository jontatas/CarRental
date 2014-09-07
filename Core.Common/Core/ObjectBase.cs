using Core.Common.Contracts;
using Core.Common.Extensions;
using Core.Common.Utils;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Core
{
    public abstract class ObjectBase : INotifyPropertyChanged, IDataErrorInfo
    {
        public ObjectBase()
        {
            _Validator = GetValidator();
            Validate();
        }

        private event PropertyChangedEventHandler _PropertyChanged;
        protected bool _IsDirty;
        protected IValidator _Validator = null;

        protected IEnumerable<ValidationFailure> _ValidationErrors = null;

        List<PropertyChangedEventHandler> _PropertyChangeSubscribers = new List<PropertyChangedEventHandler>();

        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                if (!_PropertyChangeSubscribers.Contains(value))
                {
                    _PropertyChanged += value;
                    _PropertyChangeSubscribers.Add(value);
                }
            }
            remove
            {
                _PropertyChanged -= value;
                _PropertyChangeSubscribers.Remove(value);
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName, true);
        }

        protected virtual void OnPropertyChanged(string propertyName, bool makeDirty)
        {
            if (_PropertyChanged != null)
                _PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

            if (makeDirty)
                _IsDirty = true;

            Validate();
        }

        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            string propertyName = PropertySupport.ExtractPropertyName(propertyExpression);
            OnPropertyChanged(propertyName);
        }

        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression, bool makeDirty)
        {
            string propertyName = PropertySupport.ExtractPropertyName(propertyExpression);
            OnPropertyChanged(propertyName, makeDirty);
        }

        public bool IsDirty
        {
            get { return _IsDirty; }
            set { _IsDirty = value; }
        }

        public List<ObjectBase> GetDirtyObjects()
        {
            List<ObjectBase> dirtyObjects = new List<ObjectBase>();

            WalkObjectGraph(
                o =>
                {
                    if (o.IsDirty)
                        dirtyObjects.Add(o);

                    return false;
                }, coll => { });

            return dirtyObjects;
        }

        public void CleanAll()
        {
            WalkObjectGraph(
                o =>
                {
                    if (o.IsDirty)
                        o.IsDirty = false;

                    return false;
                }, coll => { });
        }

        public virtual bool IsAnythingDirty()
        {
            bool isDirty = false;

            WalkObjectGraph(
                o =>
                {
                    if (o.IsDirty)
                    {
                        isDirty = true;
                        return true;
                    }
                    else
                        return false;
                }, coll => { });
            return isDirty;
        }

        protected void WalkObjectGraph(Func<ObjectBase, bool> snippetForObject, Action<IList> snippetForCollection, params string[] exemptProperties)
        {
            List<ObjectBase> visited = new List<ObjectBase>();
            Action<ObjectBase> walk = null;

            List<string> exemptions = new List<string>();
            if (exemptProperties != null)
                exemptions = exemptProperties.ToList();

            walk = (o) =>
                {
                    if (o != null && !visited.Contains(o))
                    {
                        visited.Add(o);

                        bool exitWalk = snippetForObject.Invoke(o);

                        if (!exitWalk)
                        {
                            PropertyInfo[] properties = o.GetBrowsableProperties();
                            foreach (PropertyInfo property in properties)
                            {
                                if (property.PropertyType.IsSubclassOf(typeof(ObjectBase)))
                                {
                                    ObjectBase obj = (ObjectBase)(property.GetValue(o, null));
                                    walk(obj);
                                }
                                else
                                {
                                    IList coll = property.GetValue(o, null) as IList;
                                    if (coll != null)
                                    {
                                        snippetForCollection.Invoke(coll);

                                        foreach (object item in coll)
                                        {
                                            if (item is ObjectBase)
                                                walk((ObjectBase)item);
                                        }
                                    }
                                }
                            }
                        }
                    }
                };
        }

        protected virtual IValidator GetValidator()
        {
            return null;
        }

        [NotNavigable]
        public IEnumerable<ValidationFailure> ValidationErrors
        {
            get { return _ValidationErrors; }
            set { }
        }

        public void Validate()
        {
            if(_Validator != null)
            {
                ValidationResult results = _Validator.Validate(this);
                _ValidationErrors = results.Errors;
            }
        }

        [NotNavigable]
        public virtual bool IsValid
        {
            get
            {
                if (_ValidationErrors != null && _ValidationErrors.Count() > 0)
                    return false;
                else
                    return true;
            }
        }


        public string Error
        {
            get { return string.Empty; }
        }

        public string this[string columnName]
        {
            get 
            {
                StringBuilder errors = new StringBuilder();

                if(_ValidationErrors != null && _ValidationErrors.Count() > 0)
                {
                    foreach (ValidationFailure validationError in _ValidationErrors)
                    {
                        if (validationError.PropertyName == columnName)
                            errors.AppendLine(validationError.ErrorMessage);
                    }
                }
                return errors.ToString();
            }
        }
    }
}
