using System.ComponentModel.DataAnnotations;

public class Teachers
{
    [Key]
    public int TeacherID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime HireDate { get; set; }
    public decimal Salary { get; set; }
    public string Department { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
}
