using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data.OracleClient;
using System.Data;
using CRUDTEST.Models;
using System.Drawing;
using System.Xml.Linq;
using CRUDTEST.ViewModels;
using System.Windows.Forms;
using System.IO;

namespace CRUDTEST
{
    public partial class Crud : System.Web.UI.Page
    {
        OracleConnection con = new OracleConnection(@"Data Source=oratest19/odbms;USER ID=EMustafov; password=manager;");

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

                try
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.CommandText = "select * from CONTACTS";
                    cmd.Connection = con;
                    con.Open();
                    OracleDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        List<PhoneContact> values = new List<PhoneContact>();

                        while (dr.Read())
                        {
                            values.Add(new PhoneContact(int.Parse(dr["id"].ToString()), dr["first_name"].ToString(), dr["last_name"].ToString(),
                                dr["main_phone_number"].ToString(), dr["email_address"].ToString(), Convert.ToInt32(dr["age"])));
                        }

                        Repeater.DataSource = values.OrderBy(x => x.Id);
                        Repeater.DataBind();


                    }
                    else
                    {
                        Response.Write("No Contacts In DataBase");
                    }
                    con.Close();
                }
                catch (Exception)
                {
                    throw;
                }
            }




        }

        protected void TextBox4_TextChanged(object sender, EventArgs e)
        {

        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            string firstName = txtFirstName.Text.Trim();
            string lastName = txtLastName.Text.Trim();
            string phoneNumber = txtPhoneNumber.Text.Trim();
            string emailAddress = txtEmailAddress.Text.Trim();
            int age = default;
            byte[] profilePicture = null;
            bool hasImage = ImageUpload.HasFile;
            var dbNull = DBNull.Value;

            if (hasImage)
            {
                profilePicture = ImageUpload.FileBytes;


            }

            if (!string.IsNullOrWhiteSpace(txtAge.Text))
            {
                age = Convert.ToInt32(txtAge.Text.Trim());
            }


            string cmdText = "insert into CONTACTS " +
                "(first_Name, last_Name, main_phone_number, email_address, age, profile_picture)" +
                " VALUES (:first_name, :last_Name, :phone_number, :email_address, :age, :profile_picture)";

            //if (!IsValid)
            //{

            //}

            if (BtnHiddenFIeld.Value == "1")
            {


                try
                {
                    using (OracleCommand command = new OracleCommand(cmdText, con))
                    {
                        command.Parameters.AddWithValue("first_name", firstName);
                        command.Parameters.AddWithValue("last_name", lastName);
                        command.Parameters.AddWithValue("phone_number", phoneNumber);
                        command.Parameters.AddWithValue("email_address", emailAddress);
                        command.Parameters.AddWithValue("age", age);

                        if (hasImage)
                        {
                            command.Parameters.AddWithValue("profile_picture", profilePicture);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("profile_picture", dbNull);
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
                FormUpdatePanel.Update();

            }

            else if (BtnHiddenFIeld.Value == "0")
            {

                if (ImageUpload.HasFile)
                {
                    string fileExtension = Path.GetExtension(ImageUpload.FileName).ToLower();

                    if (fileExtension == "jpg" || fileExtension == "gif" || fileExtension == "jpeg" || fileExtension == "png")
                    {
                        profilePicture = ImageUpload.FileBytes;
                    }

                }

                cmdText = "UPDATE CONTACTS SET FIRST_NAME =:first_name, LAST_NAME =:last_name, MAIN_PHONE_NUMBER =:phone_number, EMAIL_ADDRESS =:email_address, AGE =:age, PROFILE_PICTURE=:profile_picture WHERE ID =:id";
                int id = Convert.ToInt32(HiddenIdField.Value);

                ChangeSubmitBtnTxtAndColor();

                try
                {
                    using (OracleCommand command = new OracleCommand(cmdText, con))
                    {
                        command.Parameters.AddWithValue("id", id);
                        command.Parameters.AddWithValue("first_name", firstName);
                        command.Parameters.AddWithValue("last_name", lastName);
                        command.Parameters.AddWithValue("phone_number", phoneNumber);
                        command.Parameters.AddWithValue("email_address", emailAddress);
                        command.Parameters.AddWithValue("age", age);

                        if (hasImage)
                        {
                            command.Parameters.AddWithValue("profile_picture", profilePicture);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("profile_picture", dbNull);
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

                BtnHiddenFIeld.Value = "1";
                EmptySubmitForm();
            }


            ShowBtn_Click(sender, e);
        }

        private void EmptySubmitForm()
        {
            txtFirstName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtPhoneNumber.Text = string.Empty;
            txtEmailAddress.Text = string.Empty;
            txtAge.Text = string.Empty;
        }

        protected void ShowBtn_Click(object sender, EventArgs e)
        {

            //ShowContactsBtn.Attributes.Clear();
            //ShowContactsBtn.Attributes.Add("style", "display:none");

            try
            {
                OracleCommand cmd = new OracleCommand();
                cmd.CommandText = "select * from CONTACTS";
                cmd.Connection = con;
                con.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    List<PhoneContact> values = new List<PhoneContact>();

                    while (dr.Read())
                    {
                        values.Add(new PhoneContact(int.Parse(dr["id"].ToString()), dr["first_name"].ToString(),
                            dr["last_name"].ToString(), dr["main_phone_number"].ToString(), dr["email_address"].ToString(),
                            Convert.ToInt32(dr["age"])));
                    }

                    Repeater.DataSource = values.OrderBy(x => x.Id);
                    Repeater.DataBind();


                }
                else
                {
                    Response.Write("No Contacts In DataBase");
                }
                con.Close();
            }
            catch (Exception)
            {
                throw;
            }

            FormUpdatePanel.Update();

        }

        protected void Repeater_ItemCommand1(object source, RepeaterCommandEventArgs e)
        {

        }

        protected void Repeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }

        protected void DeleteBtn_Command(object sender, CommandEventArgs e)
        {

            try
            {
                OracleCommand cmd = new OracleCommand();

                cmd.Parameters.AddWithValue("id", Convert.ToInt32(e.CommandArgument));
                cmd.CommandText = "DELETE FROM CONTACTS WHERE id=:id";
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();

                ShowBtn_Click(sender, e);

            }
            catch (Exception)
            {
                throw;
            }


        }

        protected void UpdateBtn_Command(object sender, CommandEventArgs e)
        {
            submitBtn.Text = "Update";
            submitBtn.BackColor = ColorTranslator.FromHtml("#ffc107");
            submitBtn.ForeColor = Color.White;

            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });

            txtFirstName.Text = commandArgs[1];
            txtLastName.Text = commandArgs[2];
            txtPhoneNumber.Text = commandArgs[3];
            txtEmailAddress.Text = commandArgs[4];
            txtAge.Text = commandArgs[5];

            BtnHiddenFIeld.Value = "0";
            HiddenIdField.Value = commandArgs[0].ToString();

            //CancelUpdBtn.Attributes.Clear();
            //CancelUpdBtn.Attributes.Add("style", "display:block");
            //CancelUpdBtn.Attributes.Add("style", "color:white");
            //CancelUpdBtn.Attributes.Add("style", "font-weight:bold");
            //txtCreateContract.InnerText = "Edit a Contact";
            //submitBtn.Click -= Submit_Click;
            //submitBtn.Click += Update_Click;
            FormUpdatePanel.Update();


        }

        protected void BtnHiddenFIeld_ValueChanged(object sender, EventArgs e)
        {

        }

        protected void HiddenIdField_ValueChanged(object sender, EventArgs e)
        {

        }

        protected void CancelUpdBtn_Click(object sender, EventArgs e)
        {
            BtnHiddenFIeld.Value = "1";
            ChangeSubmitBtnTxtAndColor();
            //CancelUpdBtn.Attributes.Clear();
            //CancelUpdBtn.Attributes.Add("style", "display:none");
            EmptySubmitForm();
        }

        private void ChangeSubmitBtnTxtAndColor()
        {
            submitBtn.Text = "Submit";
            submitBtn.BackColor = ColorTranslator.FromHtml("#198754");
            submitBtn.ForeColor = Color.White;
        }

        protected void UploadImgBtn_Click(object sender, EventArgs e)
        {
            //FileStream fls;
            //fls = new FileStream(imgPath, FileMode.Open, FileAccess.Read);
            ////a byte array to read the image 
            //byte[] blob = new byte[fls.Length];
            //fls.Read(blob, 0, System.Convert.ToInt32(fls.Length));
            //fls.Close();

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Please select an image";
            ofd.Filter = "JPG|*.jpg|PNG|*.png|GIF|.gif";
            ofd.Multiselect = false;

            if (ofd.ShowDialog() == DialogResult.OK)
            {

            }


        }

        protected void DetailsBtn_Command(object sender, CommandEventArgs e)
        {
            Response.Redirect(String.Format("~/ContactDetails.aspx?id={0}", e.CommandArgument));
        }

        protected void HiddenEmailAddressField_ValueChanged(object sender, EventArgs e)
        {

        }

        protected void HiddenAgeField_ValueChanged(object sender, EventArgs e)
        {

        }

        protected void HiddenPictureField_ValueChanged(object sender, EventArgs e)
        {

        }

        protected void UpdateBtn_Command1(object sender, CommandEventArgs e)
        {

        }

        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        protected void SearchContactBtn_Command(object sender, CommandEventArgs e)
        {

        }

        protected void SearchContactBtn_Click(object sender, EventArgs e)
        {
            string searchInput = txtSearchContact.Text;

            try
            {
                OracleCommand cmd = new OracleCommand();
                cmd.Parameters.AddWithValue("search_input", searchInput);
                cmd.CommandText = "SELECT * FROM CONTACTS WHERE LOWER(FIRST_NAME) LIKE '%' || LOWER(:search_input) || '%' OR LOWER(LAST_NAME) LIKE '%' || LOWER(:search_input) || '%' OR LOWER(MAIN_PHONE_NUMBER) LIKE '%' || LOWER(:search_input) || '%'";
                cmd.Connection = con;
                con.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    List<PhoneContact> values = new List<PhoneContact>();

                    while (dr.Read())
                    {
                        var name = dr["first_name"].ToString();
                    }

                }
                else
                {
                    Response.Write("No Contacts In DataBase");
                }

                con.Close();
            }
            catch (Exception)
            {
                throw;
            }



        }
    }
}