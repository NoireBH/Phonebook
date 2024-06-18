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
                        <asp:ScriptManager ID="ScriptManager" runat="server">
                        </asp:ScriptManager>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
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
                                <div class="contact-info-btn-container">
                                    <asp:Button ID="UpdateBtn" data-bs-toggle="modal" data-bs-target="#updateModal" runat="server" Text="Update"
                                        class="btn btn-warning fw-bold text-dark" OnClick="UpdateBtn_Click" />
                                    <asp:Button ID="DeleteBtn" runat="server" Text="Delete" class="btn btn-danger fw-bold text-dark" OnClick="DeleteBtn_Click" />
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <div class="modal fade" id="updateModal" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
                            <div class="modal-dialog">
                                <div class="modal-content">
                                    <div class="modal-body">
                                        <div class="text-center form-container container-fluid">
                                            <asp:UpdatePanel ID="ContactInfoUpdatePanel" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <div class="container-fluid field-form d-flex  flex-column justify-content-center gap-2 mb-3" style="max-width: 500px;">
                                                        <div class="form-group d-flex flex-column">
                                                            <asp:Label ID="lblFirstName" CssClass="fw-bold" runat="server" Text="First Name:"></asp:Label>
                                                            <p class="required-field">Field is required*</p>
                                                            <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                        <div class="form-group d-flex flex-column">
                                                            <asp:Label ID="lblLastName" CssClass="fw-bold" runat="server" Text="Last Name:"></asp:Label>
                                                            <p class="required-field">Field is required*</p>
                                                            <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                        <div class="form-group d-flex flex-column">
                                                            <asp:Label ID="lblFormAge" CssClass="fw-bold" runat="server" Text="Age:"></asp:Label>
                                                            <asp:TextBox ID="txtAge" runat="server" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                        <div class="form-group d-flex flex-column">
                                                            <asp:Label ID="lblFormEmailAddress" CssClass="fw-bold" runat="server" Text="Email Address:"></asp:Label>
                                                            <asp:TextBox ID="txtFormEmailAddress" runat="server" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                        <div class="form-group d-flex flex-column">
                                                            <asp:Label ID="lblProfilePicture" runat="server" CssClass="fw-bold mb-3" Text="Profile Picture:"></asp:Label>
                                                            <asp:FileUpload CssClass="text-center" ID="ImageUpload" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="d-flex justify-content-center gap-2">
                                                        <asp:Button ID="SubmitContactInfo" runat="server" Text="Submit" class="btn btn-success fw-bold" OnClick="SubmitContactInfo_Click" data-bs-dismiss="modal" />
                                                        <asp:Button ID="CancelUpdateBtn" runat="server" type="button" class="btn btn-secondary fw-bold" OnClick="CancelBtn_Click" Text="Cancel" data-bs-dismiss="modal"></asp:Button>
                                                    </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="SubmitContactInfo" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="container-fluid mb-2">
                            <div class="row  d-flex flex-column align-items-center">
                                <div>
                                    <h2 class="phone-number-h2">Phone Numbers:
                                    </h2>
                                </div>
                                <div>
                                    <button type="button" class="btn btn-success fw-bold" data-bs-toggle="modal" data-bs-target="#staticBackdrop">
                                        Add a number</button>
                                    <div class="modal fade" id="staticBackdrop" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
                                        <div class="modal-dialog">
                                            <div class="modal-content">
                                                <div class="modal-body">
                                                    <div class="text-center form-container container-fluid">
                                                        <asp:UpdatePanel ID="PhoneFormUpdatePanel" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <div class="container-fluid field-form d-flex  flex-column justify-content-center gap-2 mb-3" style="max-width: 500px;">
                                                                    <div class="form-group d-flex flex-column">
                                                                        <asp:Label ID="lblPhoneNumber" CssClass="fw-bold" runat="server" Text="Phone number:"></asp:Label>
                                                                        <p class="required-field">Field is required*</p>
                                                                        <asp:TextBox ID="txtPhoneNumber" runat="server" CssClass="form-control"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ControlToValidate="txtPhoneNumber" runat="server" />
                                                                    </div>
                                                                </div>
                                                                <div class="d-flex justify-content-center gap-2">
                                                                    <asp:Button ID="submitBtn" runat="server" Text="Submit" class="btn btn-success fw-bold" OnClick="submitBtn_Click" data-bs-dismiss="modal" />
                                                                    <asp:Button ID="CancelBtn" runat="server" type="button" class="btn btn-secondary fw-bold" OnClick="CancelBtn_Click" Text="Cancel" data-bs-dismiss="modal"></asp:Button>
                                                                </div>
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
                                </div>
                            </div>
                        </div>
                        <asp:UpdatePanel ID="PhoneNumbersUpdatePanel" runat="server">
                            <ContentTemplate>
                                <asp:Repeater ID="PhoneRepeater" runat="server" DataSourceID="PhoneNumbers">
                                    <ItemTemplate>
                                        <div class="container phone-number-container d-flex justify-content-center align-items-baseline gap-2 fw-bold" style="max-width: 300px">
                                            <%# DataBinder.Eval(Container.DataItem, "PHONE_NUMBER") %>
                                            <asp:Button ID="RemoveNumberBtn" CssClass="btn-no-styling remove-number-btn" runat="server" Text="-"
                                                OnCommand="RemoveNumberBtn_Command" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' />
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
