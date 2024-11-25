﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data.OracleClient;
using System.Data;
using CRUDTEST.Models;
using CRUDTEST.Common;
using System.Web.UI;
using Image = System.Web.UI.WebControls.Image;
using static CRUDTEST.UserControls.ModalUserControl;

namespace CRUDTEST
{
    //tortoise test
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

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                ModalUserControl.ModalSelected += new ModalHandler(ModalUserControl_ModalSelected);
                Image contactImg = ModalUserControl.ContactImage;
                contactImg.Style.Add("max-width", "400px");
                contactImg.Style.Add("max-height", "300px");
                LoadContacts();
            }
            else
            {
                AlertTopFixed.Visible = false;
            }
        }

        public void LoadContacts()
        {
            string searchInput = txtsearchcontact.Text.ToLower();

            string cmdText = "SELECT * FROM CONTACTS WHERE LOWER(FIRST_NAME) LIKE LOWER(:searchInput) || '%' OR LOWER(LAST_NAME) LIKE LOWER(:searchInput) || '%'";

            if (!string.IsNullOrEmpty(searchInput))
            {
                cmdText = string.Format(cmdText, "WHERE LOWER(FIRST_NAME) LIKE LOWER(:searchInput) || '%' OR LOWER(LAST_NAME) LIKE LOWER(:searchInput) || '%'");
            }

                var values = new List<PhoneContact>();

                try
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.Parameters.AddWithValue("searchInput", searchInput);
                    cmd.CommandText = cmdText;
                    cmd.Connection = con;
                    con.Open();
                    OracleDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            values.Add(new PhoneContact(Convert.ToInt32(dr["id"]), dr["first_name"].ToString(),
                            dr["last_name"].ToString(), dr["email_address"].ToString(), Convert.ToInt32(dr["age"].ToString())));
                        }
                    }
                    else
                    {
                        ContactAlert.Visible = true;
                    }

                }
                catch (Exception)
                {
                    AlertTopFixed.InnerText = "Something went wrong while trying to load the contacts, please try again.";
                    AlertTopFixed.Visible = true;
                }

                if (values.Count > 0)
                {
                    ContactAlert.Visible = false;
                    ContactsRepeater.DataSource = values.OrderBy(x => x.Id);
                    ContactsRepeater.DataBind();
                }
                else
                {
                    ContactsRepeater.DataSource = null;
                    ContactsRepeater.DataBind();
                    ContactAlert.Visible = true;
                }           

            ContactsUpdatePanel.Update();
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

                LoadContacts();

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
            LoadContacts();
        }

        protected void ModalUserControl_ModalSelected(object sender, EventArgs e)
        {
            LoadContacts();
        }
    }
}