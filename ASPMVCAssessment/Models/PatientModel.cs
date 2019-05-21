using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASPMVCAssessment.Models
{
    public class PatientModel
    {
        [Required(ErrorMessage = "*")]
        public int Id { get; set; }
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "*")]
        public string PatientName { get; set; }
        [Required(ErrorMessage = "*")]
        public int SexCodeId { get; set; }
        [Required(ErrorMessage = "*")]
        public int HomeTelephoneNumber { get; set; }
        [Required(ErrorMessage = "*")]
        public int PasNumber { get; set; }
        [Required(ErrorMessage = "*")]
        public int DoctorId { get; set; }
        [Required(ErrorMessage = "*")]
        public int NextOfKinId { get; set; }

        public NextOfKinModel nextOfKinModel { get; set; }
    }
}