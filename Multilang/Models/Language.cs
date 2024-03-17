using System.ComponentModel.DataAnnotations;

namespace Multilang.Models
{
    public class Language
    {
        [Key]
        public int LangId { get; set; }

        [Required]
        [MaxLength(100)]
        public string LangTitle { get; set; }

        #region Relation
        public List<New> News { get; set; }
        #endregion
    }
}
