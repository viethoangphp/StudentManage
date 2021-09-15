using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudentManage.Models;
using Models.EntityModel;
using Models.DAO;

namespace StudentManage.BUS
{
    public class ConfigBUS
    {
        public static string GetConfigByKey(string key)
        {
            Config config = ConfigDAO.GetConfigByKey(key);
            if(config != null)
            {
                return config.Value;
            }
            return null;
        }
        public static int SetConfigByKey(string key,string value)
        {
            return ConfigDAO.SetConfigByKey(key, value);
        }
    }
}