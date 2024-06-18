using CRUDTEST.Models;
using CRUDTEST.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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

        protected void DetailsView1_PageIndexChanging(object sender, DetailsViewPageEventArgs e)
        {

        }

        protected void ContactDetailsDataSource_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {

        }

        protected void backToContactsBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect(String.Format("~/CRUD.aspx"));
        }

        protected void RemoveNumberBtn_Command(object sender, CommandEventArgs e)
        {

            try
            {
                OracleCommand cmd = new OracleCommand();

                cmd.Parameters.AddWithValue("id", Convert.ToInt32(e.CommandArgument));
                cmd.CommandText = "DELETE FROM PHONENUMBERS WHERE id=:id";
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();

            }
            catch (Exception)
            {

                throw;
            }

            LoadContactDetails();

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

                    //List<PhoneNumber> phoneNumbers = new List<PhoneNumber>();

                    while (dr.Read())
                    {

                        //var phoneContact = new PhoneContactDetailsViewModel(int.Parse(dr["id"].ToString()), dr["first_name"].ToString(), dr["last_name"].ToString(), dr["main_phone_number"].ToString());
                        if (dr["profile_picture"] != DBNull.Value)
                        {
                            byte[] pfpBytes = (byte[])(dr["profile_picture"]);
                            string pfp = Convert.ToBase64String(pfpBytes);
                            contactImg.ImageUrl = String.Format("data:image/jpg;base64,{0}", pfp);
                        }
                        else
                        {
                            contactImg.ImageUrl = "Images/blank-pfp.png";
                        }

                        contactImg.Style.Add("max-width", "450px");
                        contactImg.Style.Add("max-height", "300px");
                        lblContactName.Text = dr["first_name"].ToString() + " " + dr["last_name"].ToString();
                        lblContactAge.Text = "Age:" + " " + dr["age"].ToString();
                        lblEmailAddress.Text = "Email:" + " " + dr["email_address"].ToString();

                    }

                    con.Close();

                    //OracleCommand cmdGetPhoneNums = new OracleCommand();
                    //cmd.Parameters.AddWithValue("id", id);
                    //cmd.CommandText = "select id,  phone_number from PHONENUMBERS WHERE CONTACT_ID =:id ";
                    //cmd.Connection = con;
                    //con.Open();
                    //OracleDataReader dr2 = cmd.ExecuteReader();

                    //while (dr2.Read())
                    //{
                    //    phoneNumbers.Add(new PhoneNumber(Convert.ToInt32(dr2["id"].ToString()), dr2["phone_number"].ToString()));
                    //}

                    //PhoneRepeater.DataSource = phoneNumbers.OrderBy(x => x.Id);
                    //PhoneRepeater.DataBind();

                }
                else
                {
                    Response.Write("This Contact does not exist!");
                }
                con.Close();




            }
            catch (Exception)
            {
                throw;
            }

            ContactDetailsDataSource.DataBind();
            PhoneRepeater.DataBind();

        }

        protected void submitBtn_Click(object sender, EventArgs e)
        {
            string phoneNumber = txtPhoneNumber.Text;

            string cmdText = "insert into PHONENUMBERS " +
                "(phone_number, contact_id)" +
                " VALUES (:phone_number, :contact_id)";

            try
            {
                int contact_id = Convert.ToInt32(Request.QueryString["id"]);

                OracleCommand cmd = new OracleCommand();

                cmd.Parameters.AddWithValue("contact_id", contact_id);
                cmd.Parameters.AddWithValue("phone_number", phoneNumber);
                cmd.CommandText = cmdText;
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();

            }
            catch (Exception)
            {

                throw;
            }

            LoadContactDetails();
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {

        }
    }
}