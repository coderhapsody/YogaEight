using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using GoGym.Data;
using GoGym.Providers.Base;
using GoGym.Providers.ViewModels;
using NPOI.HSSF.UserModel;

namespace GoGym.Providers
{
    public class ClassProvider : BaseProvider
    {
        public ClassProvider(FitnessEntities context, IPrincipal principal) : base(context, principal)
        {            
        }

        public IDictionary<int, bool> GetAttendancesStatus(int classRunningID)
        {
            Dictionary<int, bool> result = new Dictionary<int, bool>();
            var query = from att in context.ClassAttendances
                        where att.ClassRunningID == classRunningID
                        select new
                        {
                            att.CustomerID,
                            att.IsAttend
                        };
            foreach (var item in query)
                result.Add(item.CustomerID, item.IsAttend);

            return result;
        }

        public void CopyAttendances(int currentClassRunningID, int fromClassRunningID)
        {     
            //TODO
            //context.proc_ClassRunningCopyAttendanceFromDate(currentClassRunningID, fromClassRunningID);            
        }

        public ClassRunning GetClassRunning(int classRunningID)
        {
            return context.ClassRunnings.SingleOrDefault(run => run.ID == classRunningID);
        }

        public void AddParticipant(int classRunningID, int customerID)
        {
            ClassAttendance classAttendance = new ClassAttendance();
            classAttendance.ClassRunningID = classRunningID;
            classAttendance.CustomerID = customerID;
            classAttendance.IsAttend = false;
            context.Add(classAttendance);
            context.SaveChanges();
        }

        public void DeleteParticipant(int classRunningID, int customerID)
        {
            ClassAttendance classAttendance = context.ClassAttendances.SingleOrDefault(att => att.ClassRunningID == classRunningID && att.CustomerID == customerID);
            if (classAttendance != null)
            {
                context.Delete(classAttendance);
                context.SaveChanges();
            }
        }

        public void AddTimeSlot(int branchID, int dayOfWeek, string time)
        {
            ClassTimeSlot timeSlot = new ClassTimeSlot();
            timeSlot.BranchID = branchID;
            timeSlot.DayOfWeek = dayOfWeek;
            timeSlot.StartTime = time;
            context.Add(timeSlot);
            context.SaveChanges();
        }

        public void UpdateTimeSlot(int id, int branchID, int dayOfWeek, string time)
        {
            ClassTimeSlot timeSlot = context.ClassTimeSlots.SingleOrDefault(ts => ts.ID == id);
            if (timeSlot != null)
            {
                timeSlot.BranchID = branchID;
                timeSlot.DayOfWeek = dayOfWeek;
                timeSlot.StartTime = time;
                context.Add(timeSlot);
                context.SaveChanges();
            }
        }

        public void DeleteTimeSlot(int id)
        {
            ClassTimeSlot timeSlot = context.ClassTimeSlots.SingleOrDefault(ts => ts.ID == id);
            context.Delete(timeSlot);
            context.SaveChanges();
        }

        public void DeleteTimeSlot(int[] id)
        {
            foreach (int timeSlotID in id)
            {
                ClassTimeSlot timeSlot = context.ClassTimeSlots.SingleOrDefault(ts => ts.ID == timeSlotID);
                context.Delete(timeSlot);
            }
            context.SaveChanges();
        }

        public IEnumerable<string> GetTimeSlots(int branchID, int dayOfWeek)
        {
            return context.ClassTimeSlots
                          .Where(ts => ts.BranchID == branchID && ts.DayOfWeek == dayOfWeek)
                          .Select(ts => ts.StartTime)
                          .ToList();
        }

        public void AddRoom(string code, string name, bool isActive)
        {
            ClassRoom room = new ClassRoom();
            room.Code = code;
            room.Name = name;
            room.IsActive = isActive;
            EntityHelper.SetAuditFieldForInsert(room, principal.Identity.Name);
            context.Add(room);
            context.SaveChanges();
        }

        public void UpdateRoom(int id, string code, string name, bool isActive)
        {
            ClassRoom room = context.ClassRooms.SingleOrDefault(classRoom => classRoom.ID == id);
            if (room != null)
            {
                room.Code = code;
                room.Name = name;
                room.IsActive = isActive;
                EntityHelper.SetAuditFieldForUpdate(room, principal.Identity.Name);
                context.SaveChanges();
            }
        }

