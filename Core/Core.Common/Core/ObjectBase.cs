using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

using Core.Common.Annotations;
using Core.Common.Extensions;
using Core.Common.Utils;

using FluentValidation;
using FluentValidation.Results;

namespace Core.Common.Core
{
    public class ObjectBase : INotifyPropertyChanged, IDataErrorInfo
    {
        public ObjectBase()
        {
            _Validator = this.GetValidator();
            Validate();

        }
        protected IEnumerable<ValidationFailure> _ValidationErrors = null; 
        protected IValidator _Validator = null;

        public static CompositionContainer Container { get; set; }
        
        private event PropertyChangedEventHandler _PropertyChanged;
        
        List<PropertyChangedEventHandler> _PropertyChangedSubscribers = new List<PropertyChangedEventHandler>();
 
        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                if (!_PropertyChangedSubscribers.Contains(value))
                {
                    _PropertyChanged += value;
                    _PropertyChangedSubscribers.Add(value);
                }
            }
            remove
            {
                _PropertyChanged -= value;
                _PropertyChangedSubscribers.Remove(value);
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName, true);
        }

        protected virtual void OnPropertyChanged(string propertyName, bool makeDirty)
        {
            if (_PropertyChanged != null)
            {
                _PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

            if (makeDirty)
            {
                _IsDirty = true;
            }

            Validate();
        }

        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            OnPropertyChanged(propertyExpression, true);
        }

        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression, bool makeDirty)
        {
            string propertyName = PropertySupport.ExtractPropertyName(propertyExpression);
            OnPropertyChanged(propertyName, makeDirty);
        }

        private bool _IsDirty;

        [NotNavigable]
        public bool IsDirty
        {
            get
            {
                return _IsDirty;
            }
            set
            {
                _IsDirty = value;
            }
        }

        protected List<ObjectBase> GetDirtyObjects()
        {
            List<ObjectBase> dirtyObjects = new List<ObjectBase>();
            
            WalkObjectGraph( o => 
            { 
                if (o.IsDirty)
                    dirtyObjects.Add(o);
                return false;
            }, coll =>
                {
                    
                });


            return dirtyObjects;
        }

        public void CleanAll()
        {
            WalkObjectGraph(o =>
            {
                if (o.IsDirty)
                    o.IsDirty = false;
                return false;
            }, coll =>
            {

            }); 
        }

        public bool IsAnythingDirty()
        {
            bool isDirty = false;

            WalkObjectGraph(o =>
                {
                    if (o.IsDirty)
                    {
                        isDirty = true;
                        return true;        //short circuit
                    }
                    else
                        return false;
            }, coll =>
            {

            });

            return isDirty;
        }


        protected void WalkObjectGraph(Func<ObjectBase, bool> snippetForObject, 
            Action<IList> snippetForCollection, 
            params string[] exemptProperties)
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

                        foreach (var property in properties)
                        {
                            if (!exemptions.Contains(property.Name))
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
            get
            {
                return _ValidationErrors;
            }
            set { }
        } 

        public void Validate()
        {
            if (_Validator != null)
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
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        string IDataErrorInfo.Error
        {
            get
            {
                return string.Empty;
            }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                StringBuilder errors = new StringBuilder();
                if (_ValidationErrors != null && _ValidationErrors.Count() > 0)
                {
                    foreach (var validationError in _ValidationErrors)
                    {
                        if (validationError.PropertyName == columnName)
                        {
                            errors.AppendLine(validationError.ErrorMessage);
                        }
                    }
                }

                return errors.ToString();
            }
        }
    }
}
