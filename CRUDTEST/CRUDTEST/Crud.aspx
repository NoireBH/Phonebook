<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Crud.aspx.cs" Inherits="CRUDTEST.Crud" %>

<%--<%@ Register Src="~/UserControls/ModalUserControl.ascx" TagPrefix="uc1" TagName="Modal" %>--%>



<!DOCTYPE html>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="Styles/style.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap/5.3.3/css/bootstrap.min.css" integrity="sha512-jnSuA4Ss2PkkikSOLtYs8BlYIeeIK1h99ty4YfvRPAlzr377vr3CXDb7sb7eEEBYjDtcYj+AjBH3FLv5uSJuXg==" crossorigin="anonymous" referrerpolicy="no-referrer" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.2/css/all.min.css" integrity="sha512-SnH5WK+bZxgPHs44uWIX+LLJAJ9/2PkPKZ5QiAj6Ta86w+fsb2TkcmfRyVX3pBnMFcV7oQPJkl9QevSCWr3W6A==" crossorigin="anonymous" referrerpolicy="no-referrer" />
    <title>Crud Operations</title>
    <style type="text/css">
        .auto-style1 {
            max-width: 500px;
        }

        .auto-style2 {
            height: 27px;
        }
    </style>
    <script>
        function img() {
            var url = inputToURL(document.getElementById("<%=ImageUpload.ClientID %>"));
            document.getElementById("<%=contactImg.ClientID %>").src = url;
        }
        function inputToURL(inputElement) {
            var file = inputElement.files[0];
            return window.URL.createObjectURL(file);
        }
        function showContactModal() {
            var modal = new bootstrap.Modal(document.getElementById('contactModal'));
            modal.show();
        }
        function hideContactModal() {
            $('#contactModal').modal('hide');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="FormScriptManager" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="AlertUpdatePanel" runat="server">
            <ContentTemplate>
                <div runat="server" id="AlertTopFixed" class="alert alert-danger fw-bolder text-black text-center" visible="false" role="alert">
                    An error has occured, please try again!
                </div>
            </ContentTemplate>

        </asp:UpdatePanel>
        <div class="container-fluid" style="max-width: 800px; border: 1px solid black; background-color: white">
            <div class="container-fluid">
                <header class="d-flex justify-content-center align-items-baseline gap-1">
                    <i class="fa-solid fa-address-book fa-2xl" style="color: #004080;"></i>
                    <h1 class="text-center" style="color: #004080;">PhoneBook
                    </h1>
                </header>
                <div class="phonebook-container container-fluid text-center mb-2">
                    <asp:UpdatePanel ID="AddContactBtnUpdatePanel" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="AddContactBtn" runat="server" Text="Add a contact" OnClick="AddContactBtn_Click"
                                class="btn btn-success fw-bold" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="modal fade" id="contactModal">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-body">
                                    <div class="text-center form-container container-fluid">
                                        <asp:UpdatePanel ID="FormUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                                            <ContentTemplate>
                                                <div class="container-fluid field-form d-flex  flex-column justify-content-center gap-2 mb-3" style="max-width: 500px;">
                                                    <div runat="server" id="formAlert" class="alert alert-danger fw-bolder text-black" visible="false" role="alert">
                                                        Please fill out all required fields!
                                                    </div>
                                                    <div class="form-group d-flex flex-column">
                                                        <asp:Label ID="lblFirstName" CssClass="fw-bold" runat="server" Text="First Name:"></asp:Label>
                                                        <p class="required-field">Field is required*</p>
                                                        <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ControlToValidate="txtFirstName" runat="server" ValidationGroup="contactValGroup" />

                                                    </div>
                                                    <div class="form-group d-flex flex-column">
                                                        <asp:Label ID="lblLastName" CssClass="fw-bold" runat="server" Text="Last Name:"></asp:Label>
                                                        <p class="required-field">Field is required*</p>
                                                        <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ControlToValidate="txtLastName" runat="server" ValidationGroup="contactValGroup" />

                                                    </div>
                                                    <div class="form-group d-flex flex-column">
                                                        <asp:Label ID="lblEmailAddress" runat="server" CssClass="fw-bold mb-3" Text="Email Address:"></asp:Label>
                                                        <asp:TextBox ID="txtEmailAddress" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <asp:RegularExpressionValidator ErrorMessage="Invalid email address" ControlToValidate="txtEmailAddress" runat="server" ValidationGroup="contactValGroup" />
                                                    </div>
                                                    <div class="form-group d-flex flex-column">
                                                        <asp:Label ID="lblAge" runat="server" CssClass="fw-bold mb-3" Text="Age:"></asp:Label>
                                                        <asp:TextBox ID="txtAge" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <asp:RegularExpressionValidator ID="regexNumbersOnly"
                                                            ControlToValidate="txtAge" runat="server"
                                                            ErrorMessage="Only Numbers allowed"
                                                            ValidationExpression="\d+" ValidationGroup="contactValGroup">
                                                        </asp:RegularExpressionValidator>
                                                    </div>
                                                    <asp:UpdatePanel ID="PhoneNumUpdatePanel" runat="server">
                                                        <ContentTemplate>
                                                            <div class="form-group d-flex flex-column phone-numbers-container">
                                                                <h4 class="fw-bold mb-3">Phone numbers:</h4>
                                                                <asp:TextBox ID="txtPhoneNumber" runat="server"></asp:TextBox>
                                                                <asp:Button ID="AddOrEditPhoneNumBtn" CssClass="btn btn-success mb-2" runat="server"
                                                                    Text="Save" OnCommand="AddOrEditPhoneNumBtn_Command" />
                                                                <div runat="server" id="phoneNumAlert" class="alert alert-danger fw-bolder text-black" visible="false" role="alert">
                                                                    Can't add an empty or already existing phonenumber!
                                                                </div>
                                                                <asp:Repeater ID="PhoneNumRepeater" runat="server">
                                                                    <ItemTemplate>
                                                                        <asp:PlaceHolder ID="PlaceHolderPhoneRepeater" runat="server">
                                                                            <asp:TextBox ID="txtAddOrEditphoneNum" runat="server" CssClass="text-center" Text='<%# DataBinder.Eval(Container.DataItem, "Number") %>'>
                                                                            </asp:TextBox>

                                                                        </asp:PlaceHolder>

                                                                        <div class="phone-number-buttons d-flex justify-content-center gap-3 align-items-baseline mb-2 mt-2 ">
                                                                            <asp:Button ID="RemoveNumberBtn" class="btn btn-danger fw-bold text-dark" runat="server" Text="Delete"
                                                                                OnCommand="RemoveNumberBtn_Command"
                                                                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Number") %>'></asp:Button>
                                                                            <asp:Button ID="UpdatePhoneNumBtn" Text="Update"
                                                                                class="btn btn-warning fw-bold text-dark" runat="server" OnCommand="UpdatePhoneNumBtn_Command"
                                                                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Number") + "," 
                                                                                        + DataBinder.Eval(Container.DataItem, "Id") %>' />
                                                                        </div>
                                                                    </ItemTemplate>
                                                                </asp:Repeater>
                                                            </div>
                                                            <asp:HiddenField ID="AddOrUpdatePhoneNumHiddenField" Value="1" runat="server" />
                                                            <asp:HiddenField ID="PhoneNumberIdHiddenField" runat="server" />
                                                            <asp:HiddenField ID="PhoneNumberHiddenField" runat="server" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>

                                                    <div class="form-group d-flex flex-column">
                                                        <asp:Label ID="lblContactImagePreview" runat="server" CssClass="fw-bold mb-3" Text="Profile Picture:"></asp:Label>
                                                        <div class="contact-img-container container-fluid text-center mb-2">
                                                            <asp:Image ID="contactImg" ImageUrl="Images/blank-pfp.png" CssClass="contact-img" runat="server" />
                                                        </div>
                                                        <asp:FileUpload CssClass="text-center" ID="ImageUpload" runat="server" onchange="img()" />
                                                    </div>
                                                </div>
                                                <div class="d-flex justify-content-center gap-2">
                                                    <asp:Button ID="submitBtn" runat="server" Text="Submit" class="btn btn-success fw-bold" OnClick="Submit_Click" />
                                                    <asp:Button ID="CancelUpdBtn" runat="server" type="button" class="btn btn-secondary fw-bold" OnClick="CancelUpdBtn_Click" Text="Cancel"></asp:Button>
                                                </div>
                                                <asp:HiddenField ID="BtnHiddenFIeld" Value="1" runat="server" />
                                                <asp:HiddenField ID="HiddenIdField" runat="server" />

                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="submitBtn" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <h1 class="text-center">My Contacts:
                    </h1>
                </div>
                <div class="d-flex justify-content-center mb-3">
                    <asp:UpdatePanel ID="RepeaterUpdatePanel" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Repeater ID="Repeater" runat="server">
                                <HeaderTemplate>
                                    <table border="1" style="max-width: 500px;">
                                        <tr>
                                            <td class="text-center"><b>First Name</b></td>
                                            <td class="text-center"><b>Last Name</b></td>
                                        </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td class="text-center"><%# DataBinder.Eval(Container.DataItem, "firstName") %> </td>
                                        <td class="text-center"><%# DataBinder.Eval(Container.DataItem, "lastName") %> </td>
                                        <td class="text-center">
                                            <asp:Button ID="DeleteBtn" runat="server" Text="Delete" class="btn btn-danger fw-bold text-dark" OnCommand="DeleteBtn_Command"
                                                CommandArgument='<%# DataBinder.Eval
                                            (Container.DataItem, "id") %>'
                                                OnClientClick='<%# String.Format
                                                    ("return confirm(\"Are you sure you want to delete {0} {1}?\");", Eval("firstName"), Eval("lastName")) %>' />
                                        </td>
                                        <td class="text-center">
                                            <asp:Button ID="UpdateBtn" runat="server" Text="Update"
                                                class="btn btn-warning fw-bold text-dark" OnCommand="UpdateBtn_Command"
                                                CommandArgument='<%# DataBinder.Eval
                                          (Container.DataItem, "id")%>' />
                                        </td>
                                        <td class="text-center">
                                            <asp:Button ID="DetailsBtn" runat="server" class="btn btn-info fw-bold text-dark" Text="Details"
                                                OnCommand="DetailsBtn_Command" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <br />
                </div>
            </div>
        </div>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.7.1/jquery.min.js" integrity="sha512-v2CJ7UaYy4JwqLDIrZUI/4hqeoQieOmAZNXBeQyjo21dadnwR+8ZaIJVT8EE2iyI61OV8e6M8PP2/4hpQINQ/g==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap/5.3.3/js/bootstrap.min.js" integrity="sha512-ykZ1QQr0Jy/4ZkvKuqWn4iF3lqPZyij9iRv6sGqLRdTPkY69YX6+7wvVGmsdBbiIfN/8OdsI7HABjvEok6ZopQ==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.2/js/all.min.js" integrity="sha512-u3fPA7V8qQmhBPNT5quvaXVa1mnnLSXUep5PS1qo5NRzHwG19aHmNJnj1Q8hpA/nBWZtZD4r4AX6YOt5ynLN2g==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
        <asp:SqlDataSource ID="PLSQLDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString2 %>" ProviderName="<%$ ConnectionStrings:ConnectionString2.ProviderName %>" SelectCommand="SELECT * FROM &quot;CONTACTS&quot; ORDER BY &quot;ID&quot;"></asp:SqlDataSource>
        <asp:SqlDataSource ID="PhoneNumbers" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>" SelectCommand="SELECT &quot;ID&quot;, &quot;PHONE_NUMBER&quot; FROM &quot;PHONENUMBERS&quot; WHERE (&quot;CONTACT_ID&quot; = :CONTACT_ID) ORDER BY &quot;ID&quot;">
            <SelectParameters>
                <asp:ControlParameter ControlID="HiddenIdField" Name="CONTACT_ID" PropertyName="Value" Type="Decimal" />
            </SelectParameters>
        </asp:SqlDataSource>
    </form>
</body>
</html>
