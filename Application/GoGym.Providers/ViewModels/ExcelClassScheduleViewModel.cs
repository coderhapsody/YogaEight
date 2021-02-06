using System;

namespace GoGym.Providers.ViewModels
{
    public class ExcelClassScheduleViewModel
    {
        public int DayOfWeek { get; set; }
        public string ClassCode { get; set; }
        public string InstructorBarcode { get; set; }
        public string Level { get; set; }
        public string TimeStart { get; set; }
        public string TimeEnd { get; set; }
        public string ClassRoomCode { get; set; }
    }
}