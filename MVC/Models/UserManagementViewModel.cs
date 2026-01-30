using Domain.Entities;
using Domain.Entities;
using System;
using System.Collections.Generic;

namespace MVC.Models.UserManagement
{
    public class UserManagementViewModel
    {
        public List<UserViewModel> Users { get; set; } = new List<UserViewModel>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public string SearchTerm { get; set; }
        public Role? RoleFilter { get; set; }
    }

    public class UserViewModel
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Role Role { get; set; }
        public bool IsVerfied { get; set; }
        public string Image { get; set; }
        public DateTime CreatedAt { get; set; }

        public string GetInitials()
        {
            if (string.IsNullOrWhiteSpace(FullName))
                return "??";

            var parts = FullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1)
                return parts[0].Substring(0, Math.Min(2, parts[0].Length)).ToUpper();

            return (parts[0][0].ToString() + parts[^1][0].ToString()).ToUpper();
        }

        public string GetStatusBadgeClass()
        {
            return IsVerfied ? "bg-emerald-100 text-emerald-700" : "bg-amber-100 text-amber-700";
        }

        public string GetStatusText()
        {
            return IsVerfied ? "Active" : "Pending";
        }

        public string GetRoleBadgeClass()
        {
            return Role switch
            {
                Role.Admin => "bg-red-100 text-red-700",
                Role.Instructor => "bg-amber-100 text-amber-700",
                Role.Student => "bg-emerald-100 text-emerald-700",
                _ => "bg-zinc-100 text-zinc-700"
            };
        }
    }
}