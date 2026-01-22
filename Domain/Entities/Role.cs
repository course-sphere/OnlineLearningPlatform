namespace Domain.Entities
{
    public class Role
    {
        public Guid RoleId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public List<UserRoleMapping>? Roles { get; set; } = new();
    }
}
