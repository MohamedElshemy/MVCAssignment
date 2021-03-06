//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ASPMVCAssessment.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Patient
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public Nullable<int> SexCodeId { get; set; }
        public Nullable<int> HomeTelephoneNumber { get; set; }
        public Nullable<int> PasNumber { get; set; }
        public Nullable<int> DoctorId { get; set; }
        public Nullable<int> NextOfKinId { get; set; }
    
        public virtual Doctor Doctor { get; set; }
        public virtual NextOfKin NextOfKin { get; set; }
        public virtual SexCode SexCode { get; set; }
    }
}
