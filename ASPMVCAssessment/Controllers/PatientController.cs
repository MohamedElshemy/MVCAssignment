using ASPMVCAssessment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASPMVCAssessment.BLL;
using ASPMVCAssessment.DAL;
using AutoMapper;

namespace ASPMVCAssessment.Controllers
{
    public class PatientController : Controller
    {
        PatientBLL _PatientBLL = new PatientBLL();
        NextOfKinBLL _NextOfKinBLL = new NextOfKinBLL();
        SexCodeBLL _SexCodeBLL = new SexCodeBLL();
        // GET: AddPatient
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PatientListGrid()
        {
            return PartialView("PatientListGridPartialView");
        }

        public ActionResult Add()
        {
            return View("Add", new PatientModel());
        }

        [HttpPost]
        public ActionResult AddPatient(PatientModel o)
        {
            NextOfKin _NextOfKin = new NextOfKin();
            Patient _patient = new Patient();
            if (o.Id==0)
            {
                //Add NextOfKin
                _NextOfKin.NokAddressLine1 = o.nextOfKinModel.NokAddressLine1;
                _NextOfKin.NokAddressLine2 = o.nextOfKinModel.NokAddressLine2;
                _NextOfKin.NokAddressLine3 = o.nextOfKinModel.NokAddressLine3;
                _NextOfKin.NokAddressLine4 = o.nextOfKinModel.NokAddressLine4;
                _NextOfKin.NokName = o.nextOfKinModel.NokName;
                _NextOfKin.NokRelationshipCode = o.nextOfKinModel.NokRelationshipCode;
                _NextOfKinBLL.Add(_NextOfKin);
                _NextOfKinBLL.Save();

                //Add Patient
                _patient.DoctorId = o.DoctorId;
                _patient.HomeTelephoneNumber = o.HomeTelephoneNumber;
                _patient.NextOfKinId = _NextOfKin.Id;
                _patient.PasNumber = o.PasNumber;
                _patient.PatientName = o.PatientName;
                _patient.SexCodeId = o.SexCodeId;
                _PatientBLL.Add(_patient);
                _PatientBLL.Save();
            }
            else
            {
                var pateint = _PatientBLL.GetItemById(o.Id);
                var nokToRelation = _NextOfKinBLL.GetAll().Where(a => a.Id == pateint.NextOfKinId).FirstOrDefault();
                //remove nok
                if (nokToRelation != null)
                {
                    _NextOfKin.Id = nokToRelation.Id;
                    _NextOfKin.NokAddressLine1 = o.nextOfKinModel.NokAddressLine1;
                    _NextOfKin.NokAddressLine2 = o.nextOfKinModel.NokAddressLine2;
                    _NextOfKin.NokAddressLine3 = o.nextOfKinModel.NokAddressLine3;
                    _NextOfKin.NokAddressLine4 = o.nextOfKinModel.NokAddressLine4;
                    _NextOfKin.NokName = o.nextOfKinModel.NokName;
                    _NextOfKin.NokRelationshipCode = o.nextOfKinModel.NokRelationshipCode;
                    _NextOfKinBLL.Update(_NextOfKin);
                    _NextOfKinBLL.Save();
                }

                //Update
                _patient.Id = pateint.Id;
                _patient.DoctorId = o.DoctorId;
                _patient.HomeTelephoneNumber = o.HomeTelephoneNumber;
                _patient.NextOfKinId = _NextOfKin.Id;
                _patient.PasNumber = o.PasNumber;
                _patient.PatientName = o.PatientName;
                _patient.SexCodeId = o.SexCodeId;
                _PatientBLL.Update(_patient);
                _PatientBLL.Save();

            }
            return RedirectToAction("Index");
        }

        public ActionResult loadPatientGrid(string searchText)
        {
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            var sortColumn =
                Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();


            var pageSize = length != null ? Convert.ToInt32(length) : 0;
            var skip = start != null ? Convert.ToInt32(start) : 0;
            var GlobalLst = _PatientBLL.getAll(searchText, sortColumn, sortColumnDir, pageSize, skip);
            int totalRecords = _PatientBLL.count;
            var toSerialize = new
            {
                draw = draw,
                recordsFiltered = totalRecords,
                recordsTotal = totalRecords,


                data = GlobalLst.Select(r => new
                {
                    ID = r.Id,
                    PatientName = r.PatientName,
                    PasNumber = r.PasNumber,
                    SexCodeName = _SexCodeBLL.GetItemById((int)r.SexCodeId).Name,
                    HomeTelephoneNumber = r.HomeTelephoneNumber,
                    NokName = _NextOfKinBLL.GetItemById((int)r.NextOfKinId).NokName,
                }).OrderByDescending(e => e.ID).AsQueryable()

            };

            return Json(toSerialize, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(int id)
        {
            try
            {
                var pateintToDelete = _PatientBLL.GetItemById(id);
                var nokToDelete = _NextOfKinBLL.GetAll().Where(a => a.Id == pateintToDelete.NextOfKinId).FirstOrDefault();
                _PatientBLL.Delete(pateintToDelete);
                _NextOfKinBLL.Delete(nokToDelete);
                _NextOfKinBLL.Save();
               
                return Json(new { data = "Delete", Success = true, msg = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = "Delete", Success = false, msg = "Not Deleted" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Edit(int id)
        {
            PatientModel _PatientModel = new PatientModel();
            NextOfKinModel nokModel = new NextOfKinModel();
            var patient = _PatientBLL.GetItemById(id);
            var nextoFKin = _NextOfKinBLL.GetAll().Where(a => a.Id == patient.NextOfKinId).FirstOrDefault();

            //Patient Model
            _PatientModel.Id = patient.Id;
            _PatientModel.DoctorId = (int)patient.DoctorId;
            _PatientModel.HomeTelephoneNumber = (int)patient.HomeTelephoneNumber;
            _PatientModel.PasNumber = (int)patient.PasNumber;
            _PatientModel.PatientName = patient.PatientName;
            _PatientModel.SexCodeId = (int)patient.SexCodeId;
            //next model
            nokModel.NokAddressLine1 = nextoFKin.NokAddressLine1;
            nokModel.NokAddressLine2 = nextoFKin.NokAddressLine2;
            nokModel.NokAddressLine3 = nextoFKin.NokAddressLine3;
            nokModel.NokAddressLine4 = nextoFKin.NokAddressLine4;
            nokModel.NokName = nextoFKin.NokName;
            nokModel.NokRelationshipCode = nextoFKin.NokRelationshipCode;

            _PatientModel.nextOfKinModel = nokModel;
            return View("Add", _PatientModel);
        }
    }
}