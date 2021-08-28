using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.EntityModel;

namespace Models.DAO
{
    public class ConfigDAO
    {
        private static DBContext db = new DBContext();
        public static Config GetConfigByKey(string key)
        {
            return db.Configs.FirstOrDefault(m=>m.KeyName.Equals(key));
        }
        public static int SetConfigByKey(string key,string value)
        {
            try
            {
                Config config = db.Configs.FirstOrDefault(m=>m.KeyName.Equals(key));
                if(config != null)
                {
                    config.Value = value;
                    db.SaveChanges();
                }
                else
                {
                    Config newConfig = new Config()
                    {
                        KeyName = key,
                        Value = value
                    };
                    db.Configs.Add(newConfig);
                    db.SaveChanges();
                }
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
