using System;
using System.Data.OracleClient;
using System.Net.Mail;
using System.Web.UI.WebControls;
using CRUDTEST.Common;

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
                    Response.Redirect(String.Format("~/CRUD.aspx"));
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

            txtPhoneNumber.Text = string.Empty;
            LoadContactDetails();
        }

        private void EmptySubmitForm()
        {
            txtPhoneNumber.Text = string.Empty;
        }

        protected void CancelBtn_Click(object sender, EventArgs e)
        {

        }


        protected void DeleteBtn_Click(object sender, EventArgs e)
        {

            try
            {
                OracleCommand cmd = new OracleCommand();

                cmd.Parameters.AddWithValue("id", Convert.ToInt32(Request.QueryString["id"]));
                cmd.CommandText = "DELETE FROM CONTACTS WHERE id=:id";
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();

            }
            catch (Exception)
            {
                throw;
            }

            Response.Redirect(String.Format("~/CRUD.aspx"));

        }

        protected void UpdateBtn_Click(object sender, EventArgs e)
        {
            string fullName = lblContactName.Text;
            string firstName = fullName.Split(' ')[0];
            string lastName = fullName.Split(' ')[1];
            string age = null;
            string emailAddress = string.Empty;

            if (lblContactAge.Text != "Not specified")
            {
                age = lblContactAge.Text;
            }

            if (lblEmailAddress.Text != "Not specified")
            {
                emailAddress = lblEmailAddress.Text;
            }

            txtFirstName.Text = firstName;
            txtLastName.Text = lastName;
            txtAge.Text = age;
            txtFormEmailAddress.Text = emailAddress;

            ContactInfoUpdatePanel.Update();

        }

        protected void SubmitContactInfo_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["id"]);
            string firstName = txtFirstName.Text;
            string lastName = txtLastName.Text;
            int age = default;
            string emailAddress = null;
            byte[] profilePicture = null;
            bool isDefaultProfilePicture = IsDefaultProfilePicture();
            bool hasImageUploadedInForm = ImageUpload.HasFile;
            var dbNull = DBNull.Value;

            if (!string.IsNullOrWhiteSpace(txtAge.Text))
            {
                age = Convert.ToInt32(txtAge.Text.Trim());
            }

            if (!string.IsNullOrWhiteSpace(txtFormEmailAddress.Text))
            {
                emailAddress = txtFormEmailAddress.Text;
            }

            if (contactImg.ImageUrl != CommonConstants.DefaultContactImageUrl)
            {
                profilePicture = ImageUpload.FileBytes;

            }

            string cmdText = "UPDATE CONTACTS SET FIRST_NAME =:first_name, LAST_NAME =:last_name, AGE =:age, " +
                "EMAIL_ADDRESS =:email_address, PROFILE_PICTURE =:profile_picture  WHERE ID =:id";

            try
            {
                using (OracleCommand command = new OracleCommand(cmdText, con))
                {
                    command.Parameters.AddWithValue("id", id);
                    command.Parameters.AddWithValue("first_name", firstName);
                    command.Parameters.AddWithValue("last_name", lastName);
                    command.Parameters.AddWithValue("age", age);
                    command.Parameters.AddWithValue("email_address", emailAddress);

                    if (hasImageUploadedInForm)
                    {
                        command.Parameters.AddWithValue("profile_picture", profilePicture);
                    }
                    else
                    {
                        if (!isDefaultProfilePicture)
                        {
                            cmdText = "UPDATE CONTACTS SET FIRST_NAME =:first_name, LAST_NAME =:last_name, AGE =:age, " +
                                "EMAIL_ADDRESS =:email_address WHERE ID =:id";
                        }
                        else
                        {
                            command.Parameters.AddWithValue("profile_picture", dbNull);
                        }


                    }

                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    command.Connection.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }

            EmptySubmitForm();
        }

        private bool IsDefaultProfilePicture()
        {
            if (contactImg.ImageUrl == CommonConstants.DefaultContactImageUrl)
            {
                return true;
            }

            return false;
        }
    }
}