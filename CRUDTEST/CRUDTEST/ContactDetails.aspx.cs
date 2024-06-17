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

            int id = Convert.ToInt32(Request.QueryString["id"]);

            if (!IsPostBack)
            {
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
                                contactImg.Style.Add("max-width", "500px");
                                contactImg.Style.Add("max-height", "300px");
                            }

                            lblContactName.Text = dr["first_name"].ToString() + " " + dr["last_name"].ToString();
                        }

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
            }



        }

        protected void DetailsView1_PageIndexChanging(object sender, DetailsViewPageEventArgs e)
        {

        }

        protected void ContactDetailsDataSource_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {

        }
    }
}