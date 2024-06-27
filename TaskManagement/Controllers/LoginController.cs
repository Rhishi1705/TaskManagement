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
    public class LoginController : Controller
    {
        BAL TaskBAL = new BAL();
        public ActionResult Index()
        {
            return View();
        }

        public int LoginCheck(LoginModel Login)
        {
            try
            {
                int ret = 0;
                DataTable dt = TaskBAL.LoginCheck(Login);
                if (dt.Rows.Count > 0)
                {
                    this.Session["SESS_USERID"] = dt.Rows[0]["nUserId"];
                    this.Session["SESS_USERNAME"] = dt.Rows[0]["cUserName"];
                    ret = 1;
                }
                else
                {
                    ret = 0;
                }
                return ret;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult CheckOut()
        {
            try
            {
                this.Session["SESS_USERID"] = "";
                this.Session["SESS_USERNAME"] = "";

                return RedirectToAction("Index", "Login");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
