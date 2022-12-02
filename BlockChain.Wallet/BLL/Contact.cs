using System.Data;
using System.Data.SqlClient;


namespace BlockChain.Wallet.BLL
{
    public  class Contact
    {

        public static void SaveNewContact(string name, string address, string remark)
        {
            Model.Contact c = new Model.Contact();
            c.ContactName = name;
            c.ContactAddress = address;
            c.ContactRemark = remark;
            c.CreateTime = System.DateTime.Now;

            DAL.Contact.Insert(Share. ShareParam.DbConStr, c);        
        }


        public static DataTable GetAllContact()
        {
            string sql = @"
SELECT   ContactName, ContactAddress, ContactRemark, CreateTime
FROM      Contact
";

            SqlConnection cn = new SqlConnection(Share. ShareParam.DbConStr);
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

    }
}