        public void DeleteRoom(int id)
        {
            ClassRoom room = context.ClassRooms.SingleOrDefault(classRoom => classRoom.ID == id);
            context.Delete(room);
            context.SaveChanges();
        }

        public ClassRoom GetRoom(int id)
        {
            ClassRoom room = context.ClassRooms.SingleOrDefault(classRoom => classRoom.ID == id);
            return room;
        }

        public IEnumerable<ClassRoom> GetActiveClassRooms(int branchID)
        {
            var query = from room in context.ClassRooms
                        join activeRoom in context.ActiveClassRoomInBranches on room.ID equals activeRoom.ClassRoomID
                        where room.IsActive && activeRoom.BranchID == branchID
                        select room;            
            return query.ToList();
        }

        public IEnumerable<ClassRoom> GetAllClassRooms(int branchID)
        {
            var query = from room in context.ClassRooms
                        join activeRoom in context.ActiveClassRoomInBranches on room.ID equals activeRoom.ClassRoomID
                        select room;
            return query.ToList();
        }

        public IEnumerable<ClassRoom> GetAllClassRooms()
        {
            var query = from room in context.ClassRooms
                        select room;
            return query.ToList();
        }

        public void AddClass(string code, string name, bool isActive, bool isPaid)
        {
            Class cls = new Class();
            cls.Code = code;
            cls.Name = name;
            cls.IsActive = isActive;
            cls.IsPaid = isPaid;
            EntityHelper.SetAuditFieldForInsert(cls, principal.Identity.Name);
            context.Add(cls);
            context.SaveChanges();
        }

        public void UpdateClass(int id, string code, string name, bool isActive, bool isPaid)
        {
            Class cls = context.Classes.SingleOrDefault(c => c.ID == id);
            if (cls != null)
            {
                cls.Code = code;
                cls.Name = name;
                cls.IsActive = isActive;
                cls.IsPaid = isPaid;
                EntityHelper.SetAuditFieldForUpdate(cls, principal.Identity.Name);
                context.SaveChanges();
            }
        }

        public void DeleteClass(int id)
        {
            Class @class = context.Classes.SingleOrDefault(cls => cls.ID == id);
            context.Delete(@class);
            context.SaveChanges();
        }

        public void DeleteClass(int[] arrayID)
        {
            foreach (int id in arrayID)
            {
                Class @class = context.Classes.SingleOrDefault(cls => cls.ID == id);
                context.Delete(@class);
            }
            context.SaveChanges();
        }

        public IEnumerable<Class> GetAllClasses()
        {
            return context.Classes.ToList();
        }

        public IEnumerable<Class> GetAllActiveClasses()
        {
            return context.Classes.Where(cls => cls.IsActive).ToList();
        }

        public Class GetClass(int id)
        {
            return context.Classes.SingleOrDefault(cls => cls.ID == id);
        }


        public void AddSchedule(int dayOfWeek, int branchID, int year, int month, int classID, string level, int roomID, string timeStart, string timeEnd, int instructorID)
        {
            ClassScheduleDetail sch = new ClassScheduleDetail();
            sch.BranchID = branchID;
            sch.Month = Convert.ToByte(month);
            sch.Year = year;
            sch.DayOfWeek = dayOfWeek;
            EntityHelper.SetAuditFieldForInsert(sch, principal.Identity.Name);
            sch.ClassID = classID;
            sch.Level = level;
            sch.ClassRoomID = roomID;
            sch.TimeStart = timeStart;
            sch.TimeEnd = timeEnd;
            sch.InstructorID = instructorID;
            context.Add(sch);
            context.SaveChanges();
        }

        public IEnumerable<BranchRoomCapacityViewModel> GetBranchesByClassRoom(int roomID)
        {
            var query = from branch in context.Branches
                        join activeRoom in context.ActiveClassRoomInBranches on branch.ID equals activeRoom.BranchID
                        where activeRoom.ClassRoomID == roomID
                        select new BranchRoomCapacityViewModel
                               {
                                   BranchID = branch.ID,
                                   CurrentBranch = branch,
                                   Capacity = activeRoom.Capacity
                               };
            return query.ToList();
        }

