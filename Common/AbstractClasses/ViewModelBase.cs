using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Common
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        //public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly Dictionary<string, List<string>> _dependencies = new Dictionary<string, List<string>>();
        private readonly Dictionary<string, object> _propertyValueStorage;

        public ViewModelBase()
        {
            DetectPropertiesDependencies();
            _propertyValueStorage = new Dictionary<string, object>();
        }

        public static string NameOf<T, TProp>(Expression<Func<T, TProp>> propertySelector)
        {
            var body = (MemberExpression)propertySelector.Body;
            return body.Member.Name;
        }

        protected virtual bool SetValue<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }

            storage = value;
            RaisePropertyChanged(propertyName);

            return true;
        }

        protected virtual bool SetValue<T>(ref T storage, T value, Expression<Func<T>> propertyExpression)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }

            storage = value;

            RaisePropertyChanged(GetPropertyNameFrom(propertyExpression));

            return true;
        }

        protected virtual void SetValue<T>(Expression<Func<T>> propertyExpression, T value)
        {
            var lambdaExpression = propertyExpression as LambdaExpression;

            if (lambdaExpression == null)
            {
                throw new ArgumentException("Invalid lambda expression", "Lambda expression return value can't be null");
            }

            var propertyName = GetPropertyNameFrom(lambdaExpression);
            var storedValue = GetValue<T>(propertyName);

            if (Equals(storedValue, value))
            {
                return;
            }

            _propertyValueStorage[propertyName] = value;
            RaisePropertyChanged(propertyName);
        }

        protected T GetValue<T>(Expression<Func<T>> propertyExpression)
        {
            var lambdaExpression = propertyExpression as LambdaExpression;

            if (lambdaExpression == null)
            {
                throw new ArgumentException("Invalid lambda expression", "Lambda expression return value can't be null");
            }

            var propertyName = GetPropertyNameFrom(lambdaExpression);
            return GetValue<T>(propertyName);
        }

        private T GetValue<T>([CallerMemberName] string propertyName = null)
        {
            object value;

            if (_propertyValueStorage.TryGetValue(propertyName, out value))
            {
                return (T)value;
            }

            return default(T);

        }

        [Conditional("DEBUG")]
        private void CheckPropertyName(string propertyName)
        {
            var propertyDescriptor = TypeDescriptor.GetProperties(this)[propertyName];
            if (propertyDescriptor == null)
            {
                throw new InvalidOperationException(string.Format(null, "The property with the name '{0}' doesn't exist.", propertyName));
            }
        }

        protected string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException("propertyExpression");
            }

            var body = propertyExpression.Body as MemberExpression;

            if (body == null)
            {
                throw new ArgumentException("Invalid argument", "propertyExpression");
            }

            var property = body.Member as PropertyInfo;

            if (property == null)
            {
                throw new ArgumentException("Argument is not a property", "propertyExpression");
            }

            return property.Name;
        }

        private string GetPropertyNameFrom(LambdaExpression lambdaExpression)
        {
            MemberExpression memberExpression;

            if (lambdaExpression.Body is UnaryExpression)
            {
                var unaryExpression = lambdaExpression.Body as UnaryExpression;
                memberExpression = unaryExpression.Operand as MemberExpression;
            }
            else
            {
                memberExpression = lambdaExpression.Body as MemberExpression;
            }

            return memberExpression.Member.Name;
        }

        private string GetPropertyNameFrom<T>(Expression<Func<T>> propertyExpression)
        {
            var memberExpression = propertyExpression.Body as MemberExpression;

            if (memberExpression == null)
            {
                throw new ArgumentException("Expression must be a MemberExpression.", propertyExpression.Name);
            }

            return memberExpression.Member.Name;
        }

        protected virtual void RaisePropertyChanged<T>(Expression<Func<T>> selectorExpression)
        {
            if (selectorExpression == null)
            {
                throw new ArgumentNullException("selectorExpression");
            }

            var body = selectorExpression.Body as MemberExpression;

            if (body == null)
            {
                throw new ArgumentException("The body must be a member expression");
            }

            RaisePropertyChanged(body.Member.Name);
        }

        protected virtual void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            CheckPropertyName(propertyName);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void RaisePropertiesChangedBy<T>(params Expression<Func<T>>[] expressions)
        {
            expressions.Select(expr => GetPropertyName(expr)).ToList().ForEach(propertyName =>
            {
                RaisePropertyChanged(propertyName);
                RaisePropertiesChangedBy(propertyName, new List<string>() { propertyName });
            });

        }

        protected virtual void RaisePropertiesChangedBy(string propertyName, List<string> calledProperties = null)
        {
            if (!_dependencies.Any() || !_dependencies.ContainsKey(propertyName))
            {
                return;
            }

            if (calledProperties == null)
            {
                calledProperties = new List<string>();
            }

            var dependentProperties = _dependencies[propertyName];

            foreach (var dependentProperty in dependentProperties)
            {
                if (!calledProperties.Contains(dependentProperty))
                {
                    RaisePropertyChanged(dependentProperty);
                    RaisePropertiesChangedBy(dependentProperty, calledProperties);
                }
            }
        }

        private void DetectPropertiesDependencies()
        {
            var propertyInfoWithDependencies = GetType().GetProperties()
                .Where(prop => Attribute.IsDefined(prop, typeof(DependentPropertiesAttribute)))
                .ToArray();

            foreach (var propertyInfo in propertyInfoWithDependencies)
            {
                var currentAttributes = propertyInfo.GetCustomAttributes(false).OfType<DependentPropertiesAttribute>().Single();
                if (currentAttributes.Properties != null)
                {
                    foreach (string prop in currentAttributes.Properties)
                    {
                        if (!_dependencies.ContainsKey(prop))
                        {
                            _dependencies.Add(prop, new List<string>());
                        }

                        _dependencies[prop].Add(propertyInfo.Name);
                    }
                }
            }
        }

        protected void RaisePropertiesDependentOn(params Expression<Func<object>>[] expressions)//rename as work name
        {
            expressions.Select(expression => GetPropertyName(expression))
                .ToList()
                .ForEach(propertyName =>
            {
                RaisePropertyChanged(propertyName);
                RaiseDependentProperties(propertyName, new List<string>() { propertyName });
            });

        }

        private void RaiseDependentProperties(string propertyName, List<string> calledProperties = null)
        {
            if (!_dependencies.Any() || !_dependencies.ContainsKey(propertyName))
            {
                return;
            }

            if (calledProperties == null)
            {
                calledProperties = new List<string>();
            }

            var dependentProperties = _dependencies[propertyName];

            foreach (var dependentProperty in dependentProperties)
            {
                if (!calledProperties.Contains(dependentProperty))
                {
                    RaisePropertyChanged(dependentProperty);
                    RaiseDependentProperties(dependentProperty, calledProperties);
                }
            }
        }

    }

    public class DependentPropertiesAttribute : Attribute
    {
        private readonly string[] properties;

        public DependentPropertiesAttribute(params string[] dp)
        {
            properties = dp;
        }

        public string[] Properties
        {
            get
            {
                return properties;
            }
        }
    }

}