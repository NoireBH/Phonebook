using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data.OracleClient;
using System.Data;
using CRUDTEST.Models;
using CRUDTEST.Common;
using System.Web.UI;
using Image = System.Web.UI.WebControls.Image;

namespace CRUDTEST
{

    public partial class Crud : System.Web.UI.Page
    {
        OracleConnection con = new OracleConnection(@"Data Source=oratest19/odbms;USER ID=EMustafov; password=manager;");

        public List<PhoneNumber> DynamicPhoneNumbers
        {
            get
            {
                return ModalUserControl.DynamicPhoneNumbers;
            }

            set
            {
                ModalUserControl.DynamicPhoneNumbers = value;
            }
        }

        public List<PhoneContact> Contacts
        {
            get
            {
                if (ViewState["Contacts"] == null)
                {
                    ViewState["Contacts"] = new List<PhoneContact>();
                }
                return (List<PhoneContact>)ViewState["Contacts"];
            }
            set
            {
                ViewState["Contacts"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadContacts();
            }
            else
            {
                AlertTopFixed.Visible = false;
            }
        }

        public void LoadContacts()
        {
            Image contactImg = ModalUserControl.ContactImage;

            contactImg.Style.Add("max-width", "400px");
            contactImg.Style.Add("max-height", "300px");

            Contacts.Clear();

            try
            {
                OracleCommand cmd = new OracleCommand();
                cmd.CommandText = "select * from CONTACTS";
                cmd.Connection = con;
                con.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    
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

                        Contacts.Add(new PhoneContact(int.Parse(dr["id"].ToString()), dr["first_name"].ToString(), dr["last_name"].ToString(),
                            dr["email_address"].ToString(), Convert.ToInt32(dr["age"]), pfp));
                    }

                    ContactsRepeater.DataSource = Contacts.OrderBy(x => x.Id);
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

            ContactsUpdatePanel.Update();
        }

        protected void ShowBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (Contacts.Count > 0)
                {
                    ContactAlert.Visible = false;
                    AlertTopFixed.Visible = false;
                    ContactsRepeater.DataSource = Contacts.OrderBy(x => x.Id);
                    ContactsRepeater.DataBind();
                }
                else
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

                            while (dr.Read())
                            {
                                Contacts.Add(new PhoneContact(int.Parse(dr["id"].ToString()), dr["first_name"].ToString(),
                                    dr["last_name"].ToString(), dr["email_address"].ToString(), Convert.ToInt32(dr["age"])));
                            }

                            ContactsRepeater.DataSource = Contacts.OrderBy(x => x.Id);
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

                    ModalUserControl.UpdateFormPanelContent();
                }
            }
            catch (Exception)
            {
                AlertTopFixed.InnerText = "Something went wrong when trying to display the contacts, please try again!";
                AlertTopFixed.Visible = true;
            }

            ModalUserControl.UpdateFormPanelContent();
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

                var contactToRemove = Contacts.Where(x => x.Id == Convert.ToInt32(e.CommandArgument)).FirstOrDefault();

                if (contactToRemove != null)
                {
                    Contacts.Remove(contactToRemove);
                }

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
            ModalUserControl.FormAlert.Visible = false;
            Image contactImg = ModalUserControl.ContactImage;

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
                        ModalUserControl.TextFirstName.Value = dr["first_name"].ToString();
                        ModalUserControl.TextLastName.Value = dr["last_name"].ToString();
                        ModalUserControl.TextEmailAddress.Value = dr["email_address"].ToString();

                        if (dr["age"].ToString() == "0")
                        {
                            ModalUserControl.TextAge.Value = string.Empty;
                        }
                        else
                        {
                            ModalUserControl.TextAge.Value = dr["age"].ToString();
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
                ModalUserControl.FormAlert.InnerText = "The contact doesn't exist!";
                ModalUserControl.FormAlert.Visible = true;
            }

            ModalUserControl.AddOrUpdateBtnHiddenFieldValue = "0";
            ModalUserControl.HiddenIdFieldValue = e.CommandArgument.ToString();

            List<PhoneNumber> phoneNumbers = new List<PhoneNumber>();

            try
            {
                OracleCommand cmdGetPhoneNums = new OracleCommand();
                cmdGetPhoneNums.Parameters.AddWithValue("id", ModalUserControl.HiddenIdFieldValue);
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

            ModalUserControl.PhoneNumberRepeater.DataSource = DynamicPhoneNumbers.OrderByDescending(x => x.Id);
            ModalUserControl.PhoneNumberRepeater.DataBind();

            con.Close();

            ModalUserControl.UpdateFormPanelContent();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "contactModalScript", "showContactModal();", true);
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

        protected void AddContactBtn_Click(object sender, EventArgs e)
        {
            try
            {
                ModalUserControl.EmptySubmitForm();
                ModalUserControl.FormAlert.Visible = false;
                ModalUserControl.ContactImage.ImageUrl = CommonConstants.DefaultContactImageUrl;

                ModalUserControl.ClearPhoneNumbers();
                ModalUserControl.UpdateFormPanelContent();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "contactModalScript", "showContactModal();", true);
            }
            catch (Exception)
            {
                AlertTopFixed.Visible = true;
            }

        }

        protected void SearchContactBtn_Click(object sender, EventArgs e)
        {
            string searchInput = txtsearchcontact.Text.ToLower();

            if (string.IsNullOrWhiteSpace(searchInput))
            {
                ContactAlert.Visible = false;
                LoadContacts();
            }
            else
            {
                try
                {
                    OracleCommand cmdGetContacts = new OracleCommand();
                    cmdGetContacts.Parameters.AddWithValue("searchInput", searchInput);
                    cmdGetContacts.CommandText = "SELECT * FROM CONTACTS WHERE LOWER(FIRST_NAME) LIKE LOWER(:searchInput) || '%' OR LOWER(LAST_NAME) LIKE LOWER(:searchInput) || '%'";
                    cmdGetContacts.Connection = con;
                    con.Open();
                    OracleDataReader dr = cmdGetContacts.ExecuteReader();

                    if (dr.HasRows)
                    {
                        Contacts.Clear();

                        while (dr.Read())
                        {
                            Contacts.Add(new PhoneContact(Convert.ToInt32(dr["id"]), dr["first_name"].ToString(),
                                dr["last_name"].ToString(), dr["email_address"].ToString(), Convert.ToInt32(dr["age"].ToString())));
                        }
                    }
                    else
                    {
                        AlertTopFixed.Visible = true;
                    }

                }
                catch (Exception)
                {
                    AlertTopFixed.InnerText = "Something went wrong while trying to load the contacts, please try again.";
                    AlertTopFixed.Visible = true;
                }

                if (Contacts.Count > 0)
                {
                    ContactAlert.Visible = false;
                    ContactsRepeater.DataSource = Contacts;
                    ContactsRepeater.DataBind();
                }
                else
                {
                    ContactsRepeater.DataSource = null;
                    ContactsRepeater.DataBind();
                    ContactAlert.Visible = true;
                }
            }

            ContactsUpdatePanel.Update();
        }

        public void UpdateContactsUpdatePanel()
        {
            ContactsUpdatePanel.Update();
        }

    }
}