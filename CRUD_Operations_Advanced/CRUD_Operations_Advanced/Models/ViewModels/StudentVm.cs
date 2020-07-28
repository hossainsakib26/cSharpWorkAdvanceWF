using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUD_Operations_Advanced.Models.ViewModels
{
    public class StudentVm
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public string Address { get; set; }
        public int DepartmentID { get; set; }

        public string Code { get; set; }
    }
}
