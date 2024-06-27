using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Linq;
using System.Net.Mail;
using System.Web.UI.WebControls;
using CRUDTEST.Common;
using CRUDTEST.Models;

namespace CRUDTEST
{
    public partial class ContactDetails : System.Web.UI.Page
    {
        OracleConnection con = new OracleConnection(@"Data Source=oratest19/odbms;USER ID=EMustafov; password=manager;");

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadContactDetails();
            }
        }      

        protected void backToContactsBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect(String.Format("~/CRUD.aspx"));
        }

        private void LoadContactDetails()
        {
            int id = Convert.ToInt32(Request.QueryString["id"]);

            try
            {
                OracleCommand cmd = new OracleCommand();
                cmd.Parameters.AddWithValue("id", id);
                cmd.CommandText = "select * from CONTACTS WHERE ID =:id ";
                cmd.Connection = con;
                con.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    List<PhoneNumber> phoneNumbers = new List<PhoneNumber>();

                    while (dr.Read())
                    {                       
                        if (dr["profile_picture"] != DBNull.Value)
                        {
                            byte[] pfpBytes = (byte[])(dr["profile_picture"]);
                            string pfp = Convert.ToBase64String(pfpBytes);
                            contactImg.ImageUrl = String.Format("data:image/jpg;base64,{0}", pfp);
                        }
                        else
                        {
                            contactImg.ImageUrl = CommonConstants.DefaultContactImageUrl;
                        }

                        contactImg.Style.Add("max-width", "450px");
                        contactImg.Style.Add("max-height", "300px");
                        lblContactName.Text = dr["first_name"].ToString() + " " + dr["last_name"].ToString();

                        if (Convert.ToInt32(dr["age"]) == 0)
                        {
                            lblContactAge.Text = "Not specified";
                        }
                        else
                        {
                            lblContactAge.Text = dr["age"].ToString();
                        }

                        if (string.IsNullOrWhiteSpace(dr["email_address"].ToString()))
                        {
                            lblEmailAddress.Text = "Not specified";
                        }
                        else
                        {
                            lblEmailAddress.Text = dr["email_address"].ToString();
                        }
                    }

                    con.Close();

                    OracleCommand cmdGetPhoneNums = new OracleCommand();
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.CommandText = "select id,  phone_number from PHONENUMBERS WHERE CONTACT_ID =:id ";
                    cmd.Connection = con;
                    con.Open();
                    OracleDataReader dr2 = cmd.ExecuteReader();

                    while (dr2.Read())
                    {
                        phoneNumbers.Add(new PhoneNumber(Convert.ToInt32(dr2["id"].ToString()), dr2["phone_number"].ToString()));
                    }

                    PhoneRepeater.DataSource = phoneNumbers.OrderBy(x => x.Id);
                    PhoneRepeater.DataBind();

                }
                else
                {
                    Response.Redirect(String.Format("~/CRUD.aspx"));
                }
                con.Close();

            }
            catch (Exception)
            {
                throw;
            }

            ContactDetailsDataSource.DataBind();
        }       
    }
}