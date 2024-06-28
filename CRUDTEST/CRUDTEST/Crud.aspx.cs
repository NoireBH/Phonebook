﻿using System;
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
                            }
                            else
                            {
                                pfp = CommonConstants.DefaultContactImageUrl;
                            }

                            values.Add(new PhoneContact(int.Parse(dr["id"].ToString()), dr["first_name"].ToString(), dr["last_name"].ToString(),
                                dr["email_address"].ToString(), Convert.ToInt32(dr["age"]), pfp));
                        }

                        ContactsRepeater.DataSource = values.OrderBy(x => x.Id);
                        ContactsRepeater.DataBind();

                    }
                    else
                    {
                        AlertTopFixed.InnerText = "There are currently no contacts,try adding some!";
                        AlertTopFixed.Visible = true;
                    }
                    con.Close();
                }
                catch (Exception)
                {
                    AlertTopFixed.InnerText = "Something went wrong when trying to load your contacts, please try again!";
                    AlertTopFixed.Visible = true;
                }
            }
            else
            {
                AlertTopFixed.Visible = false;
            }
        }

        private void ReBindPhoneNumDataSource()
        {
            PhoneNumRepeater.DataSource = DynamicPhoneNumbers.OrderByDescending(x => x.Id);
            PhoneNumRepeater.DataBind();
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            string firstName = textFirstName.Value.Trim();
            string lastName = textLastName.Value.Trim();
            bool requiredFieldsAreEmpty = String.IsNullOrWhiteSpace(firstName) || String.IsNullOrWhiteSpace(lastName);


            if (!requiredFieldsAreEmpty)
            {
                int fileSize = ImageUpload.PostedFile.ContentLength;
                int maxSizeInBytes = 3 * 1024 * 1024; //3mb

                if (fileSize > maxSizeInBytes)
                {
                    formAlert.InnerText = "The image you're trying to add is too big. Max image size is 3MB.";
                    formAlert.Visible = true;

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "contactModalScript", "showContactModal();", true);
                }

                int age = default;
                bool ageIsInt = true;

                if (!string.IsNullOrWhiteSpace(textAge.Value))
                {
                    ageIsInt = int.TryParse(textAge.Value, out age);
                }

                if (ageIsInt)
                {
                    string emailAddress = textEmailAddress.Value.Trim();
                    byte[] profilePicture = null;
                    bool hasImage = ImageUpload.HasFile;
                    var dbNull = DBNull.Value;

                    if (hasImage)
                    {
                        profilePicture = ImageUpload.FileBytes;

                    }

                    string cmdText = "insert into CONTACTS " +
                        "(first_Name, last_Name, email_address, age, profile_picture)" +
                        " VALUES (:first_name, :last_Name, :email_address, :age, :profile_picture) RETURNING ID INTO :newId";

                    if (BtnHiddenFIeld.Value == "1")
                    {
                        try
                        {
                            using (OracleCommand command = new OracleCommand(cmdText, con))
                            {
                                command.Parameters.AddWithValue("first_name", firstName);
                                command.Parameters.AddWithValue("last_name", lastName);
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

                            cmdText = "insert into PHONENUMBERS " +
                           "(phone_number, contact_id)" +
                           " VALUES (:phone_number, :contact_id)";

                        }
                        catch (Exception)
                        {
                            formAlert.InnerText = "Something went wrong while trying to add the contact, please try again.";
                            formAlert.Visible = true;

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "contactModalScript", "showContactModal();", true);
                        }

                        if (DynamicPhoneNumbers.Count > 0)
                        {
                            AddPhoneNums();
                        }

                        Response.Redirect(String.Format("~/ContactDetails.aspx?id={0}", Convert.ToInt32(HiddenIdField.Value)));
                    }

                    else if (BtnHiddenFIeld.Value == "0")
                    {


                        cmdText = "UPDATE CONTACTS SET FIRST_NAME =:first_name, LAST_NAME =:last_name, EMAIL_ADDRESS =:email_address, AGE =:age, PROFILE_PICTURE=:profile_picture WHERE ID =:id";

                        try
                        {
                            using (OracleCommand command = new OracleCommand(cmdText, con))
                            {
                                command.Parameters.AddWithValue("id", Convert.ToInt32(HiddenIdField.Value));
                                command.Parameters.AddWithValue("first_name", firstName);
                                command.Parameters.AddWithValue("last_name", lastName);
                                command.Parameters.AddWithValue("email_address", emailAddress);
                                command.Parameters.AddWithValue("age", age);

                                if (hasImage)
                                {
                                    command.Parameters.AddWithValue("profile_picture", profilePicture);
                                }
                                else
                                {
                                    bool isDefaultProfilePicture = IsDefaultProfilePicture();

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
                            formAlert.InnerText = "Something went wrong while trying to update the contact info, please try again.";
                            formAlert.Visible = true;

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "contactModalScript", "showContactModal();", true);
                        }

                        if (DynamicPhoneNumbers.Count > 0)
                        {
                            AddPhoneNums();
                        }

                        BtnHiddenFIeld.Value = "1";
                        AddOrUpdatePhoneNumHiddenField.Value = "1";
                        EmptySubmitForm();
                        FormUpdatePanel.Update();
                    }

                    EmptySubmitForm();
                    Response.Redirect(String.Format("~/ContactDetails.aspx?id={0}", Convert.ToInt32(HiddenIdField.Value)));
                }
                else
                {
                    formAlert.InnerText = "Age must be a number!";
                }
            }
            else
            {
                formAlert.InnerText = "Please fill out all required fields!";
            }

            formAlert.Visible = true;
            phoneNumAlert.Visible = false;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "contactModalScript", "showContactModal();", true);
            FormUpdatePanel.Update();
        }

        private void AddPhoneNums()
        {
            string cmdText = "DELETE FROM PHONENUMBERS WHERE CONTACT_ID =:contact_id";
            try
            {
                using (OracleCommand command = new OracleCommand(cmdText, con))
                {
                    command.Parameters.AddWithValue("contact_id", Convert.ToInt32(HiddenIdField.Value));

                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    command.Connection.Close();
                }
            }
            catch (Exception)
            {
                formAlert.InnerText = "Something went wrong, please try again.";
                formAlert.Visible = true;

                ScriptManager.RegisterStartupScript(this, this.GetType(), "contactModalScript", "showContactModal();", true);
            }

            cmdText = "insert into PHONENUMBERS " +
                        "(phone_number, contact_id)" +
                        " VALUES (:phone_number, :contact_id)";

            foreach (var phoneNumber in DynamicPhoneNumbers)
            {
                try
                {
                    using (OracleCommand command = new OracleCommand(cmdText, con))
                    {
                        command.Parameters.AddWithValue("contact_id", Convert.ToInt32(HiddenIdField.Value));
                        command.Parameters.AddWithValue("phone_number", phoneNumber.Number);

                        command.Connection.Open();
                        command.ExecuteNonQuery();
                        command.Connection.Close();
                    }
                }
                catch (Exception)
                {
                    formAlert.InnerText = "Something went wrong, please try again.";
                    formAlert.Visible = true;

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "contactModalScript", "showContactModal();", true);
                }
            }
        }

        private void EmptySubmitForm()
        {
            textFirstName.Value = string.Empty;
            textLastName.Value = string.Empty;
            textEmailAddress.Value = string.Empty;
            textAge.Value = string.Empty;
            textPhoneNumber.Value = string.Empty;
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

                    ContactsRepeater.DataSource = values.OrderBy(x => x.Id);
                    ContactsRepeater.DataBind();
                }
                else
                {
                    AlertTopFixed.InnerText = "There are currently no contacts,try adding some!";
                    AlertTopFixed.Visible = true;
                }
                con.Close();
            }
            catch (Exception)
            {
                AlertTopFixed.InnerText = "Something went wrong when trying to display the contacts, please try again!";
                AlertTopFixed.Visible = true;
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
                AlertTopFixed.InnerText = "Something went wrong when trying to delete the contact, please try again!";
                AlertTopFixed.Visible = true;
            }
        }

        protected void UpdateBtn_Command(object sender, CommandEventArgs e)
        {
            formAlert.Visible = false;

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
                        textFirstName.Value = dr["first_name"].ToString();
                        textLastName.Value = dr["last_name"].ToString();
                        textEmailAddress.Value = dr["email_address"].ToString();

                        if (dr["age"].ToString() == "0")
                        {
                            textAge.Value = string.Empty;
                        }
                        else
                        {
                            textAge.Value = dr["age"].ToString();
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
                    AlertTopFixed.InnerText = "There are currently no contacts,try adding some!";
                    AlertTopFixed.Visible = true;
                }
                con.Close();
            }
            catch (Exception)
            {
                formAlert.InnerText = "The contact doesn't exist!";
                formAlert.Visible = true;
            }

            BtnHiddenFIeld.Value = "0";
            HiddenIdField.Value = e.CommandArgument.ToString();

            List<PhoneNumber> phoneNumbers = new List<PhoneNumber>();

            try
            {
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
            }
            catch (Exception)
            {
                AlertTopFixed.InnerText = "Something went wrong while trying to load the phonenumbers, please try again.";
                AlertTopFixed.Visible = true;
            }

            DynamicPhoneNumbers = phoneNumbers;

            ReBindPhoneNumDataSource();
            con.Close();

            FormUpdatePanel.Update();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "contactModalScript", "showContactModal();", true);
        }

        protected void CancelUpdBtn_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "hideContactModalScript", "hideContactModal();", true);
            phoneNumAlert.Visible = false;
            BtnHiddenFIeld.Value = "1";
            AddOrUpdatePhoneNumHiddenField.Value = "1";
            EmptySubmitForm();
            FormUpdatePanel.Update();
        }

        protected void DetailsBtn_Command(object sender, CommandEventArgs e)
        {
            try
            {
                Response.Redirect(String.Format("~/ContactDetails.aspx?id={0}", e.CommandArgument));
            }
            catch (Exception)
            {
                AlertTopFixed.InnerText = "The contact you're trying to see the details of doesn't exist!";
                AlertTopFixed.Visible = true;
            }
        }

        protected void RemoveNumberBtn_Command(object sender, CommandEventArgs e)
        {
            phoneNumAlert.Visible = false;
            PhoneNumber phoneToRemove = DynamicPhoneNumbers.Where(p => p.Number == e.CommandArgument.ToString()).FirstOrDefault();

            if (phoneToRemove != default)
            {
                DynamicPhoneNumbers.Remove(phoneToRemove);
            }
            else
            {
                phoneNumAlert.InnerText = "That phonenumber doesn't exist!";
                phoneNumAlert.Visible = true;
            }

            ReBindPhoneNumDataSource();

            FormUpdatePanel.Update();
        }

        protected void UpdatePhoneNumBtn_Command(object sender, CommandEventArgs e)
        {
            phoneNumAlert.Visible = false;
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
            string phoneNumber = commandArgs[0].ToString();
            string id = commandArgs[1].ToString();

            AddOrUpdatePhoneNumHiddenField.Value = "0";
            PhoneNumberIdHiddenField.Value = id;
            textPhoneNumber.Value = phoneNumber;
            PhoneNumberHiddenField.Value = phoneNumber;
        }

        protected void AddOrEditPhoneNumBtn_Command(object sender, CommandEventArgs e)
        {
            string phoneNumber = textPhoneNumber.Value;

            if (phoneNumber.Length <= 15)
            {
                if (AddOrUpdatePhoneNumHiddenField.Value == "1")
                {
                    if (!string.IsNullOrWhiteSpace(phoneNumber) && !DynamicPhoneNumbers.Any(p => p.Number == phoneNumber))
                    {
                        phoneNumAlert.Visible = false;
                        DynamicPhoneNumbers.Add(new PhoneNumber(phoneNumber));
                        ReBindPhoneNumDataSource();
                    }
                    else
                    {
                        phoneNumAlert.Visible = true;
                        phoneNumAlert.InnerText = "Can't add an empty or already existing phonenumber!";
                    }

                }
                else if (AddOrUpdatePhoneNumHiddenField.Value == "0")
                {
                    PhoneNumber phoneToUpdate = DynamicPhoneNumbers.Where(p => p.Number == PhoneNumberHiddenField.Value.ToString()).FirstOrDefault();

                    phoneToUpdate.Number = phoneNumber;
                    ReBindPhoneNumDataSource();
                }

                textPhoneNumber.Value = string.Empty;
                AddOrUpdatePhoneNumHiddenField.Value = "1";
            }

            else
            {
                phoneNumAlert.InnerText = "Phonenumber can be no more than 15 characters long!";
                phoneNumAlert.Visible = true;
            }

            FormUpdatePanel.Update();
        }

        protected void AddContactBtn_Click(object sender, EventArgs e)
        {
            try
            {
                EmptySubmitForm();
                formAlert.Visible = false;
                contactImg.ImageUrl = Common.CommonConstants.DefaultContactImageUrl;

                PhoneNumRepeater.DataSource = null;
                PhoneNumRepeater.DataBind();

                FormUpdatePanel.Update();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "contactModalScript", "showContactModal();", true);
            }
            catch (Exception)
            {
                AlertTopFixed.Visible = true;
            }

        }

        protected void SearchContactBtn_Click(object sender, EventArgs e)
        {
            string searchInput = txtsearchcontact.Text;

            var contacts = ContactsRepeater.DataSource as List<PhoneContact>;

            foreach (PhoneContact contact in contacts)
            {

            }



            //try
            //{
            //    OracleCommand cmd = new OracleCommand();
            //    cmd.Parameters.AddWithValue("search_input", searchInput);
            //    cmd.CommandText = "SELECT * FROM CONTACTS WHERE LOWER(FIRST_NAME) LIKE '%' || LOWER(:search_input) || '%' OR LOWER(LAST_NAME) LIKE '%' || LOWER(:search_input) || '%'";
            //    cmd.Connection = con;
            //    con.Open();
            //    OracleDataReader dr = cmd.ExecuteReader();
            //    if (dr.HasRows)
            //    {
            //        List<PhoneContact> values = new List<PhoneContact>();

            //        while (dr.Read())
            //        {
            //            var name = dr["first_name"].ToString();
            //        }

            //    }

            //    con.Close();
            //}
            //catch (Exception)
            //{
            //    throw;
            //}

        }

        private bool IsDefaultProfilePicture() => contactImg.ImageUrl == CommonConstants.DefaultContactImageUrl;       
    }
}