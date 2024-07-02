
using CRUDTEST.Models;
using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRUDTEST.Common;
using Image = System.Web.UI.WebControls.Image;
using System.Web.UI.HtmlControls;
using Microsoft.Ajax.Utilities;

namespace CRUDTEST.UserControls
{
    public partial class ModalUserControl : System.Web.UI.UserControl
    {
        OracleConnection con = new OracleConnection(@"Data Source=oratest19/odbms;USER ID=EMustafov; password=manager;");

        public Image ContactImage
        {
            get { return contactImg; }
            set { contactImg = value; }
        }

        public HtmlGenericControl FormAlert
        {
            get { return formAlert; }
            set { formAlert = value; }
        }

        public HtmlInputText TextFirstName
        {
            get { return textFirstName; }
            set { textFirstName = value; }
        }

        public HtmlInputText TextLastName
        {
            get { return textLastName; }
            set { textLastName = value; }
        }

        public HtmlInputGenericControl TextEmailAddress
        {
            get { return textEmailAddress; }
            set { textEmailAddress = value; }
        }


        public HtmlInputGenericControl TextAge
        {
            get { return textAge; }
            set { textAge = value; }
        }

        public string HiddenIdFieldValue
        {
            get { return HiddenIdField.Value; }
            set { HiddenIdField.Value = value; }
        }

        public string AddOrUpdateBtnHiddenFieldValue
        {
            get { return BtnHiddenFIeld.Value; }
            set { BtnHiddenFIeld.Value = value; }
        }

        public List<PhoneNumber> DynamicPhoneNumbers
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

        public Repeater PhoneNumberRepeater
        {
            get { return PhoneNumRepeater; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

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

                    if (!string.IsNullOrWhiteSpace(phoneNumber) && !DynamicPhoneNumbers.Any(p => p.Number == phoneNumber))
                    {
                        phoneNumAlert.Visible = false;
                        phoneToUpdate.Number = phoneNumber;
                        ReBindPhoneNumDataSource();
                    }
                    else
                    {
                        phoneNumAlert.Visible = true;
                        phoneNumAlert.InnerText = "Can't add an empty or already existing phonenumber!";
                    }

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

        protected void Submit_Click(object sender, EventArgs e)
        {
            string firstName = textFirstName.Value.Trim();
            string lastName = textLastName.Value.Trim();
            bool requiredFieldsAreEmpty = String.IsNullOrWhiteSpace(firstName) || String.IsNullOrWhiteSpace(lastName);
            Crud mainPage = (Crud)Context.Handler;

            if (firstName.Length > 30 || lastName.Length > 30)
            {
                formAlert.InnerText = "First and last name needs to be no more than 30 characters long!";
            }
            else if (!requiredFieldsAreEmpty)
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

                        //Response.Redirect(String.Format("~/ContactDetails.aspx?id={0}", Convert.ToInt32(HiddenIdField.Value)));
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
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "hideContactModalScript", "hideContactModal();", true);
                    mainPage.UpdateContactsUpdatePanel();
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

            //formAlert.Visible = true;
            //phoneNumAlert.Visible = false;
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "contactModalScript", "showContactModal();", true);

            mainPage.LoadContacts();
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

        public void EmptySubmitForm()
        {
            textFirstName.Value = string.Empty;
            textLastName.Value = string.Empty;
            textEmailAddress.Value = string.Empty;
            textAge.Value = string.Empty;
            textPhoneNumber.Value = string.Empty;
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

        private bool IsDefaultProfilePicture() => contactImg.ImageUrl == CommonConstants.DefaultContactImageUrl;

        public void UpdateFormPanelContent()
        {
            FormUpdatePanel.Update();
        }

        public void ReBindPhoneNumDataSource()
        {
            PhoneNumRepeater.DataSource = DynamicPhoneNumbers.OrderByDescending(x => x.Id);
            PhoneNumRepeater.DataBind();
        }

        public void ClearPhoneNumbers()
        {
            DynamicPhoneNumbers.Clear();
            PhoneNumRepeater.DataSource = null;
            PhoneNumRepeater.DataBind();
        }
    }
}
