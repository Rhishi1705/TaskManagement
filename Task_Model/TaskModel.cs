using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_Model
{
    public class TaskModel
    {
        public int nTaskId { get; set; }
        public string cTitle { get; set; }
        public string cDecription { get; set; }
        public DateTime dDueDate { get; set; }
        public int nStatus { get; set; }
        public int nUserId { get; set; }
        public bool bCancelled { get; set; }
    }
    public class LoginModel
    {
        public string cUserName { get; set; }
        public string cPassWord { get; set; }
    }
}
