using System;
using System.Runtime.InteropServices;
using System.Security;

namespace BlockChain.Common
{

    /// <summary>
    /// 密码强度
    /// </summary>
    public enum Strength
    {
        Invalid = 0,    //无效密码
        Weak = 1,       //低强度密码
        Normal = 2,     //中强度密码
        Strong = 3      //高强度密码
    };

    public class PasswordHelper
    {
      
        /// <summary>
        /// 计算密码强度
        /// </summary>
        /// <param name="password">密码字符串</param>
        /// <returns></returns>
        public static Strength PasswordStrength(string password)
        {
            //空字符串强度值为0
            if (password == "") return Strength.Invalid;
            //字符统计
            int iNum = 0, iLtt = 0, iSym = 0;
            foreach (char c in password)
            {
                if (c >= '0' && c <= '9') iNum++;
                else if (c >= 'a' && c <= 'z') iLtt++;
                else if (c >= 'A' && c <= 'Z') iLtt++;
                else iSym++;
            }
            if (iLtt == 0 && iSym == 0) return Strength.Weak; //纯数字密码
            if (iNum == 0 && iLtt == 0) return Strength.Weak; //纯符号密码
            if (iNum == 0 && iSym == 0) return Strength.Weak; //纯字母密码
            if (password.Length <= 6) return Strength.Weak; //长度不大于6的密码
            if (iLtt == 0) return Strength.Normal; //数字和符号构成的密码
            if (iSym == 0) return Strength.Normal; //数字和字母构成的密码
            if (iNum == 0) return Strength.Normal; //字母和符号构成的密码
            if (password.Length <= 10) return Strength.Normal; //长度不大于10的密码
            return Strength.Strong; //由数字、字母、符号构成的密码
        }


        public static string GetHDRealPassword(string salt, string password)
        {
            return salt + password;
        }


      
        private SecureString SecurePassword;

        public PasswordHelper(string password)
        {
            SecurePassword  = new SecureString();
            foreach (char c in password.ToCharArray())
            {
                SecurePassword.AppendChar(c);
            }
        }


        public String GetPassword()
        {
            if (null == SecurePassword)
            {
                return string.Empty;
            }

            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToBSTR(SecurePassword);
                return Marshal.PtrToStringBSTR(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeBSTR(valuePtr);
            }
        }

        

    }
}
