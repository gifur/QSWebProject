using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace QS.Common.Validator
{
    /// <summary>
    /// Validator based on Data Annotations. 
    /// This validator use IValidatableObject interface and
    /// ValidationAttribute ( hierachy of this) for
    /// perform validation
    /// </summary>
    public class DataAnnotationsEntityValidator
        : IEntityValidator
    {
        #region Private Methods


        /// <summary>
        /// Get erros if object implement IValidatableObject
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="item">验证项</param>
        /// <param name="errors">当前错误的集合</param>
        void SetValidatableObjectErrors<TEntity>(TEntity item, List<string> errors) where TEntity : class
        {
            if (typeof(IValidatableObject).IsAssignableFrom(typeof(TEntity)))
            {
                var validationContext = new ValidationContext(item, null, null);

                var validationResults = ((IValidatableObject)item).Validate(validationContext);

                errors.AddRange(validationResults.Select(vr => vr.ErrorMessage));
            }
        }

        /// <summary>
        /// 获得验证属性的错误
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="item">需要验证的项目、</param>
        /// <param name="errors">当前错误的集合</param>
        void SetValidationAttributeErrors<TEntity>(TEntity item, List<string> errors) where TEntity : class
        {
            var result = from property in TypeDescriptor.GetProperties(item).Cast<PropertyDescriptor>()
                         from attribute in property.Attributes.OfType<ValidationAttribute>()
                         where !attribute.IsValid(property.GetValue(item))
                         select attribute.FormatErrorMessage(string.Empty);

            if (result != null && result.Any())
            {
                errors.AddRange(result);
            }
        }

        #endregion

        #region IEntityValidator Members


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool IsValid<TEntity>(TEntity item) where TEntity : class
        {
            if (item == null)
                return false;

            List<string> validationErrors = new List<string>();

            SetValidatableObjectErrors(item, validationErrors);
            SetValidationAttributeErrors(item, validationErrors);

            return !validationErrors.Any();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public IEnumerable<string> GetInvalidMessages<TEntity>(TEntity item) where TEntity : class
        {
            if (item == null)
                return null;

            List<string> validationErrors = new List<string>();

            SetValidatableObjectErrors(item, validationErrors);
            SetValidationAttributeErrors(item, validationErrors);


            return validationErrors;
        }

        #endregion
    }
}
