var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseHttpsRedirection();
// create list of employees.
List<Employee> employees = new List<Employee>();


app.MapPut("/employees/{id}", (Guid id, Employee updatedEmployee) =>
{
    var employee = employees.FirstOrDefault(employee => employee.Id == id);
    if (employee == null)
    {
        return Results.NotFound($"Employee with this id {id} does not exists.");
    }
    employee.FirstName = updatedEmployee.FirstName ?? employee.FirstName;
    employee.LastName = updatedEmployee.LastName ?? employee.LastName;
    employee.Email = updatedEmployee.Email ?? employee.Email;

    return Results.Ok(employee);
});



app.Run();

class Employee
{
    public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Position { get; set; }
    public decimal Salary { get; set; }
    public DateTime CreatedAt { get; set; }
}




