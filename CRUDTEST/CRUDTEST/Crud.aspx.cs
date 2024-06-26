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
using CRUDTEST.Common;
using Label = System.Web.UI.WebControls.Label;
using TextBox = System.Web.UI.WebControls.TextBox;
using System.Web.UI;
using Button = System.Web.UI.WebControls.Button;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using Microsoft.Ajax.Utilities;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CRUDTEST
{

    public partial class Crud : System.Web.UI.Page
    {
        OracleConnection con = new OracleConnection(@"Data Source=oratest19/odbms;USER ID=EMustafov; password=manager;");

        private List<PhoneNumber> DynamicPhoneNumbers
        {
            get
            {
                if (ViewState["PhoneNumbers"] == null)
                {
                    ViewState["PhoneNumbers"] = new List<PhoneNumber>();
                }
                return (List<PhoneNumber>)ViewState["PhoneNumbers"];
            }
            set
            {
                ViewState["PhoneNumbers"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

                contactImg.Style.Add("max-width", "400px");
                contactImg.Style.Add("max-height", "300px");

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
                            string pfp = null;

                            if (dr["profile_picture"] != DBNull.Value)
                            {
                                byte[] pfpBytes = (byte[])(dr["profile_picture"]);
                                pfp = String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(pfpBytes));
                                //contactImg.ImageUrl = String.Format("data:image/jpg;base64,{0}", pfp);
                            }
                            else
                            {
                                pfp = CommonConstants.DefaultContactImageUrl;
                            }

                            values.Add(new PhoneContact(int.Parse(dr["id"].ToString()), dr["first_name"].ToString(), dr["last_name"].ToString(),
                                dr["email_address"].ToString(), Convert.ToInt32(dr["age"]), pfp));
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

            else
            {
                //RecreateTextboxes();
            }
        }

        //private void RecreateTextboxes()
        //{
        //    PhoneNumRepeater.Controls.Clear();
        //    foreach (var phoneNumber in DynamicPhoneNumbers)
        //    {

        //        TextBox dynamicTextBox = new TextBox();
        //        dynamicTextBox.ID = "ControlID_" + phoneNumber.Id;
        //        dynamicTextBox.Text = phoneNumber.Number;
        //        dynamicTextBox.Attributes["class"] = "text-center";
        //        PhoneNumRepeater.Controls.Add(dynamicTextBox);               
        //    }

        //    PhoneNumRepeater.DataBind();
        //}

        protected void TextBox4_TextChanged(object sender, EventArgs e)
        {

        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            string firstName = txtFirstName.Text.Trim();
            string lastName = txtLastName.Text.Trim();
            //string phoneNumber = txtPhoneNumber.Text.Trim();
            string emailAddress = txtEmailAddress.Text.Trim();
            int age = default;
            byte[] profilePicture = null;
            bool isDefaultProfilePicture = IsDefaultProfilePicture();
            bool hasImage = ImageUpload.HasFile;
            var dbNull = DBNull.Value;

            if (hasImage)
            {
                profilePicture = ImageUpload.FileBytes;
                //string pfp = String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(profilePicture));
                //contactImg.ImageUrl = String.Format("data:image/jpg;base64,{0}", pfp);
                //RepeaterUpdatePanel.Update();

            }

            if (!string.IsNullOrWhiteSpace(txtAge.Text))
            {
                age = Convert.ToInt32(txtAge.Text.Trim());
            }


            string cmdText = "insert into CONTACTS " +
                "(first_Name, last_Name, email_address, age, profile_picture)" +
                " VALUES (:first_name, :last_Name, :email_address, :age, :profile_picture) RETURNING ID INTO :newId";

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
                        //command.Parameters.AddWithValue("phone_number", phoneNumber);
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

                        OracleParameter newIdParam = new OracleParameter("newId", OracleType.Int32);
                        newIdParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(newIdParam);


                        command.Connection.Open();
                        command.ExecuteNonQuery();

                        HiddenIdField.Value = newIdParam.Value.ToString();

                        command.Connection.Close();
                    }

                    List<string> txtBoxPhoneNums = new List<string>();

                    foreach (var item in PhoneNumRepeater.Controls)
                    {

                        TextBox textbox = (TextBox)item;
                        string phoneNumber = textbox.Text;

                        txtBoxPhoneNums.Add(phoneNumber);
                    }

                    cmdText = "insert into PHONENUMBERS " +
                   "(phone_number, contact_id)" +
                   " VALUES (:phone_number, :contact_id)";


                    try
                    {
                        foreach (var phoneNum in txtBoxPhoneNums)
                        {
                            using (OracleCommand command = new OracleCommand(cmdText, con))
                            {
                                command.Parameters.AddWithValue("phone_number", phoneNum);
                                command.Parameters.AddWithValue("contact_id", Convert.ToInt32(HiddenIdField.Value));

                                command.Connection.Open();
                                command.ExecuteNonQuery();
                                command.Connection.Close();
                            }
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }




                }
                catch (Exception)
                {

                    throw;
                }

                Response.Redirect(String.Format("~/ContactDetails.aspx?id={0}", Convert.ToInt32(HiddenIdField.Value)));

            }

            else if (BtnHiddenFIeld.Value == "0")
            {

                if (ImageUpload.HasFile)
                {
                    string fileExtension = Path.GetExtension(ImageUpload.FileName).ToLower();

                    if (fileExtension != ".jpg" || fileExtension == ".gif" || fileExtension != ".jpeg" || fileExtension != ".png")
                    {
                        hasImage = false;
                    }

                }

                cmdText = "UPDATE CONTACTS SET FIRST_NAME =:first_name, LAST_NAME =:last_name, EMAIL_ADDRESS =:email_address, AGE =:age, PROFILE_PICTURE=:profile_picture WHERE ID =:id";
                int id = Convert.ToInt32(HiddenIdField.Value);

                ChangeSubmitBtnTxtAndColor();

                try
                {
                    using (OracleCommand command = new OracleCommand(cmdText, con))
                    {
                        command.Parameters.AddWithValue("id", id);
                        command.Parameters.AddWithValue("first_name", firstName);
                        command.Parameters.AddWithValue("last_name", lastName);
                        //command.Parameters.AddWithValue("phone_number", phoneNumber);
                        command.Parameters.AddWithValue("email_address", emailAddress);
                        command.Parameters.AddWithValue("age", age);

                        if (hasImage)
                        {
                            command.Parameters.AddWithValue("profile_picture", profilePicture);
                        }
                        else
                        {
                            if (!isDefaultProfilePicture)
                            {
                                cmdText = "UPDATE CONTACTS SET FIRST_NAME =:first_name, LAST_NAME =:last_name, AGE =:age, " +
                                    "EMAIL_ADDRESS =:email_address WHERE ID =:id";
                                command.CommandText = cmdText;
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

                List<string> txtBoxPhoneNums = new List<string>();

                foreach (var item in PhoneNumRepeater.Controls)
                {

                    TextBox textbox = (TextBox)item;
                    string phoneNumber = textbox.Text;

                    txtBoxPhoneNums.Add(phoneNumber);
                }

                //string getLastInsertedPhoneNumId = "SELECT SCOPE_IDENTITY();";
                //int lastInsertedID = -1;

                //using (OracleCommand command = new OracleCommand(getLastInsertedPhoneNumId, con))
                //{
                //    lastInsertedID = Convert.ToInt32(command.ExecuteScalar());
                //}

                cmdText = "insert into PHONENUMBERS " +
               "(phone_number, contact_id)" +
               " VALUES (:phone_number, :contact_id)";


                try
                {
                    foreach (var phoneNum in txtBoxPhoneNums)
                    {
                        using (OracleCommand command = new OracleCommand(cmdText, con))
                        {
                            command.Parameters.AddWithValue("phone_number", phoneNum);
                            command.Parameters.AddWithValue("contact_id", id);

                            command.Connection.Open();
                            command.ExecuteNonQuery();
                            command.Connection.Close();
                        }
                    }
                }
                catch (Exception)
                {

                    throw;
                }

                BtnHiddenFIeld.Value = "1";
                AddOrUpdatePhoneNumHiddenField.Value = "1";
                EmptySubmitForm();
                FormUpdatePanel.Update();
                Response.Redirect(String.Format("~/ContactDetails.aspx?id={0}", id));
            }


            EmptySubmitForm();
            FormUpdatePanel.Update();

            ShowBtn_Click(sender, e);
        }

        private void EmptySubmitForm()
        {
            txtFirstName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtEmailAddress.Text = string.Empty;
            txtAge.Text = string.Empty;
            txtPhoneNumber.Text = string.Empty;
        }

        protected void ShowBtn_Click(object sender, EventArgs e)
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
                        values.Add(new PhoneContact(int.Parse(dr["id"].ToString()), dr["first_name"].ToString(),
                            dr["last_name"].ToString(), dr["email_address"].ToString(), Convert.ToInt32(dr["age"])));
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

            try
            {
                OracleCommand cmd = new OracleCommand();
                cmd.Parameters.AddWithValue("id", Convert.ToInt32(e.CommandArgument));
                cmd.CommandText = "select * from CONTACTS WHERE ID =:id";
                cmd.Connection = con;
                con.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        txtFirstName.Text = dr["first_name"].ToString();
                        txtLastName.Text = dr["last_name"].ToString();
                        txtEmailAddress.Text = dr["email_address"].ToString();

                        if (dr["age"].ToString() == "0")
                        {
                            txtAge.Text = string.Empty;
                        }
                        else
                        {
                            txtAge.Text = dr["age"].ToString();
                        }

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

            BtnHiddenFIeld.Value = "0";
            HiddenIdField.Value = e.CommandArgument.ToString();

            List<PhoneNumber> phoneNumbers = new List<PhoneNumber>();

            OracleCommand cmdGetPhoneNums = new OracleCommand();
            cmdGetPhoneNums.Parameters.AddWithValue("id", HiddenIdField.Value);
            cmdGetPhoneNums.CommandText = "select id,  phone_number from PHONENUMBERS WHERE CONTACT_ID =:id ";
            cmdGetPhoneNums.Connection = con;
            con.Open();
            OracleDataReader dr2 = cmdGetPhoneNums.ExecuteReader();

            while (dr2.Read())
            {
                phoneNumbers.Add(new PhoneNumber(Convert.ToInt32(dr2["id"].ToString()), dr2["phone_number"].ToString()));
            }

            DynamicPhoneNumbers = phoneNumbers;

            PhoneNumRepeater.DataSource = DynamicPhoneNumbers;
            PhoneNumRepeater.DataBind();
            con.Close();


            FormUpdatePanel.Update();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "contactModalScript", "showContactModal();", true);

        }

        protected void BtnHiddenFIeld_ValueChanged(object sender, EventArgs e)
        {

        }

        protected void HiddenIdField_ValueChanged(object sender, EventArgs e)
        {

        }

        protected void CancelUpdBtn_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "hideContactModalScript", "hideContactModal();", true);
            BtnHiddenFIeld.Value = "1";
            EmptySubmitForm();
            FormUpdatePanel.Update();
        }

        private void ChangeSubmitBtnTxtAndColor()
        {
            submitBtn.Text = "Submit";
            submitBtn.BackColor = ColorTranslator.FromHtml("#198754");
            submitBtn.ForeColor = Color.White;
        }


        protected void DetailsBtn_Command(object sender, CommandEventArgs e)
        {
            Response.Redirect(String.Format("~/ContactDetails.aspx?id={0}", e.CommandArgument));
        }


        //protected void SearchContactBtn_Click(object sender, EventArgs e)
        //{
        //    string searchInput = txtSearchContact.Text;

        //    try
        //    {
        //        OracleCommand cmd = new OracleCommand();
        //        cmd.Parameters.AddWithValue("search_input", searchInput);
        //        cmd.CommandText = "SELECT * FROM CONTACTS WHERE LOWER(FIRST_NAME) LIKE '%' || LOWER(:search_input) || '%' OR LOWER(LAST_NAME) LIKE '%' || LOWER(:search_input) || '%'";
        //        cmd.Connection = con;
        //        con.Open();
        //        OracleDataReader dr = cmd.ExecuteReader();
        //        if (dr.HasRows)
        //        {
        //            List<PhoneContact> values = new List<PhoneContact>();

        //            while (dr.Read())
        //            {
        //                var name = dr["first_name"].ToString();
        //            }

        //        }

        //        con.Close();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }



        //}

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

            PhoneNumRepeater.DataSource = DynamicPhoneNumbers;
            PhoneNumRepeater.DataBind();
            con.Close();

            FormUpdatePanel.Update();

        }

        protected void UpdatePhoneNumBtn_Command(object sender, CommandEventArgs e)
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
            string phoneNumber = commandArgs[0].ToString();
            string id = commandArgs[1].ToString();

            AddOrUpdatePhoneNumHiddenField.Value = "0";
            PhoneNumberIdHiddenField.Value = id;
            txtPhoneNumber.Text = phoneNumber;

        }

        protected void AddOrEditPhoneNumBtn_Command(object sender, CommandEventArgs e)
        {
            string phoneNumber = txtPhoneNumber.Text;
            string cmdText = "insert into PHONENUMBERS " +
                    "(phone_number, contact_id)" +
                    " VALUES (:phone_number, :contact_id)";

            if (AddOrUpdatePhoneNumHiddenField.Value == "1")
            {
                //try
                //{
                //    int contact_id = Convert.ToInt32(HiddenIdField.Value);

                //    OracleCommand cmd = new OracleCommand();

                //    cmd.Parameters.AddWithValue("contact_id", contact_id);
                //    cmd.Parameters.AddWithValue("phone_number", phoneNumber);
                //    cmd.CommandText = cmdText;
                //    cmd.Connection = con;
                //    con.Open();
                //    cmd.ExecuteNonQuery();
                //    cmd.Connection.Close();

                //}
                //catch (Exception)
                //{
                //    throw;
                //}

                if (!string.IsNullOrWhiteSpace(phoneNumber))
                {
                    DynamicPhoneNumbers.Add(new PhoneNumber(phoneNumber));
                    //RecreateTextboxes();

                }


            }
            else if (AddOrUpdatePhoneNumHiddenField.Value == "0")
            {


                //cmdText = "UPDATE PHONENUMBERS SET PHONE_NUMBER =:phone_number WHERE ID =:id";

                //try
                //{
                //    int id = Convert.ToInt32(PhoneNumberIdHiddenField.Value);

                //    OracleCommand cmd = new OracleCommand();

                //    cmd.Parameters.AddWithValue("id", id);
                //    cmd.Parameters.AddWithValue("phone_number", phoneNumber);
                //    cmd.CommandText = cmdText;
                //    cmd.Connection = con;
                //    con.Open();
                //    cmd.ExecuteNonQuery();
                //    cmd.Connection.Close();

                //}
                //catch (Exception)
                //{
                //    throw;
                //}

                //RepeaterItem item = (RepeaterItem)((Button)sender).NamingContainer;
                //TextBox txtAddOrEditphoneNum = (TextBox)item.FindControl("txtAddOrEditphoneNum");
                //string phoneNumberFromTextBox = txtAddOrEditphoneNum.Text;


            }
            else
            {
                //BulletedList1.Items.add
            }

            //List<PhoneNumber> phoneNumbers = new List<PhoneNumber>();

            //OracleCommand cmdGetPhoneNums = new OracleCommand();
            //cmdGetPhoneNums.Parameters.AddWithValue("id", HiddenIdField.Value);
            //cmdGetPhoneNums.CommandText = "select id,  phone_number from PHONENUMBERS WHERE CONTACT_ID =:id ";
            //cmdGetPhoneNums.Connection = con;
            //con.Open();
            //OracleDataReader dr2 = cmdGetPhoneNums.ExecuteReader();

            //while (dr2.Read())
            //{
            //    phoneNumbers.Add(new PhoneNumber(Convert.ToInt32(dr2["id"].ToString()), dr2["phone_number"].ToString()));
            //}

            //PhoneNumRepeater.DataSourceID = "PhoneNumbers";
            //PhoneNumRepeater.DataBind();
            //con.Close();

            txtPhoneNumber.Text = string.Empty;
            AddOrUpdatePhoneNumHiddenField.Value = "1";

            FormUpdatePanel.Update();

        }

        protected void AddContactBtn_Click(object sender, EventArgs e)
        {
            EmptySubmitForm();
            contactImg.ImageUrl = Common.CommonConstants.DefaultContactImageUrl;

            PhoneNumRepeater.DataSource = null;
            PhoneNumRepeater.DataBind();

            FormUpdatePanel.Update();
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