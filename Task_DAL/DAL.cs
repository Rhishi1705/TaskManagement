using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Task_Model;
using System.Security.Cryptography;

namespace Task_DAL
{
    public class DAL
    {
        public DataTable GetTaskList(int nStatus = 0)
        {
            try
            {
                DataTable rst = new DataTable();
                SqlParameter[] sqlParameters = new SqlParameter[1];

                sqlParameters[0] = new SqlParameter("@nStatus", System.Data.SqlDbType.VarChar);
                sqlParameters[0].Value = nStatus;

                rst = SelectDataTable("dbo.PR_TaskList", CommandType.StoredProcedure, sqlParameters);
                return rst;
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
                DataTable rst = new DataTable();
                SqlParameter[] sqlParameters = new SqlParameter[6];
                sqlParameters[0] = new SqlParameter("@nTaskId", System.Data.SqlDbType.VarChar);
                sqlParameters[0].Value = Task.nTaskId;
                sqlParameters[1] = new SqlParameter("@cTitle", System.Data.SqlDbType.VarChar);
                sqlParameters[1].Value = Task.cTitle;
                sqlParameters[2] = new SqlParameter("@cDecription", System.Data.SqlDbType.VarChar);
                sqlParameters[2].Value = Task.cDecription;
                sqlParameters[3] = new SqlParameter("@cDueDate", System.Data.SqlDbType.VarChar);
                sqlParameters[3].Value = Task.dDueDate.ToString("dd/MM/yyyy");
                sqlParameters[4] = new SqlParameter("@nStatus", System.Data.SqlDbType.VarChar);
                sqlParameters[4].Value = Task.nStatus;
                sqlParameters[5] = new SqlParameter("@nUserId", System.Data.SqlDbType.VarChar);
                sqlParameters[5].Value = Task.nUserId;

                rst = SelectDataTable("dbo.PR_TaskSave", CommandType.StoredProcedure, sqlParameters);
                if (rst.Rows.Count > 0)
                {
                     ret = Convert.ToInt32(rst.Rows[0][0]);
                }
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
                int ret = 0;
                DataTable rst = new DataTable();
                SqlParameter[] sqlParameters = new SqlParameter[1];
                sqlParameters[0] = new SqlParameter("@nTaskId", System.Data.SqlDbType.VarChar);
                sqlParameters[0].Value = nTaskId;

                rst = SelectDataTable("dbo.PR_TaskDelete", CommandType.StoredProcedure, sqlParameters);
                if (rst.Rows.Count > 0)
                {
                    ret = Convert.ToInt32(rst.Rows[0][0]);
                }
                return ret;
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
                string EncryptedPassWord = gethash(Login.cUserName + Login.cPassWord);

                DataTable rst = new DataTable();
                SqlParameter[] sqlParameters = new SqlParameter[2];
                sqlParameters[0] = new SqlParameter("@cUserName", System.Data.SqlDbType.VarChar);
                sqlParameters[0].Value = Login.cUserName;
                sqlParameters[1] = new SqlParameter("@cPassWord", System.Data.SqlDbType.VarChar);
                sqlParameters[1].Value = EncryptedPassWord;

                rst = SelectDataTable("dbo.PR_CheckLogin", CommandType.StoredProcedure, sqlParameters);
                return rst;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string gethash(string strinput)
        {
            string hashstring;
            using (MD5 md5Hash = MD5.Create())
            {        
                hashstring = GetMd5Hash(md5Hash, strinput);
            }
            return hashstring;
        }
        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash. 
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes 
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data  
            // and format each one as a hexadecimal string. 
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string. 
            return sBuilder.ToString();
        }


        public DataTable SelectDataTable(string queryName, CommandType queryType, SqlParameter[] parameters)
        {
            string connectionStringSetting = System.Configuration.ConfigurationManager.ConnectionStrings["IndConnection"].ConnectionString;

            DataTable result = new DataTable();
            if (connectionStringSetting == null)
            {
                //redirect to exceptionlayer
            }
            else
            {
                using (SqlConnection connection = new SqlConnection(connectionStringSetting))
                {
                    using (SqlCommand command = new SqlCommand(queryName, connection))
                    {

                        try
                        {
                            if (queryType == CommandType.StoredProcedure)
                            {
                                command.CommandType = queryType;
                                if (parameters != null)
                                {
                                    foreach (SqlParameter parameter in parameters)
                                    {
                                        command.Parameters.Add(parameter);
                                    }

                                    connection.Open();
                                    SqlDataReader dr = command.ExecuteReader();
                                    result.Load(dr);
                                }

                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        finally
                        {
                            if (connection.State != ConnectionState.Closed)
                            {
                                connection.Close();
                            }
                        }
                    }
                }
            }
            return result;
        }




    }
}
