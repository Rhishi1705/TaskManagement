using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_DAL;
using Task_Model;

namespace Task_BAL
{
     
    public class BAL
    {
        DAL TaskDAL = new DAL();

        public DataTable GetTaskList()
        {
            try
            {
                return TaskDAL.GetTaskList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CreateTask(TaskModel Task)
        {
            try
            {
                int ret = 0;
                ret = TaskDAL.CreateTask(Task);//Create Function

                return ret;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeleteTask(int nTaskId)
        {
            try
            {
                return TaskDAL.DeleteTask(nTaskId);//Delete Function
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable LoginCheck(LoginModel Login)
        {
            try
            {
                return TaskDAL.LoginCheck(Login);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
