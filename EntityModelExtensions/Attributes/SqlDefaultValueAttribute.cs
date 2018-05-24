using System;

namespace EntityModelExtensions.Attributes
{
    /// <summary>
    /// Атрибут для установки значения по умолчанию для поля таблицы SQL сервера
    /// <remarks>https://stackoverflow.com/questions/32049742/ef-7-set-initial-default-value-for-datetime-column</remarks>
    /// <example>[SqlDefaultValue("getutcdate()")]</example>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SqlDefaultValueAttribute : Attribute
    {
        public string DefaultValue { get; set; }

        public SqlDefaultValueAttribute(string value)
        {
            DefaultValue = value;
        }
    }
}
