using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NameCheck.WebApi
{
    public class NameCheckViewModel
    {
        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string Name { get; set; }
        public List<NameCheckModel> History { get; set; }
    }
}