namespace LibraryManagementSystem.Dtos.AdminDto
{
    public class CreateAdminDto
    {
        public required string AdminName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
