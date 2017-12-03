using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using Qoden.Util;

namespace Qoden.Validation
{
    public static class DataMemberPropertyValidatorExtension
    {
        public static Check<T> CheckDataMember<T, TDto>(this IValidator validator, TDto dto,
            Expression<Func<TDto, T>> property,
            Action<Error> onError = null, bool clear = true)
        {
            var dataProperty = PropertyName(property);
            var dataMember = dataProperty.GetCustomAttribute<DataMemberAttribute>();

            Assert.Argument(dataMember, "property")
                .NotNull("Property {PropertyName} does not have DataMember attribute",
                    e => { e.Add("PropertyName", dataProperty.Name); });
            var name = dataMember.Name;
            if (string.IsNullOrEmpty(name)) 
            {
                name = dataProperty.Name;
            }
            var value = (T) Inspection.GetValue(dto, dataProperty);
            return validator.CheckValue(value, name, onError, clear);
        }

        public static Check<T> CheckColumn<T, TDto>(this IValidator validator, TDto dto,
            Expression<Func<TDto, T>> property,
            Action<Error> onError = null, bool clear = true)
        {
            var dataProperty = PropertyName(property);
            var propertyName = dataProperty.Name;

            var dataMember = dataProperty.GetCustomAttribute<ColumnAttribute>();

            Assert.Argument(dataMember, "property")
                .NotNull("Property {PropertyName} does not have Column attribute",
                    e => { e.Add("PropertyName", propertyName); });
            var name = dataMember.Name;
            if (string.IsNullOrEmpty(name))
            {
                name = dataProperty.Name;
            }
            var value = (T) Inspection.GetValue(dto, dataProperty);
            return validator.CheckValue(value, name, onError, clear);
        }

        private static PropertyInfo PropertyName<T, TDto>(Expression<Func<TDto, T>> property)
        {
            var propertyName = PropertySupport.ExtractPropertyName(property);
            var dataProperty = typeof(TDto)
                .GetProperties()
                .FirstOrDefault(x => x.Name == propertyName);
            Assert.Argument(dataProperty, "property").NotNull("Property {PropertyName} not found in {DtoType}", e =>
            {
                e.Add("PropertyName", propertyName);
                e.Add("DtoType", typeof(TDto).Name);
            });
            return dataProperty;
        }
    }
}