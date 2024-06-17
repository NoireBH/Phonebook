<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Crud.aspx.cs" Inherits="CRUDTEST.Crud" %>

<!DOCTYPE html>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="Styles/style.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap/5.3.3/css/bootstrap.min.css" integrity="sha512-jnSuA4Ss2PkkikSOLtYs8BlYIeeIK1h99ty4YfvRPAlzr377vr3CXDb7sb7eEEBYjDtcYj+AjBH3FLv5uSJuXg==" crossorigin="anonymous" referrerpolicy="no-referrer" />
    <title>Crud Operations</title>
    <style type="text/css">
        .auto-style1 {
            max-width: 500px;
        }

        .auto-style2 {
            height: 27px;
        }
    </style>

</head>
<body>

    <form id="form1" runat="server">
        <div class="container-fluid">
            <div class="container-fluid" style="max-width: 800px; border: 1px solid black; background-color: white">
                <h1 class="text-center">PhoneBook
                </h1>
                <div class="phonebook-container container-fluid text-center mb-2">
                    <button type="button" class="btn btn-success fw-bold" data-bs-toggle="modal" data-bs-target="#staticBackdrop">
                        Add a contact</button>
                    <div class="modal fade" id="staticBackdrop" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-body">
                                    <div class="text-center form-container container-fluid">
                                        <asp:ScriptManager ID="FormScriptManager" runat="server">
                                        </asp:ScriptManager>

                                        <asp:UpdatePanel ID="FormUpdatePanel" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div class="container-fluid field-form d-flex  flex-column justify-content-center gap-2 mb-3" style="max-width: 500px;">

                                                    <div class="form-group d-flex flex-column">
                                                        <asp:Label ID="lblFirstName" CssClass="fw-bold" runat="server" Text="First Name:"></asp:Label>
                                                        <p class="required-field">Field is required*</p>
                                                        <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ControlToValidate="txtFirstName" runat="server" />

                                                    </div>
                                                    <div class="form-group d-flex flex-column">
                                                        <asp:Label ID="lblLastName" CssClass="fw-bold" runat="server" Text="Last Name:"></asp:Label>
                                                        <p class="required-field">Field is required*</p>
                                                        <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ControlToValidate="txtLastName" runat="server" />

                                                    </div>
                                                    <div class="form-group d-flex flex-column">
                                                        <asp:Label ID="lblPhoneNumber" CssClass="fw-bold" runat="server" Text="Phone Number:"></asp:Label>
                                                        <p class="required-field">Field is required*</p>
                                                        <asp:TextBox ID="txtPhoneNumber" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <%--<asp:RegularExpressionValidator ID="PhoneNumberRegex" ValidationExpression="^\+?[0-9]+$" runat="server" 
                                                            ErrorMessage="Phone number must contain only numbers or a start with a country code ex. +359" 
                                                            ControlToValidate="txtPhoneNumber"></asp:RegularExpressionValidator>--%>
                                                    </div>
                                                    <div class="form-group d-flex flex-column">
                                                        <asp:Label ID="lblEmailAddress" runat="server" CssClass="fw-bold mb-3" Text="Email Address:"></asp:Label>
                                                        <asp:TextBox ID="txtEmailAddress" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                    <div class="form-group d-flex flex-column">
                                                        <asp:Label ID="lblAge" runat="server" CssClass="fw-bold mb-3" Text="Age:"></asp:Label>
                                                        <asp:TextBox ID="txtAge" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <asp:RegularExpressionValidator ID="regexNumbersOnly"
                                                            ControlToValidate="txtAge" runat="server"
                                                            ErrorMessage="Only Numbers allowed"
                                                            ValidationExpression="\d+">
                                                        </asp:RegularExpressionValidator>
                                                    </div>

                                                    <%--<asp:Button ID="UploadImgBtn" runat="server" Text="Upload Image" OnClick="UploadImgBtn_Click" />--%>
                                                    <div class="form-group d-flex flex-column">
                                                        <asp:Label ID="lblProfilePicture" runat="server" CssClass="fw-bold mb-3" Text="Profile Picture:"></asp:Label>
                                                        <asp:FileUpload CssClass="text-center" ID="ImageUpload" runat="server" />

                                                    </div>


                                                </div>
                                                <div class="d-flex justify-content-center gap-2">
                                                    <asp:Button ID="submitBtn" runat="server" Text="Submit" class="btn btn-success fw-bold" OnClick="Submit_Click" data-bs-dismiss="modal" />
                                                    <asp:Button runat="server" type="button" class="btn btn-secondary fw-bold" OnClick="CancelUpdBtn_Click" Text="Cancel" data-bs-dismiss="modal"></asp:Button>
                                                </div>

                                                <asp:HiddenField ID="BtnHiddenFIeld" Value="1" runat="server" OnValueChanged="BtnHiddenFIeld_ValueChanged" />
                                                <asp:HiddenField ID="HiddenIdField" runat="server" OnValueChanged="HiddenIdField_ValueChanged" />
                                                <asp:HiddenField ID="HiddenEmailAddressField" runat="server" OnValueChanged="HiddenEmailAddressField_ValueChanged" />
                                                <asp:HiddenField ID="HiddenAgeField" runat="server" OnValueChanged="HiddenAgeField_ValueChanged" />
                                                <asp:HiddenField ID="HiddenPictureField" runat="server" OnValueChanged="HiddenPictureField_ValueChanged" />
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


                    <h1>My Contacts:
                    </h1>
                    <%--<asp:UpdatePanel ID="ShowContactsUpdatePanel" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="ShowContactsBtn" runat="server" Text="Show Contacts" class="btn btn-success fw-bold" OnClick="ShowBtn_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>--%>
                </div>

                <div class="d-flex justify-content-center mb-3">

                    <asp:UpdatePanel ID="RepeaterUpdatePanel" runat="server">
                        <ContentTemplate>
                            <asp:Repeater ID="Repeater" runat="server" OnItemCommand="Repeater_ItemCommand">
                                <HeaderTemplate>
                                    <table border="1" style="max-width: 500px;">
                                        <tr>
                                            <td class="text-center"><b>First Name</b></td>
                                            <td class="text-center"><b>Last Name</b></td>
                                            <td class="text-center"><b>Phone Number</b></td>
                                        </tr>
                                </HeaderTemplate>

                                <ItemTemplate>
                                    <tr>
                                        <td class="text-center"><%# DataBinder.Eval(Container.DataItem, "firstName") %> </td>
                                        <td class="text-center"><%# DataBinder.Eval(Container.DataItem, "lastName") %> </td>
                                        <td class="text-center"><%# DataBinder.Eval(Container.DataItem, "phoneNumber") %> </td>
                                        <td class="text-center">
                                            <asp:Button ID="DeleteBtn" runat="server" Text="Delete" class="btn btn-danger fw-bold text-dark" OnCommand="DeleteBtn_Command"
                                                CommandArgument='<%# DataBinder.Eval
                  (Container.DataItem, "id") %>' />
                                        </td>
                                        <td class="text-center">
                                            <asp:Button ID="UpdateBtn" data-bs-toggle="modal" data-bs-target="#staticBackdrop" runat="server" Text="Update"
                                                class="btn btn-warning fw-bold text-dark" OnCommand="UpdateBtn_Command"
                                                CommandArgument='<%# DataBinder.Eval
                  (Container.DataItem, "id") + "," + DataBinder.Eval(Container.DataItem, "firstName") + "," + 
                  DataBinder.Eval(Container.DataItem, "lastName") + "," + DataBinder.Eval(Container.DataItem, "phoneNumber")
                  + "," + DataBinder.Eval(Container.DataItem, "emailAddress") + "," + DataBinder.Eval(Container.DataItem, "age")%>' />
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
        <asp:SqlDataSource ID="PLSQLDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString2 %>" ProviderName="<%$ ConnectionStrings:ConnectionString2.ProviderName %>" SelectCommand="SELECT * FROM &quot;CONTACTS&quot; ORDER BY &quot;ID&quot;"></asp:SqlDataSource>

    </form>
</body>
</html>
