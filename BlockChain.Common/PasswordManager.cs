using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.Common
{

    /// <summary>
    /// 把密码保存到内存中，输入key和password，其中key一般是地址，或者助记词的key
    /// </summary>
   public class PasswordManager
    {
        private static object LockObj = new object();

        private static Hashtable ht = new Hashtable();

        public static bool SavePassword(string key, string password)
        {
            lock(LockObj)
            {
                key = key.ToUpper();
                if (ht[key] == null)
                {
                    Common.PasswordHelper ph = new Common.PasswordHelper(password);
                    ht.Add(key, ph);
                    return true;
                }
                else //if (ht[key] != null)
                {
                    Common.PasswordHelper oldph = (Common.PasswordHelper)ht[key];
                    if (oldph.GetPassword() != password)
                    {
                        Common.PasswordHelper newph = new Common.PasswordHelper(password);
                        ht[key] = newph;
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool GetPassword(string key, out string password)
        {
            lock (LockObj)
            {
                password = string.Empty;
                key = key.ToUpper();

                if (ht[key] != null)
                {
                    Common.PasswordHelper ph = (Common.PasswordHelper) ht[key];
                    password = ph.GetPassword();
                    return true;
                }
            }
            return false;
        }


        public static bool RemovePassword(string key)
        {
            lock (LockObj)
            {
                key = key.ToUpper();
                if (ht[key] != null)
                {
                    ht.Remove(key);
                    return true;
                }
            }
            return false;
        }

    }

}