        public void UpdateRoomsAtBranch(int roomID, IList<BranchRoomCapacityViewModel> branchesID)
        {
            foreach (var item in context.ActiveClassRoomInBranches.Where(room => room.ClassRoomID == roomID))
                context.Delete(item);

            foreach (var item in branchesID)
            {
                ActiveClassRoomInBranch room = new ActiveClassRoomInBranch();
                room.ClassRoomID = roomID;
                room.BranchID = item.BranchID;
                room.Capacity = Convert.ToInt16(item.Capacity);
                context.Add(room);
            }

            context.SaveChanges();

        }

        public void DeleteSchedule(int id)
        {
            var schedule = context.ClassScheduleDetails.Single(sch => sch.ID == id);
            
            foreach (var classRunning in schedule.ClassRunnings)
            {
                classRunning.ClassAttendances.Clear();                                
            }                                           
            //schedule.ClassRunnings.Clear();            
            context.Delete(schedule);
            context.SaveChanges();
        }

        public void UploadFromExcel(int branchID, int year, int month, string fileName)
        {
            IList<ExcelClassScheduleViewModel> data = ReadFromExcel(fileName);

            context.Delete(
                context.ClassScheduleDetails
                .Where(row => row.BranchID == branchID &&
                              row.Year == year &&
                              row.Month == month));

            foreach (var row in data)
            {
                Class cls = context.Classes.SingleOrDefault(item => item.Code == row.ClassCode);
                if (cls != null)
                {
                    ClassRoom room = context.ClassRooms.SingleOrDefault(item => item.Code == row.ClassRoomCode);
                    if (room != null)
                    {
                        Instructor inst = context.Instructors.SingleOrDefault(item => item.Barcode == row.InstructorBarcode);
                        if (inst != null)
                        {
                            TimeSpan startTime, endTime;
                            if (TimeSpan.TryParse(row.TimeStart, out startTime))
                            {
                                if (TimeSpan.TryParse(row.TimeEnd, out endTime))
                                {
                                    if (row.Level.Length <= 10)
                                    {
                                        if (row.DayOfWeek >= 1 && row.DayOfWeek <= 7)
                                        {
                                            ClassScheduleDetail csd = new ClassScheduleDetail();
                                            csd.BranchID = branchID;
                                            csd.Year = year;
                                            csd.Month = month;
                                            csd.DayOfWeek = row.DayOfWeek;
                                            csd.ClassID = cls.ID;
                                            csd.ClassRoomID = room.ID;
                                            csd.InstructorID = inst.ID;
                                            csd.TimeStart = row.TimeStart;
                                            csd.TimeEnd = row.TimeEnd;
                                            csd.Level = row.Level;
                                            csd.IsActive = true;
                                            EntityHelper.SetAuditFieldForInsert(csd, principal.Identity.Name);
                                            context.Add(csd);
                                        }
                                        else
                                        {
                                            throw new Exception("Wrong format in Excel file: day of week should be between 1 and 7, 1 for monday and 7 for sunday");
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception("Wrong format in Excel file: level should be less than 10 chars");
                                    }
                                }
                                else
                                {
                                    throw new Exception("Wrong format in Excel file: invalid time end for " + row.TimeEnd);
                                }
                            }
                            else
                            {
                                throw new Exception("Wrong format in Excel file: invalid time start for " + row.TimeStart);
                            }
                        }
                        else
                        {
                            throw new Exception("Wrong format in Excel file: No instructor defined for " + row.InstructorBarcode);
                        }
                    }
                    else
                    {
                        throw new Exception("Wrong format in Excel file: No class room defined for " + row.ClassRoomCode);
                    }
                }
                else
                {
                    throw new Exception("Wrong format in Excel file: No class defined for " + row.ClassCode);
                }
            }

            context.SaveChanges();
        }

        private IList<ExcelClassScheduleViewModel> ReadFromExcel(string fileName)
        {
            IList<ExcelClassScheduleViewModel> result = new List<ExcelClassScheduleViewModel>();

            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                HSSFWorkbook workbook = new HSSFWorkbook(fs);
                HSSFSheet sheet = workbook.GetSheetAt(0) as HSSFSheet;
                IEnumerator rows = sheet.GetRowEnumerator();
                int rowIndex = 1;
                while (rows.MoveNext())
                {
                    if (rowIndex > 1)
                    {
                        HSSFRow row = rows.Current as HSSFRow;
                        ExcelClassScheduleViewModel model = new ExcelClassScheduleViewModel();
                        model.DayOfWeek = Convert.ToInt32(row.Cells[0].StringCellValue);
                        model.ClassCode = row.Cells[1].StringCellValue;
                        model.Level = row.Cells[2].StringCellValue;
                        model.ClassRoomCode = row.Cells[3].StringCellValue;
                        model.InstructorBarcode = row.Cells[4].StringCellValue;
                        model.TimeStart = row.Cells[5].StringCellValue;
                        model.TimeEnd = row.Cells[6].StringCellValue;
                        result.Add(model);
                    }
                    rowIndex++;
                }
            }


            return result;
        }

