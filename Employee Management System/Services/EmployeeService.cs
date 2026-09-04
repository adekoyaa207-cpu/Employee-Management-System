using EmployeeManagementSystem.Data;
using Microsoft.Data.SqlClient;

namespace EmployeeManagementSystem.Services
{
    public class EmployeeService
    {
        private readonly DatabaseConnection _database;

        public EmployeeService(DatabaseConnection database)
        {
            _database = database;
        }

        // 1. ADD EMPLOYEE
        public void AddEmployee(string fullName, string department, decimal salary)
        {
            try
            {
                using SqlConnection connection = _database.GetConnection();

                connection.Open();

                string query = @"
                    INSERT INTO Employee
                    (FullName, Department, Salary)
                    VALUES
                    (@FullName, @Department, @Salary)";

                using SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@FullName", fullName);
                command.Parameters.AddWithValue("@Department", department);
                command.Parameters.AddWithValue("@Salary", salary);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine("Employee added successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding employee: " + ex.Message);
            }
        }


        // 2. UPDATE EMPLOYEE SALARY
        public void UpdateEmployeeSalary(int employeeId, decimal newSalary)
        {
            try
            {
                using SqlConnection connection = _database.GetConnection();

                connection.Open();

                string query = @"
                    UPDATE Employee
                    SET Salary = @Salary
                    WHERE EmployeeId = @EmployeeId";

                using SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Salary", newSalary);
                command.Parameters.AddWithValue("@EmployeeId", employeeId);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine("Employee salary updated successfully.");
                }
                else
                {
                    Console.WriteLine("Employee not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating salary: " + ex.Message);
            }
        }


        // 3. DELETE EMPLOYEE
        public void DeleteEmployee(int employeeId)
        {
            try
            {
                using SqlConnection connection = _database.GetConnection();

                connection.Open();

                string query = @"
                    DELETE FROM Employee
                    WHERE EmployeeId = @EmployeeId";

                using SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@EmployeeId", employeeId);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine("Employee deleted successfully.");
                }
                else
                {
                    Console.WriteLine("Employee not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting employee: " + ex.Message);
            }
        }


        // 4. FIND EMPLOYEE BY ID
        public void FindEmployeeById(int employeeId)
        {
            try
            {
                using SqlConnection connection = _database.GetConnection();

                connection.Open();

                string query = @"
                    SELECT EmployeeId,
                           FullName,
                           Department,
                           Salary,
                           DateCreated
                    FROM Employee
                    WHERE EmployeeId = @EmployeeId";

                using SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@EmployeeId", employeeId);

                using SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    Console.WriteLine();
                    Console.WriteLine("Employee Found");
                    Console.WriteLine("----------------------------");
                    Console.WriteLine($"Employee ID : {reader["EmployeeId"]}");
                    Console.WriteLine($"Full Name   : {reader["FullName"]}");
                    Console.WriteLine($"Department  : {reader["Department"]}");
                    Console.WriteLine($"Salary      : {reader["Salary"]:N2}");
                    Console.WriteLine($"Date Created: {reader["DateCreated"]}");
                }
                else
                {
                    Console.WriteLine("Employee not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error finding employee: " + ex.Message);
            }
        }


        // 5. VIEW ALL EMPLOYEES
        public void ViewAllEmployees()
        {
            try
            {
                using SqlConnection connection = _database.GetConnection();

                connection.Open();

                string query = @"
                    SELECT EmployeeId,
                           FullName,
                           Department,
                           Salary
                    FROM Employee
                    ORDER BY EmployeeId";

                using SqlCommand command = new SqlCommand(query, connection);

                using SqlDataReader reader = command.ExecuteReader();

                Console.WriteLine();
                Console.WriteLine("ALL EMPLOYEES");
                Console.WriteLine("-------------------------------------------------------------");
                Console.WriteLine(
                    "{0,-5} {1,-20} {2,-15} {3,12}",
                    "ID",
                    "Name",
                    "Department",
                    "Salary");

                Console.WriteLine("-------------------------------------------------------------");

                while (reader.Read())
                {
                    Console.WriteLine(
                        "{0,-5} {1,-20} {2,-15} {3,12:N2}",
                        reader["EmployeeId"],
                        reader["FullName"],
                        reader["Department"],
                        reader["Salary"]);
                }

                Console.WriteLine("-------------------------------------------------------------");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error displaying employees: " + ex.Message);
            }
        }


        // 6. DEPARTMENT SALARY REPORT
        public void DepartmentSalaryReport()
        {
            try
            {
                using SqlConnection connection = _database.GetConnection();

                connection.Open();

                string query = @"
                    SELECT
                        Department,
                        COUNT(*) AS Employees,
                        SUM(Salary) AS TotalSalary,
                        AVG(Salary) AS AverageSalary
                    FROM Employee
                    GROUP BY Department
                    ORDER BY Department";

                using SqlDataAdapter adapter = new SqlDataAdapter(query, connection);

                System.Data.DataTable table = new System.Data.DataTable();

                adapter.Fill(table);

                Console.WriteLine();
                Console.WriteLine("DEPARTMENT SALARY REPORT");
                Console.WriteLine("----------------------------------------------------------------");

                Console.WriteLine(
                    "{0,-15} {1,-12} {2,-15} {3,-15}",
                    "Department",
                    "Employees",
                    "Total Salary",
                    "Average Salary");

                Console.WriteLine("----------------------------------------------------------------");

                foreach (System.Data.DataRow row in table.Rows)
                {
                    Console.WriteLine(
                        "{0,-15} {1,-12} {2,-15:N2} {3,-15:N2}",
                        row["Department"],
                        row["Employees"],
                        row["TotalSalary"],
                        row["AverageSalary"]);
                }

                Console.WriteLine("----------------------------------------------------------------");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error generating report: " + ex.Message);
            }
        }
    }
}