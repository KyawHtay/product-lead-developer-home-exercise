using UKParliament.CodeTest.Data;

namespace UKParliament.CodeTest.Web.ViewModels;

public class PersonViewModel
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required DateOnly DateOfBirth { get; set; }
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
}