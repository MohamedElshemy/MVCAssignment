using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using ASPMVCAssessment.DAL;
using System.Data.Linq;

namespace ASPMVCAssessment.BLL
{
    public class PatientBLL:Repositories<Patient>
    {
       
        public int count;
        public List<Patient> getAll(string searchText, string sortColumn, string sortColumnDir, int pageSize, int skip)
        {
            try
            {

                var obj = GetAll().Where(a => (string.IsNullOrEmpty(searchText)
                || a.PatientName.Contains(searchText)
                || a.PasNumber.ToString().Contains(searchText)
                || a.HomeTelephoneNumber.ToString().Contains(searchText)
                ))
                .AsQueryable().OrderBy(sortColumn + " " + sortColumnDir).ToList();
                count = obj.Count();

                return obj.Skip(skip).Take(pageSize).Distinct().ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
