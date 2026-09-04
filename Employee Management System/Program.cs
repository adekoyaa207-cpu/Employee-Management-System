using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Services;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .Build();

string? connectionString =
    configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    Console.WriteLine("Connection string was not found.");
    return;
}

DatabaseConnection database =
    new DatabaseConnection(connectionString);

Console.WriteLine("==========================================");
Console.WriteLine("       EMPLOYEE MANAGEMENT SYSTEM");
Console.WriteLine("==========================================");

Console.WriteLine();
Console.WriteLine("Testing database connection...");

if (database.TestConnection())
{
    Console.WriteLine("Database connection successful.");
}
else
{
    Console.WriteLine("Database connection failed.");
    Console.WriteLine("Please check your SQL Server and connection string.");
    return;
}

EmployeeService employeeService =
    new EmployeeService(database);

while (true)
{
    Console.WriteLine();
    Console.WriteLine("==========================================");
    Console.WriteLine("       EMPLOYEE MANAGEMENT SYSTEM");
    Console.WriteLine("==========================================");

    Console.WriteLine("1. Add Employee");
    Console.WriteLine("2. Update Employee Salary");
    Console.WriteLine("3. Delete Employee");
    Console.WriteLine("4. Find Employee By ID");
    Console.WriteLine("5. View All Employees");
    Console.WriteLine("6. Department Salary Report");
    Console.WriteLine("7. Exit");

    Console.WriteLine();
    Console.Write("Select Option: ");

    string? option = Console.ReadLine();

    Console.WriteLine();

    switch (option)
    {
        case "1":

            Console.Write("Enter Full Name: ");
            string fullName = Console.ReadLine() ?? "";

            Console.Write("Enter Department: ");
            string department = Console.ReadLine() ?? "";

            Console.Write("Enter Salary: ");

            if (decimal.TryParse(Console.ReadLine(), out decimal salary))
            {
                employeeService.AddEmployee(
                    fullName,
                    department,
                    salary);
            }
            else
            {
                Console.WriteLine("Invalid salary.");
            }

            break;


        case "2":

            Console.Write("Enter Employee ID: ");

            if (!int.TryParse(Console.ReadLine(), out int updateId))
            {
                Console.WriteLine("Invalid Employee ID.");
                break;
            }

            Console.Write("Enter New Salary: ");

            if (!decimal.TryParse(Console.ReadLine(), out decimal newSalary))
            {
                Console.WriteLine("Invalid salary.");
                break;
            }

            employeeService.UpdateEmployeeSalary(
                updateId,
                newSalary);

            break;


        case "3":

            Console.Write("Enter Employee ID: ");

            if (!int.TryParse(Console.ReadLine(), out int deleteId))
            {
                Console.WriteLine("Invalid Employee ID.");
                break;
            }

            employeeService.DeleteEmployee(deleteId);

            break;


        case "4":

            Console.Write("Enter Employee ID: ");

            if (!int.TryParse(Console.ReadLine(), out int findId))
            {
                Console.WriteLine("Invalid Employee ID.");
                break;
            }

            employeeService.FindEmployeeById(findId);

            break;


        case "5":

            employeeService.ViewAllEmployees();

            break;


        case "6":

            employeeService.DepartmentSalaryReport();

            break;


        case "7":

            Console.WriteLine("Thank you for using Employee Management System.");
            return;


        default:

            Console.WriteLine("Invalid option. Please select 1 - 7.");

            break;
    }

    Console.WriteLine();
    Console.WriteLine("Press ENTER to continue...");
    Console.ReadLine();

    Console.Clear();
}