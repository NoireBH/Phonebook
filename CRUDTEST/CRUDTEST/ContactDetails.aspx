<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContactDetails.aspx.cs" Inherits="CRUDTEST.ContactDetails" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Contact Details</title>
    <link href="Styles/style.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap/5.3.3/css/bootstrap.min.css" integrity="sha512-jnSuA4Ss2PkkikSOLtYs8BlYIeeIK1h99ty4YfvRPAlzr377vr3CXDb7sb7eEEBYjDtcYj+AjBH3FLv5uSJuXg==" crossorigin="anonymous" referrerpolicy="no-referrer" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.2/css/all.min.css" integrity="sha512-SnH5WK+bZxgPHs44uWIX+LLJAJ9/2PkPKZ5QiAj6Ta86w+fsb2TkcmfRyVX3pBnMFcV7oQPJkl9QevSCWr3W6A==" crossorigin="anonymous" referrerpolicy="no-referrer" />
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
                        <asp:ScriptManager ID="ScriptManager" runat="server">
                        </asp:ScriptManager>
                        <asp:UpdatePanel ID="ContactMainInfoUpdatePanel" ChildrenAsTriggers="true" runat="server">
                            <ContentTemplate>
                                <h1>
                                    <asp:Label ID="lblContactName" runat="server"></asp:Label>
                                </h1>
                                <p>
                                    <span>Age: </span>
                                    <asp:Label ID="lblContactAge" runat="server"></asp:Label>
                                </p>
                                <p>
                                    <span>Email: </span>
                                    <asp:Label ID="lblEmailAddress" runat="server"></asp:Label>
                                </p>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div class="container-fluid mb-2">
                            <div class="row  d-flex flex-column align-items-center">
                                <div class="mb-1">
                                    <h1 class="phone-number-h2">Phone numbers:
                                    </h1>
                                </div>                               
                            </div>
                        </div>
                        <asp:UpdatePanel ID="PhoneNumbersUpdatePanel" runat="server">
                            <ContentTemplate>
                                <asp:Repeater ID="PhoneRepeater" runat="server">
                                    <ItemTemplate>
                                        <div class="container phone-number-container d-flex justify-content-center align-items-baseline gap-3 fw-bold"
                                            style="max-width: 300px">
                                            <%# DataBinder.Eval(Container.DataItem, "Number") %>                                          
                                        </div>
                                        <br />
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:SqlDataSource ID="PhoneNumbers" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString2 %>" ProviderName="<%$ ConnectionStrings:ConnectionString2.ProviderName %>" SelectCommand="SELECT &quot;PHONE_NUMBER&quot;, &quot;ID&quot; FROM &quot;PHONENUMBERS&quot; WHERE (&quot;CONTACT_ID&quot; = :CONTACT_ID) ORDER BY &quot;ID&quot;">
                            <SelectParameters>
                                <asp:QueryStringParameter DefaultValue="null" Name="CONTACT_ID" QueryStringField="id" Type="Decimal" />
                            </SelectParameters>
                        </asp:SqlDataSource>

                    </div>
                </div>
            </div>

        </div>

        <asp:Panel ID="PhoneNumberPanel" runat="server" Height="361px">
            <asp:SqlDataSource ID="ContactDetailsDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>" SelectCommand="SELECT &quot;FIRST_NAME&quot;, &quot;LAST_NAME&quot;, &quot;PROFILE_PICTURE&quot; FROM &quot;CONTACTS&quot; WHERE (&quot;ID&quot; = :ID)">
                <SelectParameters>
                    <asp:QueryStringParameter DefaultValue="null" Name="ID" QueryStringField="id" Type="Decimal" />
                </SelectParameters>
            </asp:SqlDataSource>
        </asp:Panel>
    </form>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.7.1/jquery.min.js" integrity="sha512-v2CJ7UaYy4JwqLDIrZUI/4hqeoQieOmAZNXBeQyjo21dadnwR+8ZaIJVT8EE2iyI61OV8e6M8PP2/4hpQINQ/g==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap/5.3.3/js/bootstrap.min.js" integrity="sha512-ykZ1QQr0Jy/4ZkvKuqWn4iF3lqPZyij9iRv6sGqLRdTPkY69YX6+7wvVGmsdBbiIfN/8OdsI7HABjvEok6ZopQ==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.2/js/all.min.js" integrity="sha512-u3fPA7V8qQmhBPNT5quvaXVa1mnnLSXUep5PS1qo5NRzHwG19aHmNJnj1Q8hpA/nBWZtZD4r4AX6YOt5ynLN2g==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
</body>
</html>
