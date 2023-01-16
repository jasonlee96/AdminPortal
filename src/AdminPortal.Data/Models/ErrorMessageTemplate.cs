using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CommonService.Enums;

namespace AdminPortal.Data.Models
{
    [Table(nameof(ErrorMessageTemplate))]
    public class ErrorMessageTemplate
    {
        [Key, Required]
        public virtual int Id { get; set; }

        [Required]

        [Column(TypeName = "nvarchar(20)")]
        public virtual ErrorCode Code { get; set; }
        [Required]

        [Column(TypeName = "nvarchar(10)")]
        public virtual LanguageCode Language { get; set; }

        [Required]
        public virtual string Message { get; set; }
    }

}
