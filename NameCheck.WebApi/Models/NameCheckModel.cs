using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NameCheck.WebApi
{
    public class NameCheckModel
    {
        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string Name { get; set; }
        public IList<CheckResultModel> History { get; set; }
    }
}