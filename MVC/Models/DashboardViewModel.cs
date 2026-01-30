using System;
using System.Collections.Generic;

namespace MVC.Models.Dashboard
{
    public class DashboardViewModel
    {
        // ===== Cards =====
        public int TotalUsers { get; set; }
        public int TotalInstructors { get; set; }
        public int TotalStudents { get; set; }

        public int TotalCourses { get; set; }
        public int ActiveCourses { get; set; }

        public int TotalEnrollments { get; set; }
        public decimal TotalRevenue { get; set; }

        // ===== Table =====
        public List<RecentActivityViewModel> RecentActivities { get; set; }
            = new List<RecentActivityViewModel>();
        public List<LatestEnrollmentVM> LatestEnrollments { get; set; }
        = new List<LatestEnrollmentVM>();
    }

    public class RecentActivityViewModel
    {
        public DateTime Time { get; set; }
        public string UserEmail { get; set; }
        public string Activity { get; set; }
        public decimal? Amount { get; set; }
        public string Status { get; set; }
    }
    public class LatestEnrollmentVM
    {
        public DateTime? EnrolledAt { get; set; }
        public string UserEmail { get; set; }
        public string CourseName { get; set; }
        public decimal Progress { get; set; }
        public string Status { get; set; }


    }
}