using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Task_BAL;
using Task_Model;

namespace TaskManagement.Controllers
{
    public class TaskController : Controller
    {

        BAL TaskBAL = new BAL();

        public ActionResult Index()
        {
            DataTable dt = TaskBAL.GetTaskList();
            return View(dt);
        }

        [HttpPost]
        public int SaveData(TaskModel Task)
        {
            try
            {
                Task.nUserId = int.Parse(System.Web.HttpContext.Current.Session["SESS_USERID"].ToString().Trim());
                return TaskBAL.CreateTask(Task); //Save And Update Fn.
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Delete(int nTaskId)
        {
            try
            {
                return TaskBAL.DeleteTask(nTaskId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
