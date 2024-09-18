using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseHttpsRedirection();
// create list of employees.
List<Employee> employees = new List<Employee>();

bool isEmail(string email)
{
    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
    RegexOptions.IgnoreCase);

    if (isEmail)
    {
        return true;
    }
    return false;
}

// GET "/employees" =>  Retrieve a list of employees with pagination
app.MapGet("/employees", (HttpRequest request) =>
{
    // read query from the request
    int page = int.TryParse(request.Query["page"], out int parsedPage) ? parsedPage : 1;
    int limit = int.TryParse(request.Query["limit"], out int parsedLimit) ? parsedLimit : 3;

    // pagination
    int skip = (page - 1) * limit;
    var paginatedEmployee = employees.Skip(skip).Take(limit);

    return Results.Ok(paginatedEmployee);

});

// POST "/employees" => Create a new employee.
app.MapPost("/employees", (Employee newEmployee) =>
{
    newEmployee.CreatedAt = DateTime.Now;
    newEmployee.Id = Guid.NewGuid();

    // check if FirstName, LastName, and Position are null/empty or not
    if (string.IsNullOrEmpty(newEmployee.FirstName) ||
    string.IsNullOrEmpty(newEmployee.LastName) ||
    string.IsNullOrEmpty(newEmployee.Position) ||
    string.IsNullOrEmpty(newEmployee.Email))
    {
        return Results.BadRequest("FirstName, LastName, and Position should not be null or empty.");
    }

    // check if email is valid 
    if (!isEmail(newEmployee.Email))
    {
        return Results.BadRequest("Email should be valid.");
    }

    // check if email is unique
    var foundEmployee = employees.FirstOrDefault(employee => employee.Email == newEmployee.Email);
    if (foundEmployee != null)
    {
        return Results.NotFound("Email should be unique.");
    }

    // check if salary is positive or not
    if (newEmployee.Salary < 0)
    {
        return Results.BadRequest("Salary should be positive.");
    }
    employees.Add(newEmployee);
    return Results.Created("New employee added successfully.", newEmployee);
});

// Put "/employees/{id}" => update the employee by Id.
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
