namespace Application.Dal.Domain.Settings
{
    public class Setting : BaseEntity
    {
        /// <summary>
        /// Наименование параметра
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Значение
        /// </summary>
        public string Value { get; set; }
    }
}