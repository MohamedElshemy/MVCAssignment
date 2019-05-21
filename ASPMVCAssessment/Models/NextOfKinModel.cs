using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASPMVCAssessment.Models
{
    public class NextOfKinModel
    {
        [Required(ErrorMessage = "*")]
        public int Id { get; set; }
        [Required(ErrorMessage = "*")]
        public string NokName { get; set; }
        [Required(ErrorMessage = "*")]
        public string NokRelationshipCode { get; set; }
        [Required(ErrorMessage = "*")]
        public string NokAddressLine1 { get; set; }
        [Required(ErrorMessage = "*")]
        public string NokAddressLine2 { get; set; }
        [Required(ErrorMessage = "*")]
        public string NokAddressLine3 { get; set; }
        [Required(ErrorMessage = "*")]
        public string NokAddressLine4 { get; set; }
    }
}