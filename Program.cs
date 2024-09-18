var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseHttpsRedirection();

// create list of employees.
List<Employee> employees = new List<Employee>();

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


