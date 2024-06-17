<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContactDetails.aspx.cs" Inherits="CRUDTEST.ContactDetails" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Contact Details</title>
    <link href="Styles/style.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap/5.3.3/css/bootstrap.min.css" integrity="sha512-jnSuA4Ss2PkkikSOLtYs8BlYIeeIK1h99ty4YfvRPAlzr377vr3CXDb7sb7eEEBYjDtcYj+AjBH3FLv5uSJuXg==" crossorigin="anonymous" referrerpolicy="no-referrer" />
</head>
<body>
    <form id="ContactDetails" runat="server">
        <div class="container-fluid mt-5 contact-details-container">
            <div class="row">
                <div class="back-to-contacts container-fluid text-center mb-2">
                    <asp:Button ID="backToContactsBtn" CssClass="btn btn-info w-100" Text="Back to contacts" runat="server" OnClick="backToContactsBtn_Click" />
                </div>
                <div class="contact-img-container container-fluid text-center">
                    <asp:Image ID="contactImg" CssClass="contact-img" runat="server" />
                </div>
                <div class="d-flex flex-column align-content-center text-center">
                    <div class="container-fluid contact-main-info">
                        <h1>
                            <asp:Label ID="lblContactName" runat="server"></asp:Label>
                        </h1>
                        <p>
                            <asp:Label ID="lblContactAge" runat="server"></asp:Label>
                        </p>
                        <p>
                            <asp:Label ID="lblEmailAddress" runat="server"></asp:Label>
                        </p>
                        <div>
                            <%--<asp:Button ID="DeleteBtn" runat="server" Text="Delete" class="btn btn-danger fw-bold text-dark" OnCommand="DeleteBtn_Command"
                CommandArgument='<%# DataBinder.Eval
            (Container.DataItem, "id") %>' />
            <asp:Button ID="UpdateBtn" data-bs-toggle="modal" data-bs-target="#staticBackdrop" runat="server" Text="Update"
                class="btn btn-warning fw-bold text-dark" OnCommand="UpdateBtn_Command"
                CommandArgument='<%# DataBinder.Eval
            (Container.DataItem, "id") + "," + DataBinder.Eval(Container.DataItem, "firstName") + "," + 
            DataBinder.Eval(Container.DataItem, "lastName") + "," + DataBinder.Eval(Container.DataItem, "phoneNumber")
            + "," + DataBinder.Eval(Container.DataItem, "emailAddress") + "," + DataBinder.Eval(Container.DataItem, "age")%>' />--%>
                        </div>
                        <asp:Repeater ID="PhoneRepeater" runat="server" DataSourceID="PhoneNumbers">
                            <HeaderTemplate>
                                <h2 class="phone-number-h2">Phone Numbers:
                                </h2>
                                <asp:Button ID="AddNumberBtn" runat="server" CssClass="btn-no-styling add-number-btn" Text="+" />
                            </HeaderTemplate>

                            <ItemTemplate>
                                <div class="container phone-number-container d-flex justify-content-center align-items-baseline gap-2 fw-bold" style="max-width: 300px">
                                    <%# DataBinder.Eval(Container.DataItem, "PHONE_NUMBER") %>
                                    <asp:Button ID="RemoveNumberBtn" CssClass="btn-no-styling remove-number-btn" runat="server" Text="-" OnCommand="RemoveNumberBtn_Command" />
                                </div>

                                <br />
                            </ItemTemplate>
                        </asp:Repeater>
                        <asp:SqlDataSource ID="PhoneNumbers" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>" SelectCommand="SELECT &quot;PHONE_NUMBER&quot; FROM &quot;PHONENUMBERS&quot; WHERE (&quot;CONTACT_ID&quot; = :CONTACT_ID)">
                            <SelectParameters>
                                <asp:QueryStringParameter DefaultValue="null" Name="CONTACT_ID" QueryStringField="id" Type="Decimal" />
                            </SelectParameters>
                        </asp:SqlDataSource>

                    </div>
                </div>
            </div>

        </div>

        <asp:Panel ID="Panel1" runat="server" Height="361px">
            <asp:SqlDataSource ID="ContactDetailsDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>" SelectCommand="SELECT &quot;FIRST_NAME&quot;, &quot;LAST_NAME&quot;, &quot;PROFILE_PICTURE&quot; FROM &quot;CONTACTS&quot; WHERE (&quot;ID&quot; = :ID)" OnSelecting="ContactDetailsDataSource_Selecting">
                <SelectParameters>
                    <asp:QueryStringParameter DefaultValue="null" Name="ID" QueryStringField="id" Type="Decimal" />
                </SelectParameters>
            </asp:SqlDataSource>
        </asp:Panel>
    </form>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.7.1/jquery.min.js" integrity="sha512-v2CJ7UaYy4JwqLDIrZUI/4hqeoQieOmAZNXBeQyjo21dadnwR+8ZaIJVT8EE2iyI61OV8e6M8PP2/4hpQINQ/g==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap/5.3.3/js/bootstrap.min.js" integrity="sha512-ykZ1QQr0Jy/4ZkvKuqWn4iF3lqPZyij9iRv6sGqLRdTPkY69YX6+7wvVGmsdBbiIfN/8OdsI7HABjvEok6ZopQ==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
</body>
</html>
