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

// Retrieve a specific employee by their unique Id//
app.MapGet("/employees/{id}", (Guid id) =>
{
    var employee = employees.FirstOrDefault(employee => employee.Id 
    == id);
    if (employee is null){
      return Results.NotFound("Employee is NotFound ");
      
    }
  
  return Results.Ok(employee);
});

//Delete an employee by their unique Id//
    
    app.MapDelete("/employees/{id}", (Guid id ) => 
    {

    var employee = employees.FirstOrDefault(employee => employee.Id == id);
    if (employee is null){
      return Results.NotFound("Employee is NotFound ");
      
    }
    
    employees.Remove(employee);
    return Results.Ok($"Employee with ID {id} deleted");
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


<<<<<<< HEAD
=======


>>>>>>> 851f314ebbac4e99280adecdf9e5b99429962e5b
