<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Crud.aspx.cs" Inherits="CRUDTEST.Crud" %>
<%@ Register Src="~/UserControls/ModalUserControl.ascx" TagName="Modal" TagPrefix="uc1" %>



<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="Styles/style.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap/5.3.3/css/bootstrap.min.css" integrity="sha512-jnSuA4Ss2PkkikSOLtYs8BlYIeeIK1h99ty4YfvRPAlzr377vr3CXDb7sb7eEEBYjDtcYj+AjBH3FLv5uSJuXg==" crossorigin="anonymous" referrerpolicy="no-referrer" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.2/css/all.min.css" integrity="sha512-SnH5WK+bZxgPHs44uWIX+LLJAJ9/2PkPKZ5QiAj6Ta86w+fsb2TkcmfRyVX3pBnMFcV7oQPJkl9QevSCWr3W6A==" crossorigin="anonymous" referrerpolicy="no-referrer" />
    <title>Phonebook</title>
    <script>
        function img() {
            var inputElement = document.getElementById("<%= ImageUpload.ClientID %>");
            var imgElement = document.getElementById("<%= contactImg.ClientID %>");
            var maxSizeInMB = 3;
            var maxSizeInBytes = maxSizeInMB * 1024 * 1024;

            if (inputElement.files && inputElement.files[0]) {
                var file = inputElement.files[0];
                var fileSize = file.size;

                if (fileSize > maxSizeInBytes) {
                    alert('File size must be less than ' + maxSizeInMB + ' MB.');
                    inputElement.value = '';

                    return false;
                }

                var fileName = file.name.toLowerCase();
                if (!fileName.endsWith('.jpg') && !fileName.endsWith('.jpeg') && !fileName.endsWith('.png') && !fileName.endsWith('.gif')) {
                    alert('Please upload a JPG, JPEG, PNG, or GIF image file.');
                    inputElement.value = '';
                    return false;
                }

                var url = window.URL.createObjectURL(file);
                imgElement.src = url;
            }

        }
        function showContactModal() {
            var modal = new bootstrap.Modal(document.getElementById('contactModal'));
            modal.show();
        }
        function hideContactModal() {
            $('#contactModal').modal('hide');
        }
        function validateForm() {
            var firstName = document.getElementById('<%= textFirstName.ClientID %>').value.trim();
            var lastName = document.getElementById('<%= textLastName.ClientID %>').value.trim();
            var isValid = true;

            if (firstName === '' || lastName === '') {
                document.getElementById('<%= formAlert.ClientID %>').style.display = 'block';
                isValid = false;
            } else {
                document.getElementById('<%= formAlert.ClientID %>').stsyle.display = 'none';
            }

            return isValid;
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
        <div class="container-fluid">
            <div class="container-fluid">
                <header class="d-flex justify-content-center align-items-baseline gap-1">
                    <i class="fa-solid fa-address-book fa-2xl" style="color: #004080;"></i>
                    <h1 class="text-center" style="color: #004080;">PhoneBook
                    </h1>
                </header>
                <asp:UpdatePanel ID="searchupdatepanel" runat="server">
                    <ContentTemplate>
                        <div class="container-fluid text-center search-contact-container mb-3">
                            <h3>search for a contact</h3>
                            <div class="container-fluid d-flex justify-content-center align-items-baseline gap-2">
                                <asp:TextBox ID="txtsearchcontact" runat="server"></asp:TextBox>
                                <asp:LinkButton ID="searchcontactbtn" runat="server" OnClick="SearchContactBtn_Click">
                    <i class="fa-solid fa-magnifying-glass fa-lg" style="color: #004080;"></i>
                                </asp:LinkButton>
                            </div>
                        </div>
                        <asp:Repeater ID="FoundContactsRepeater" runat="server">
                        </asp:Repeater>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="phonebook-container container-fluid text-center mb-2">
                    <asp:UpdatePanel ID="AddContactBtnUpdatePanel" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="AddContactBtn" runat="server" Text="Add a contact" OnClick="AddContactBtn_Click"
                                class="btn btn-success fw-bold" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <uc1:Modal ID="ModalUserControl" runat="server" />
                    <h1 class="text-center">My Contacts:
                    </h1>
                </div>
                <div class="container-fluid">
                </div>
                <div class="row d-flex justify-content-center mb-3">
                    <div class="col">
                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                            <ContentTemplate>
                                <asp:Repeater ID="ContactsRepeater" runat="server">
                                    <HeaderTemplate>
                                        <table class="table table-hover">
                                            <thead>
                                                <tr>
                                                    <th class="text-center" scope="col"></th>
                                                    <th class="text-center" scope="col">First Name</th>
                                                    <th class="text-center" scope="col">Last Name</th>
                                                </tr>
                                            </thead>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tbody>
                                            <tr>
                                                <td class="text-center d-flex gap-3">
                                                    <asp:LinkButton ClientIDMode="AutoID" ID="DeleteBtn" runat="server" CssClass="fw-bold text-dark btn-icon"
                                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>'
                                                        OnCommand="DeleteBtn_Command"
                                                        OnClientClick='<%# String.Format
             ("return confirm(\"Are you sure you want to delete {0} {1}?\");", Eval("firstName"), Eval("lastName")) %>'>
             <i class="fa-solid fa-trash-can" style="color: #ff0000;"></i> 
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ClientIDMode="AutoID" ID="UpdateBtn" runat="server" CssClass="btn-icon" OnCommand="UpdateBtn_Command"
                                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>'>
             <i class="fa-solid fa-pen-to-square" style="color: #FFD43B;"></i> 
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ClientIDMode="AutoID" ID="DetailsBtn" runat="server" CssClass="btn-icon" Text="Details"
                                                        OnCommand="DetailsBtn_Command" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>'>
             <i class="fa-solid fa-circle-info" style="color: #74C0FC;"></i>
                                                    </asp:LinkButton>
                                                </td>
                                                <td class="text-center fw-bold"><%# DataBinder.Eval(Container.DataItem, "firstName") %> </td>
                                                <td class="text-center fw-bold"><%# DataBinder.Eval(Container.DataItem, "lastName") %> </td>
                                            </tr>
                                        </tbody>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
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
