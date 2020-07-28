using CRUD_Operations_Advanced.Models;
using CRUD_Operations_Advanced.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUD_Operations_Advanced.DAL
{
    public class DBContext
    {
        //connection db
        //step: 1
        // => meaning of this sign is get property
        private static string ConString => @"SERVER = DESKTOP-AK2N3HV\SAZZADSQL; DATABASE = AdvancedCsharpDB; INTEGRATED SECURITY = True;";
        SqlConnection _con = new SqlConnection(ConString);

        //department
        public bool SaveDepartment(Department dept)
        {
            //string query = $@"INSERT INTO Departments ( name, code ) VALUES (@Id, @Name, @Code )";
            string query = $@"SP_SaveDepartment";
            SqlCommand command = new SqlCommand(query, _con);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Id", dept.ID);
            command.Parameters.AddWithValue("@Name", dept.Name); // @Name no need to match with database column's name.
            command.Parameters.AddWithValue("@Code", dept.Code); // @Code no need to match with database column's name.

            var isSave = SaveChange(command);
            return isSave;
        }

        public int GetSeqId(string sequenceName)
        {
            if (string.IsNullOrEmpty(sequenceName))
            {
                throw new Exception("Empty id");
            }
            string query = $@"SELECT NEXT VALUE FOR {sequenceName}";
            var command = new SqlCommand(query, _con);
            _con.Open();
            var _id = command.ExecuteScalar();
            _con.Close();
            if (_id != null)
            {
                return Convert.ToInt32(_id);
            }
            throw new Exception("Id is not valid");
        }

        public List<Department> GetDepartments()
        {
            string query = $@"Select * From Departments";
            SqlCommand command = new SqlCommand(query, _con);

            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            _con.Open();
            da.Fill(dt);
            _con.Close();

            var departments = new List<Department>();

            foreach (DataRow row in dt.Rows)
            {
                var department = new Department
                {
                    ID = Convert.ToInt32(row["id"]),
                    Name = row["name"].ToString(),
                    Code = row["code"].ToString()
                };
                departments.Add(department);
            }
            return departments;
        }

        //student code
        public bool SaveStudent(Student student)
        {
            string query = $@"SP_SaveStudent";
            SqlCommand command = new SqlCommand(query, _con);
            command.CommandType = CommandType.StoredProcedure; //this command must use when we use store procedure
            command.Parameters.AddWithValue("@Id", student.ID);
            command.Parameters.AddWithValue("@Name", student.Name);
            command.Parameters.AddWithValue("@Email", student.Email);
            command.Parameters.AddWithValue("@ContactNumber", student.Contact);
            command.Parameters.AddWithValue("@Address", student.Address);
            command.Parameters.AddWithValue("@DepartmentId", student.DepartmentID);
            
            var isSave = SaveChange(command);
            return isSave;
        }

        public int GetStdSqId(string sequenceName)
        {
            if (string.IsNullOrEmpty(sequenceName))
            {
                throw new Exception("Empty student id");
            }
            string query = $@"SELECT NEXT VALUE FOR {sequenceName}";
            var command = new SqlCommand(query, _con);
            _con.Open();
            var _id = command.ExecuteScalar();
            _con.Close();
            if (_id != null)
            {
                return Convert.ToInt32(_id);
            }
            else
            {
                throw new Exception("Do not get ID.");
            }
        }

        public DataTable GetStudents()
        {
            string query = $@"Select * From VW_Students";
            SqlCommand command = new SqlCommand(query, _con);

            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            _con.Open();
            da.Fill(dt);
            _con.Close();

            return dt;
        }

        public Student GetStudentById(int id)
        {
            string query = $@"SELECT * FROM Students WHERE id = ('{id}')";
            SqlCommand command = new SqlCommand(query, _con);
            _con.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                var student = new Student
                {
                    ID = Convert.ToInt32(reader["id"]),
                    Name = reader["name"].ToString(),
                    DepartmentID = Convert.ToInt32(reader["departmentId"]),
                    Address = reader["address"].ToString(),
                    Contact = reader["contactNumber"].ToString(),
                    Email = reader["email"].ToString()
                };

                if(student.ID > 0)
                {
                    _con.Close();
                    return student;
                }
            }
            _con.Close();
            return null;
        }

        public bool UpdateStudent(int id, Student std)
        {
            string query = $@"UPDATE Students SET name = '{std.Name}', email = '{std.Email}', contactNumber = '{std.Contact}', address = '{std.Address}', departmentId = '{std.DepartmentID}' WHERE id = '{id}'";
            SqlCommand command = new SqlCommand(query, _con);
            command.Parameters.AddWithValue("@Name", std.Name);
            command.Parameters.AddWithValue("@Email", std.Email);
            command.Parameters.AddWithValue("@ContactNo", std.Contact);
            command.Parameters.AddWithValue("@Address", std.Address);
            command.Parameters.AddWithValue("@DepartmentId", std.DepartmentID);
            if (SaveChange(command))
            {
                return true;
            }            
            return false;
        }

        //search student using StudentVm
        public List<StudentVm> SearchStudents(string name)
        {
            var studentNameParams = !string.IsNullOrEmpty(name) ? $@"'%{name}%'" : "''";
            string query = $@"Select * From VW_Students WHERE name Like {studentNameParams}";
            SqlDataAdapter adapter = new SqlDataAdapter(query, _con);
            DataTable dt = new DataTable();
            _con.Open();
            adapter.Fill(dt);
            _con.Close();

            var stds = new List<StudentVm>();
            foreach (DataRow dr in dt.Rows)
            {
                var std = new StudentVm
                {
                    ID = Convert.ToInt32(dr["id"]),
                    Name = dr["name"].ToString(),
                    Contact = dr["contactNumber"].ToString(),
                    Email = dr["email"].ToString(),
                    Address = dr["address"].ToString(),
                    Code = dr["code"].ToString(),
                };

                if (std.ID > 0)
                {
                    stds.Add(std);
                }
            }
            if (stds != null)
            {
                return stds;
            }
            else
            {
                return null;
            }

        }

        //method for all save changes
        private bool SaveChange(SqlCommand command)
        {
            _con.Open();
            int rowAffected = command.ExecuteNonQuery();
            _con.Close();
            if (rowAffected > 0)
            {
                return true;
            }
            return false;
        }
    }
}
