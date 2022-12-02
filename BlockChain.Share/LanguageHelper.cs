using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace BlockChain.Share
{

    /// <summary>
    /// 语言，采用 CULTURE  
    /// </summary>
    public class LanguageHelper : IValueConverter
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// 界面的原始语言，是 中文，不需要翻译的
        /// </summary>
        public const string OriginalLanguageCode = "zh-Hans";


        #region IValueConverter

        /// <summary>
        /// 译文转换
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter">原文</param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //设计时不要翻译， System.ComponentModel.LicenseContext
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
            {
                if (parameter != null)
                {
                    return (string)parameter;
                }
                return "???";
            }

            if (parameter != null)
            {
                string OriginalText = parameter.ToString();
                var result = GetTranslationText(OriginalText);
                return result;
            }
            return "***";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("The method or operation is not implemented.");
        }

        #endregion


        /// <summary>
        /// 得到系统的语言代码(CultureInfo.Name)，例如 zh-Hans en  ， 最长 7 位字符 
        /// 参见： https://docs.microsoft.com/zh-cn/dotnet/api/system.globalization.cultureinfo.name?view=net-6.0  ci.Name
        /// </summary>
        /// <returns></returns>
        public static CultureInfo GetSysCultureInfo()
        {
            var lan = CultureInfo.CurrentCulture;           // System.Threading.Thread.CurrentThread.CurrentCulture;
            if (lan.Parent != null) {
                lan = lan.Parent;                           // 返回不带区域的语言
            }
            return lan;
        }
        

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////


        /// <summary>
        /// 初始化 T_Language 的数据
        /// </summary>
        public static void IniLangageData()
        {
            DeleteLanguage();

            var SysLan = GetSysCultureInfo();

            foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.AllCultures))
            {
                if (string.IsNullOrWhiteSpace(ci.Name))
                {
                    continue;
                }

                Model.T_Language m = new Model.T_Language();
                m.CultureInfoName = ci.Name;
                m.TwoLetterISOLanguageName = ci.TwoLetterISOLanguageName;
                m.ThreeLetterWindowsLanguageName = ci.ThreeLetterWindowsLanguageName;
                m.ThreeLetterISOLanguageName = ci.ThreeLetterISOLanguageName;
                m.DisplayName = ci.DisplayName;
                m.EnglishName = ci.EnglishName;
                m.NativeName = ci.NativeName;
                m.IsSelected = ci.LCID == SysLan.LCID;
                m.LCID = ci.LCID;

                m.ValidateEmptyAndLen();

                DAL.T_Language.Insert(Share.ShareParam.DbConStr, m);
            }
        }

        private static void DeleteLanguage()
        {
            string sql = @"DELETE FROM T_Language";

            SqlConnection cn = new SqlConnection(Share.ShareParam.DbConStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cn.Open();
            try
            {
                cm.ExecuteNonQuery();
            }
            finally
            {
                cn.Close();
            }
        }


        public static void UpdateSelectedLanguage(string selectedName)
        {
            string sql = @"
UPDATE  T_Language
SET         IsSelected = 0;

UPDATE  T_Language
SET         IsSelected = 1
WHERE   (CultureInfoName = @CultureInfoName);
";

            SqlConnection cn = new SqlConnection(Share.ShareParam.DbConStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;
            cm.Parameters.Add("@CultureInfoName", SqlDbType.NVarChar, 32).Value = selectedName;

            cn.Open();
            try
            {
                cm.ExecuteNonQuery();
            }
            finally
            {
                cn.Close();
            }
        }


        public static Model.T_Language? GetSelectedModel()
        {
            string conStr = Share.ShareParam.DbConStr;
            string sql = @"select * from T_Language Where IsSelected = 1";

            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cm;
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count == 1)
            {
                return Model.T_Language.DataRow2Object(ds.Tables[0].Rows[0]);
            }
            else if (ds.Tables[0].Rows.Count > 1)
            {
                throw new Exception("数据错误，对应多条记录！");
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 更新 译文数量 
        /// </summary>
        public static void UpdateItemsNumber()
        {
            string conStr = Share.ShareParam.DbConStr;
            string sql = @"
UPDATE  T_Language SET ItemsNumber = (SELECT COUNT(*) AS Expr1 FROM T_TranslationText WHERE (LanCode = T_Language.CultureInfoName)) WHERE (CultureInfoName <> 'zh-Hans');
UPDATE  T_Language SET ItemsNumber = (SELECT COUNT(*) AS Expr1 FROM T_OriginalText) WHERE (CultureInfoName = 'zh-Hans');
";

            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            cn.Open();
            try
            {
                cm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                log.Error("Update T_Language ItemsNumber", ex);
            }
            finally
            {
                cn.Close();
            }
        }

        public static DataTable GetLanguageList()
        {
            UpdateItemsNumber();

            string conStr = Share.ShareParam.DbConStr;
            string sql = @"select * from T_Language";

            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cm;
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);
            return ds.Tables[0];
        }


        /// <summary>
        /// 得到某种语言的所有译文列表， 可以用于译文导出
        /// </summary>
        /// <param name="_lancode"></param>
        /// <returns></returns>
        public static DataSet GetLanAllTranslationText(string _lancode)
        {
            string conStr = Share.ShareParam.DbConStr;
            string sql = @"
SELECT  *
FROM      T_TranslationText
WHERE   (LanCode = @LanCode)
";

            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;
            cm.Parameters.Add("@LanCode", SqlDbType.NVarChar, 32).Value = _lancode;

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cm;
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds);
            return ds;
        }


        /// <summary>
        /// 得到App设置的语言 
        /// </summary>
        /// <returns></returns>
        public static string GetAppLanguageCode()
        {
            string key = "{349362C9-8496-408D-BB3A-711CEF15CC6D}";
            var result = Common.Cache.GetData(key);
            if (result == null)
            {
                result = _GetAppLanguageCode();
                Common.Cache.AddByAbsoluteTime(key, result);
            }
            return (string)result;
        }

        private static string _GetAppLanguageCode()
        {
            var m = GetSelectedModel();
            if (m != null)
            {
                return m.CultureInfoName;
            }
            else
            {
                return GetSysCultureInfo().Name;
            }
        }

        public static string GetTranslationText(string originalText)
        {
            //设计时不要翻译，
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
            {
                return originalText;
            }

            // 中文（简体）（zh-Hans）不需要保存 
            var lan = GetAppLanguageCode();
            if (lan == OriginalLanguageCode || lan.ToLower() == OriginalLanguageCode.ToLower()) {
                return originalText;
            }

            string key = originalText + "{35469139-3A49-4412-A255-1E880F40E931}";
            var result = Common.Cache.GetData<string>(key);
            if (string.IsNullOrEmpty(result))
            {
                result = _GetTranslationText(originalText);
                Common.Cache.AddByAbsoluteTime(key, result);
            }
            return result;
        }

        private static string _GetTranslationText(string originalText)
        {
            try
            {
                var lan = GetAppLanguageCode();

                var hash = Common.Security.Tools.GetHashValue(originalText);
                if (!DAL.T_OriginalText.Exist(Share.ShareParam.DbConStr, hash))
                {
                    Model.T_OriginalText mo = new Model.T_OriginalText();
                    mo.OriginalHash = hash;
                    mo.OriginalText = originalText;
                    mo.ValidateEmptyAndLen();

                    DAL.T_OriginalText.Insert(Share.ShareParam.DbConStr, mo);
                }

                //所有的数据都备份，方便以后全部删除原始表（T_OriginalText 和 T_TranslationText）数据。
                //1，(原文表)处理备份！
                if (!DAL.T_OriginalText_BAK.Exist(Share.ShareParam.DbConStr, hash))
                {
                    Model.T_OriginalText_BAK mo = new Model.T_OriginalText_BAK();
                    mo.OriginalHash = hash;
                    mo.OriginalText = originalText;
                    mo.ValidateEmptyAndLen();

                    DAL.T_OriginalText_BAK.Insert(Share.ShareParam.DbConStr, mo);
                }

                try
                {
                    //1.1 记录引用位置 
                    //index:0为本身的方法；1为调用方法；2为其上上层， //本文地址:http://www.cnblogs.com/Interkey/p/GetMethodName.html  //StackFrame[] sfs = ss.GetFrames();
                    StackTrace ss = new StackTrace(true);
                    var frame = ss.GetFrame(3);                     
                    if (frame != null)
                    {
                        MethodBase mb = frame.GetMethod();
                        if (mb != null)
                        {
                            if (mb.DeclaringType != null)
                            {
                                string refrence = mb.DeclaringType.Namespace + "." + mb.DeclaringType.Name + "." + mb.Name;             //哪个地方使用的！！！
                                string RefHash = Common.Security.Tools.GetHashValue(refrence);

                                if (!DAL.T_Refrence.Exist(Share.ShareParam.DbConStr, hash, RefHash))
                                {
                                    Model.T_Refrence mr = new Model.T_Refrence();
                                    mr.OriginalHash = hash;
                                    mr.RefrenceFormHash = RefHash;
                                    mr.RefrenceForm = refrence;

                                    DAL.T_Refrence.Insert(Share.ShareParam.DbConStr, mr);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error("", ex);
                }

                var mt = DAL.T_TranslationText.GetModel(Share.ShareParam.DbConStr, hash, lan);
                var mt_bak = DAL.T_TranslationText_BAK.GetModel(Share.ShareParam.DbConStr, hash, lan);

                if (mt == null)
                {
                    mt = new Model.T_TranslationText();
                    mt.LanCode = lan;
                    mt.OriginalHash = hash;

                    // 先从 备份表 读取，再写入译文表。
                    if (mt_bak != null && !string.IsNullOrEmpty(mt_bak.TranslationText))
                    {
                        mt.TranslationText = mt_bak.TranslationText;                    
                    }
                    else
                    {
                        //原来的方法：打个 问号 标记； 现在的方法：直接从微软查询
                        if (IsTranslateOnLine)
                        {
                            mt.TranslationText = MicrosoftTextTranslation.TranslatFromZh(originalText, lan).Result;      //new  这里可能需要一定时间，秒级
                        }
                        else
                        {                         
                            mt.TranslationText = "?" + originalText;                                                            //old                                   
                        }
                    }

                    mt.ValidateEmptyAndLen();
                    DAL.T_TranslationText.Insert(Share.ShareParam.DbConStr, mt);
                }

                //2，(译文表)处理备份！
                if (mt_bak == null)
                {
                    mt_bak = new Model.T_TranslationText_BAK();
                    mt_bak.LanCode = lan;
                    mt_bak.OriginalHash = hash;
                    mt_bak.TranslationText = mt.TranslationText;
                    mt_bak.ValidateEmptyAndLen();
                    DAL.T_TranslationText_BAK.Insert(Share.ShareParam.DbConStr, mt_bak);
                }
                else
                {
                    if (mt_bak.TranslationText != mt.TranslationText)
                    {
                        mt_bak.TranslationText = mt.TranslationText;
                        DAL.T_TranslationText_BAK.Update(Share.ShareParam.DbConStr, mt_bak);
                    }
                }

                return mt.TranslationText;
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                return "**" + originalText;
            }
        }



        /// <summary>
        /// 是否使用在线翻译
        /// </summary>
        public static bool IsTranslateOnLine
        {
            get {
                return Properties.Settings.Default.IsTranslateOnLine;
            }
            set {
                Properties.Settings.Default.IsTranslateOnLine = value;
                Properties.Settings.Default.Save();
            }
        
        }


        public static void DeleteAllTranslateText(string _LanCode) 
        {
            string sql = @"
DELETE FROM T_TranslationText WHERE   (LanCode = @LanCode);
DELETE FROM T_TranslationText_BAK WHERE   (LanCode = @LanCode);
";

            SqlConnection cn = new SqlConnection(ShareParam.DbConStr);
            SqlCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.CommandType = System.Data.CommandType.Text;
            cm.CommandText = sql;
            cm.Parameters.Add("@LanCode", SqlDbType.NVarChar, 32).Value = _LanCode;

            cn.Open();
            try
            {
                cm.ExecuteNonQuery();
            }
            finally
            {
                cn.Close();
            }
        }


    }





}
