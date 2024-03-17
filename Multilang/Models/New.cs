using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Multilang.Models
{
    public class New
    {
        [Key]
        public int NewsId { get; set; }
        public int LangId { get; set; }

        [MaxLength(300)]
        public string? Title { get; set; }
        public string? Description { get; set; }

        [MaxLength(300)]
        public string? ImageName { get; set; }
        public DateTime CreateDate { get; set; }

        #region Relation
        [ForeignKey("LangId")]
        public Language Language { get; set; }
        #endregion
    }
}
