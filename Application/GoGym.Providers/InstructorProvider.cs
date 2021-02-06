using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using GoGym.Data;
using GoGym.Providers.Base;
using GoGym.Providers.ViewModels;

namespace GoGym.Providers
{
    public class InstructorProvider : BaseProvider
    {
        public InstructorProvider(FitnessEntities context, IPrincipal principal)
            : base(context, principal)
        {
        }

        public void AddInstructor(
                    string barcode,
                    string name,
                    DateTime hireDate,
                    string status,
                    string email,
                    string homePhone,
                    string cellPhone,
                    bool isActive)
        {
            Instructor inst = new Instructor();
            inst.Barcode = barcode;
            inst.Name = name;
            inst.HireDate = hireDate;
            inst.Status = Convert.ToChar(status);
            inst.Email = email;
            inst.HomePhone = homePhone;
            inst.CellPhone = cellPhone;
            inst.IsActive = isActive;
            EntityHelper.SetAuditFieldForInsert(inst, principal.Identity.Name);
            context.Add(inst);
            context.SaveChanges();
        }

        public void UpdateInstructor(
            int id,
            string barcode,
            string name,
            DateTime hireDate,
            string status,
            string email,
            string homePhone,
            string cellPhone,
            bool isActive)
        {
            Instructor inst = context.Instructors.SingleOrDefault(instructor => instructor.ID == id);
            if (inst != null)
            {
                inst.Barcode = barcode;
                inst.Name = name;
                inst.HireDate = hireDate;
                inst.Status = Convert.ToChar(status);
                inst.Email = email;
                inst.HomePhone = homePhone;
                inst.CellPhone = cellPhone;
                inst.IsActive = isActive;
                EntityHelper.SetAuditFieldForUpdate(inst, principal.Identity.Name);
                context.SaveChanges();
            }
        }

        public void DeleteInstructor(int id)
        {
            Instructor inst = context.Instructors.SingleOrDefault(instructor => instructor.ID == id);
            if (inst != null)
            {
                context.Delete(inst);
                context.SaveChanges();
            }
        }


        public IEnumerable<Instructor> GetAllInstructors()
        {
            return context.Instructors.ToList();
        }

        public IEnumerable<Instructor> GetActiveInstructors()
        {
            return context.Instructors.Where(inst => inst.IsActive).ToList();
        }


        public Instructor GetInstructor(int id)
        {
            return context.Instructors.SingleOrDefault(inst => inst.ID == id);
        }

        public Instructor GetInstructor(string barcode)
        {
            return context.Instructors.SingleOrDefault(inst => inst.Barcode == barcode);
        }

        public InstructorCheckInViewModel DoCheckIn(int branchID, string barcode)
        {
            InstructorCheckInViewModel instVM = null;

            Instructor inst = GetInstructor(barcode);
            if (inst != null)
            {
                InstructorAttendance instAtt = new InstructorAttendance();
                instAtt.InstructorID = inst.ID;
                instAtt.Date = DateTime.Today;
                instAtt.AttendanceIn = DateTime.Now;
                instAtt.BranchID = branchID;

                instVM = new InstructorCheckInViewModel();
                instVM.Barcode = inst.Barcode;
                instVM.BranchID = branchID;
                instVM.CheckInWhen = instAtt.AttendanceIn;
                instVM.Name = inst.Name;

                context.Add(instAtt);
                context.SaveChanges();
            }

            return instVM;
        }

        public List<InstructorCheckInViewModel> GetInstructorCheckInHistory(int branchID)
        {
            var query = from inst in context.InstructorAttendances
                        where inst.BranchID == branchID
                        orderby inst.AttendanceIn descending
                        select new InstructorCheckInViewModel
                        {
                            BranchID = branchID,
                            Barcode = inst.Instructor.Barcode,
                            CheckInWhen = inst.AttendanceIn,
                            Name = inst.Instructor.Name
                        };
            return query.ToList();
        }
    }
}
