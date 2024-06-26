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

        private List<PhoneNumber> NewPhoneNumbers
        {
            get
            {
                if (ViewState["NewPhoneNumbers"] == null)
                {
                    ViewState["NewPhoneNumbers"] = new List<PhoneNumber>();
                }
                return (List<PhoneNumber>)ViewState["NewPhoneNumbers"];
            }
            set
            {
                ViewState["NewPhoneNumbers"] = value;
            }
        }

        private List<PhoneNumber> PhoneNumbersToDelete
        {
            get
            {
                if (ViewState["PhoneNumbersToDelete"] == null)
                {
                    ViewState["PhoneNumbersToDelete"] = new List<PhoneNumber>();
                }
                return (List<PhoneNumber>)ViewState["PhoneNumbersToDelete"];
            }
            set
            {
                ViewState["PhoneNumbersToDelete"] = value;
            }
        }

        private List<PhoneNumber> PhoneNumbersToUpdate
        {
            get
            {
                if (ViewState["PhoneNumbersToUpdate"] == null)
                {
                    ViewState["PhoneNumbersToUpdate"] = new List<PhoneNumber>();
                }
                return (List<PhoneNumber>)ViewState["PhoneNumbersToUpdate"];
            }
            set
            {
                ViewState["PhoneNumbersToUpdate"] = value;
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

        private void ReBindPhoneNumDataSource()
        {
            PhoneNumRepeater.DataSource = DynamicPhoneNumbers.OrderBy(x => x.Id);
            PhoneNumRepeater.DataBind();
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            string firstName = txtFirstName.Text.Trim();
            string lastName = txtLastName.Text.Trim();
            string emailAddress = txtEmailAddress.Text.Trim();
            int age = default;
            bool ageIsInt = true;
            byte[] profilePicture = null;
            bool isDefaultProfilePicture = IsDefaultProfilePicture();
            bool hasImage = ImageUpload.HasFile;
            var dbNull = DBNull.Value;           
            bool requiredFieldsAreEmpty = String.IsNullOrWhiteSpace(firstName) || String.IsNullOrWhiteSpace(lastName);

            if (!requiredFieldsAreEmpty)
            {
                if (hasImage)
                {
                    profilePicture = ImageUpload.FileBytes;

                }

                try
                {
                    age = Convert.ToInt32(txtAge.Text);
                }
                catch (Exception)
                {
                    throw;
                }

                if (!string.IsNullOrWhiteSpace(txtAge.Text))
                {
                    ageIsInt = int.TryParse(txtAge.Text, out age);
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

                        try
                        {
                            foreach (var phoneNum in NewPhoneNumbers)
                            {
                                using (OracleCommand command = new OracleCommand(cmdText, con))
                                {
                                    command.Parameters.AddWithValue("phone_number", phoneNum.Number);
                                    command.Parameters.AddWithValue("contact_id", Convert.ToInt32(HiddenIdField.Value));

                                    command.Connection.Open();
                                    command.ExecuteNonQuery();
                                    command.Connection.Close();
                                }
                            }
                        }
                        catch (Exception)
                        {
                            formAlert.Visible = true;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "contactModalScript", "showContactModal();", true);
                        }

                    }
                    catch (Exception)
                    {
                        if (!ageIsInt)
                        {
                            formAlert.InnerText = "Age must be a number!";
                        }

                        formAlert.Visible = true;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "contactModalScript", "showContactModal();", true);
                    }

                    if (!requiredFieldsAreEmpty)
                    {
                        Response.Redirect(String.Format("~/ContactDetails.aspx?id={0}", Convert.ToInt32(HiddenIdField.Value)));
                    }

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
                        formAlert.Visible = true;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "contactModalScript", "showContactModal();", true);
                    }

                    cmdText = "insert into PHONENUMBERS " +
                   "(phone_number, contact_id)" +
                   " VALUES (:phone_number, :contact_id)";

                    try
                    {
                        foreach (var phoneNum in NewPhoneNumbers)
                        {

                            using (OracleCommand command = new OracleCommand(cmdText, con))
                            {
                                command.Parameters.AddWithValue("phone_number", phoneNum.Number);
                                command.Parameters.AddWithValue("contact_id", Convert.ToInt32(HiddenIdField.Value));

                                command.Connection.Open();
                                command.ExecuteNonQuery();
                                command.Connection.Close();
                            }

                        }
                    }
                    catch (Exception)
                    {
                        formAlert.InnerText = "The contact you're trying to insert a phonenumber doesn't exist!";
                        formAlert.Visible = true;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "contactModalScript", "showContactModal();", true);
                    }

                    BtnHiddenFIeld.Value = "1";
                    AddOrUpdatePhoneNumHiddenField.Value = "1";
                    EmptySubmitForm();
                    FormUpdatePanel.Update();

                    foreach (var phoneNum in PhoneNumbersToDelete)
                    {

                        try
                        {
                            OracleCommand cmd = new OracleCommand();

                            cmd.Parameters.AddWithValue("id", phoneNum.Id);
                            cmd.CommandText = "DELETE FROM PHONENUMBERS WHERE id=:id";
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            cmd.Connection.Close();
                        }
                        catch (Exception)
                        {
                            formAlert.InnerText = "The contact you're trying to delete a phonenumber from doesn't exist!";
                            formAlert.Visible = true;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "contactModalScript", "showContactModal();", true);
                        }
                    }

                    foreach (var phoneNum in PhoneNumbersToUpdate)
                    {
                        if (phoneNum.Id != 0)
                        {
                            try
                            {
                                OracleCommand cmd = new OracleCommand();

                                cmd.Parameters.AddWithValue("id", phoneNum.Id);
                                cmd.Parameters.AddWithValue("phoneNumber", phoneNum.Number);
                                cmd.CommandText = "UPDATE PHONENUMBERS SET PHONE_NUMBER=:phoneNumber WHERE ID =:id";
                                cmd.Connection = con;
                                con.Open();
                                cmd.ExecuteNonQuery();
                                cmd.Connection.Close();
                            }
                            catch (Exception)
                            {
                                formAlert.InnerText = "The contact you're trying to update a phonenumber from doesn't exist!";
                                formAlert.Visible = true;
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "contactModalScript", "showContactModal();", true);
                            }
                        }
                        else
                        {
                            using (OracleCommand command = new OracleCommand(cmdText, con))
                            {
                                command.Parameters.AddWithValue("phone_number", phoneNum.Number);
                                command.Parameters.AddWithValue("contact_id", Convert.ToInt32(HiddenIdField.Value));

                                command.Connection.Open();
                                command.ExecuteNonQuery();
                                command.Connection.Close();
                            }
                        }
                    }
                }

                if (!requiredFieldsAreEmpty)
                {
                    EmptySubmitForm();
                    Response.Redirect(String.Format("~/ContactDetails.aspx?id={0}", Convert.ToInt32(HiddenIdField.Value)));
                }
            }

            formAlert.Visible = true;
            phoneNumAlert.Visible = false;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "contactModalScript", "showContactModal();", true);
            FormUpdatePanel.Update();
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
            Response.Redirect(String.Format("~/ContactDetails.aspx?id={0}", e.CommandArgument));
        }

        protected void RemoveNumberBtn_Command(object sender, CommandEventArgs e)
        {
            PhoneNumber phoneToRemove = DynamicPhoneNumbers.Where(p => p.Number == e.CommandArgument.ToString()).FirstOrDefault();
            PhoneNumber phoneInNewPhoneNumbers = NewPhoneNumbers.Where(p => p.Number == phoneToRemove.Number).FirstOrDefault();

            if (phoneToRemove != default)
            {
                if (phoneInNewPhoneNumbers != null)
                {
                    NewPhoneNumbers.Remove(phoneInNewPhoneNumbers);
                }
                else
                {
                    PhoneNumbersToDelete.Add(phoneToRemove);
                }

                DynamicPhoneNumbers.Remove(phoneToRemove);
            }

            ReBindPhoneNumDataSource();

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
            PhoneNumberHiddenField.Value = phoneNumber;
        }

        protected void AddOrEditPhoneNumBtn_Command(object sender, CommandEventArgs e)
        {
            string phoneNumber = txtPhoneNumber.Text;

            if (phoneNumber.Length <= 15)
            {
                if (AddOrUpdatePhoneNumHiddenField.Value == "1")
                {
                    if (!string.IsNullOrWhiteSpace(phoneNumber) && !DynamicPhoneNumbers.Any(p => p.Number == phoneNumber))
                    {
                        phoneNumAlert.Visible = false;
                        DynamicPhoneNumbers.Add(new PhoneNumber(phoneNumber));
                        NewPhoneNumbers.Add(new PhoneNumber(phoneNumber));
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
                    PhoneNumber newPhoneNumber = NewPhoneNumbers.Where(p => p.Number == PhoneNumberHiddenField.Value.ToString()).FirstOrDefault();

                    if (newPhoneNumber != null)
                    {
                        NewPhoneNumbers.Remove(newPhoneNumber);
                    }
                    phoneToUpdate.Number = phoneNumber;
                    PhoneNumbersToUpdate.Add(phoneToUpdate);
                    ReBindPhoneNumDataSource();
                }

                txtPhoneNumber.Text = string.Empty;
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
            EmptySubmitForm();
            formAlert.Visible = false;
            contactImg.ImageUrl = Common.CommonConstants.DefaultContactImageUrl;

            PhoneNumRepeater.DataSource = null;
            PhoneNumRepeater.DataBind();

            FormUpdatePanel.Update();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "contactModalScript", "showContactModal();", true);
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