using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_Constants
{
    public class DataAccess
    {
        public static Database db;
        public DataAccess()
        {
            db = new Database(Constants.CONNECTIONSTRING);
        }
    }
}


