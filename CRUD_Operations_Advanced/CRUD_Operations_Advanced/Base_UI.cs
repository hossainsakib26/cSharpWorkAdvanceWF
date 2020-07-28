using CRUD_Operations_Advanced.DAL;
using CRUD_Operations_Advanced.Models;
using CRUD_Operations_Advanced.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUD_Operations_Advanced
{
    public partial class Base_Form_UI : Form
    {
        public Base_Form_UI()
        {
            InitializeComponent();
            LoadData();

        }

        Department dept;
        Student std;
        //StudentVm stdVm;

        DBContext db = new DBContext();
        
        int id = 0;
        private void SaveButton_Click(object sender, EventArgs e)
        {
            var comboDeptObj = departmentComboBox.SelectedItem as Department;
            std = new Student();

            std.ID = db.GetStdSqId("SQ_StudentId");
            std.Name = nameTextBox.Text;
            std.Email = emailTextBox.Text;
            std.Contact = contactTextBox.Text;
            std.Address = addressTextBox.Text;

            if (comboDeptObj.ID > 0)
            {
                std.DepartmentID = comboDeptObj.ID;
            }
            else
            {
                MessageBox.Show("Combobox have no value!");
            }

            if (SaveButton.Text == "Save")
            {
                var isSave = db.SaveStudent(std);
                if (isSave)
                {
                    MessageBox.Show($"New student name {std.Name} is successfully added!");
                    clear();
                    LoadData();
                }
                else
                {
                    MessageBox.Show($"New student name {std.Name} is not added!");
                }
            }
            else if (SaveButton.Text == "Update" && std.ID > 0)
            {
                var isUpdate = db.UpdateStudent(std.ID, std);
                if (isUpdate)
                {
                    MessageBox.Show($"{std.ID} is update successfully!");
                    clear();
                    LoadData();
                    SaveButton.Text = "Save";
                }
                else
                {
                    MessageBox.Show($"{std.ID} is not update!");
                }
            }

        }

        private void SaveDeptButton_Click(object sender, EventArgs e)
        {
            dept = new Department();

            dept.ID = db.GetSeqId("SQ_DepartmentId");
            dept.Name = dNameTextBox.Text;
            dept.Code = dCodeTextBox.Text;

            var isSave = db.SaveDepartment(dept);
            if (isSave)
            {
                MessageBox.Show($"{dept.Name} is successfully added in the database!");
                clear();
                LoadData();
            }
            else
            {
                MessageBox.Show($"{dept.Name} is not added in the database!");
            }
        }

        void clear()
        {
            dNameTextBox.Text = "";
            dCodeTextBox.Text = "";
            nameTextBox.Text = "";
            emailTextBox.Text = "";
            contactTextBox.Text = "";
            addressTextBox.Text = "";
        }

        private void LoadData()
        {
            departmentBindingSource1.DataSource = null;
            departmentBindingSource1.DataSource = db.GetDepartments();

            departmentBindingSource.DataSource = null;
            departmentBindingSource.DataSource = db.GetDepartments();

            studentVmBindingSource.DataSource = null;
            studentVmBindingSource.DataSource = db.GetStudents();
        }

        private void studentDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            SaveButton.Text = "Update";
            
           
            //get the row index
            DataGridViewRow row = studentDataGridView.Rows[e.RowIndex];
            id = Convert.ToInt32(row.Cells[0].Value);

            std = new Student();

            std = db.GetStudentById(id);
            if (std != null)
            {
                nameTextBox.Text = std.Name;
                emailTextBox.Text = std.Email;
                contactTextBox.Text = std.Contact;
                addressTextBox.Text = std.Address;
                departmentComboBox.SelectedIndex = std.DepartmentID;
            }
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            var stdVm = new StudentVm();
            stdVm.Name = searchTextBox.Text;

            if (db.SearchStudents(stdVm.Name) != null)
            {
                studentVmBindingSource.DataSource = null;
                studentVmBindingSource.DataSource = db.SearchStudents(stdVm.Name);
            }
        }
    }
}
