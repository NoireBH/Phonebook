<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContactCreateAndUpdateFormUserControl.ascx.cs" Inherits="CRUDTEST.WebUserControl1" %>
<div class="text-center form-container container-fluid">
    <h3 runat="server" id="txtCreateContract">Create a contact</h3>

    <asp:ScriptManager ID="FormScriptManager" runat="server">
    </asp:ScriptManager>

    <asp:UpdatePanel ID="FormUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="container-fluid field-form d-flex flex-column justify-content-center gap-2 mb-2" style="max-width: 500px;">
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
                    <asp:RequiredFieldValidator ControlToValidate="txtPhoneNumber" runat="server" />
                </div>

            </div>
            <div class="d-flex justify-content-center gap-2">
                <asp:Button ID="submitBtn" runat="server" Text="Submit" class="btn btn-success fw-bold" OnClick="Submit_Click" />
                <asp:Button ID="CancelUpdBtn" runat="server" Text="Cancel" class="d-none fw-bold text-dark" OnClick="CancelUpdBtn_Click" />
            </div>

            <asp:HiddenField ID="BtnHiddenFIeld" Value="1" runat="server" OnValueChanged="BtnHiddenFIeld_ValueChanged" />
            <asp:HiddenField ID="HiddenIdField" runat="server" OnValueChanged="HiddenIdField_ValueChanged" />
        </ContentTemplate>

    </asp:UpdatePanel>

</div>