        public void UpdateAttendancesStatus(int classRunningID, int runningInstructorID, int runningAssistantID, string notes, int[] values)
        {
            ClassRunning classRunning = context.ClassRunnings.SingleOrDefault(cls => cls.ID == classRunningID);
            if (classRunning != null)
            {
                classRunning.RunningInstructorID = runningInstructorID;
                classRunning.RunningAssistantID = runningAssistantID == 0 ? (int?)null : runningAssistantID;
                classRunning.Notes = notes;

                if (!classRunning.RunningStartWhen.HasValue)
                    classRunning.RunningStartWhen = DateTime.Now;

                var attendances = context.ClassAttendances.Where(cls => cls.ClassRunningID == classRunningID);
                foreach (var attendance in attendances)
                    attendance.IsAttend = false;

                foreach (var value in values)
                {
                    ClassAttendance attendance =
                        context.ClassAttendances.SingleOrDefault(
                            att => att.ClassRunningID == classRunningID && att.CustomerID == value);
                    if (attendance != null)
                    {
                        attendance.IsAttend = true;
                    }
                }

                context.SaveChanges();
            }
        }

        public ClassScheduleDetail GetSchedule(int id)
        {
            return context.ClassScheduleDetails.SingleOrDefault(sched => sched.ID == id);
        }

        public void UpdateSchedule(int id, int classID, int classRoomID, int instructorID, string level)
        {
            ClassScheduleDetail schedule = GetSchedule(id);
            if (schedule != null)
            {
                schedule.ClassID = classID;
                schedule.ClassRoomID = classRoomID;
                schedule.InstructorID = instructorID;
                schedule.Level = level;
                EntityHelper.SetAuditFieldForUpdate(schedule, principal.Identity.Name);
                context.SaveChanges();
            }
        }

        public bool VerifyScheduleDeletion(int id)
        {
            ClassScheduleDetail schedule = GetSchedule(id);
            if (schedule != null)
            {
                foreach (ClassRunning classRunning in schedule.ClassRunnings)
                {
                    if (classRunning.ClassAttendances.Count > 0)
                        return false;
                }
            }

            return true;
        }

        public void CopyScheduleFromLastMonth(int branchID, int year, int month)
        {
            DateTime lastPeriod = new DateTime(year, month, 1).AddMonths(-1);

            var schedules =
                context.ClassScheduleDetails.Where(
                    sch => sch.BranchID == branchID && sch.Year == lastPeriod.Year && sch.Month == lastPeriod.Month);

            context.Delete(
                context.ClassScheduleDetails.Where(sch => sch.BranchID == branchID && sch.Year == year && sch.Month == month));
            context.SaveChanges();

            foreach (var schedule in schedules)
            {
                var newSchedule = new ClassScheduleDetail();
                newSchedule.InstructorID = schedule.InstructorID;
                newSchedule.Level = schedule.Level;
                newSchedule.ClassRoomID = schedule.ClassRoomID;
                newSchedule.ClassID = schedule.ClassID;
                newSchedule.DayOfWeek = schedule.DayOfWeek;
                newSchedule.TimeStart = schedule.TimeStart;
                newSchedule.TimeEnd = schedule.TimeEnd;
                newSchedule.BranchID = schedule.BranchID;
                newSchedule.Year = year;
                newSchedule.Month = month;
                newSchedule.IsActive = schedule.IsActive;
                EntityHelper.SetAuditFieldForInsert(newSchedule, principal.Identity.Name);
                context.Add(newSchedule);
            }
            context.SaveChanges();
        }

        public void DeleteClassRoom(int[] id)
        {
            Array.ForEach(id, DeleteRoom);
        }
    }
}
