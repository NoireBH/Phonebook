<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Crud.aspx.cs" Inherits="CRUDTEST.Crud" %>

<%@ Register Src="~/UserControls/ModalUserControl.ascx" TagName="Modal" TagPrefix="uc" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="Styles/style.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap/5.3.3/css/bootstrap.min.css" integrity="sha512-jnSuA4Ss2PkkikSOLtYs8BlYIeeIK1h99ty4YfvRPAlzr377vr3CXDb7sb7eEEBYjDtcYj+AjBH3FLv5uSJuXg==" crossorigin="anonymous" referrerpolicy="no-referrer" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.2/css/all.min.css" integrity="sha512-SnH5WK+bZxgPHs44uWIX+LLJAJ9/2PkPKZ5QiAj6Ta86w+fsb2TkcmfRyVX3pBnMFcV7oQPJkl9QevSCWr3W6A==" crossorigin="anonymous" referrerpolicy="no-referrer" />
    <title>Phonebook</title>

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
                <asp:UpdatePanel ID="SearchUpdatePanel" runat="server" UpdateMode="Conditional">
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
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="phonebook-container container-fluid text-center mb-2">
                    <asp:UpdatePanel ID="AddContactBtnUpdatePanel" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="AddContactBtn" runat="server" Text="Add a contact" OnClick="AddContactBtn_Click"
                                class="btn btn-success fw-bold" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <uc:Modal ID="ModalUserControl" runat="server" OnModalSelected="ModalUserControl_ModalSelected" />
                </div>
            </div>
            <div class="container-fluid">
            </div>
            <div class="row d-flex justify-content-center mb-3">
                <div class="col">
                    <asp:UpdatePanel ID="ContactsUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <div runat="server" id="ContactAlert" class="alert alert-danger fw-bolder text-black text-center" visible="false" role="alert">
                                No contacts with that name found!            
                            </div>
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
        <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.7.1/jquery.min.js" integrity="sha512-v2CJ7UaYy4JwqLDIrZUI/4hqeoQieOmAZNXBeQyjo21dadnwR+8ZaIJVT8EE2iyI61OV8e6M8PP2/4hpQINQ/g==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap/5.3.3/js/bootstrap.min.js" integrity="sha512-ykZ1QQr0Jy/4ZkvKuqWn4iF3lqPZyij9iRv6sGqLRdTPkY69YX6+7wvVGmsdBbiIfN/8OdsI7HABjvEok6ZopQ==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.2/js/all.min.js" integrity="sha512-u3fPA7V8qQmhBPNT5quvaXVa1mnnLSXUep5PS1qo5NRzHwG19aHmNJnj1Q8hpA/nBWZtZD4r4AX6YOt5ynLN2g==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
    </form>
</body>
</html>
