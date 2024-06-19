<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddOrEditPhoneNumberUserControl.ascx.cs" Inherits="CRUDTEST.UserControls.AddOrEditPhoneNumberUserControl" %>

<div class="container-fluid field-form d-flex  flex-column justify-content-center gap-2 mb-3" style="max-width: 500px;">
    <div class="form-group d-flex flex-column">
        <asp:Label ID="lblPhoneNumber" CssClass="fw-bold" runat="server" Text="Phone number:"></asp:Label>
        <p class="required-field">Field is required*</p>
        <asp:TextBox ID="txtPhoneNumber" runat="server" CssClass="form-control"></asp:TextBox>
        <asp:RequiredFieldValidator ControlToValidate="txtPhoneNumber" runat="server" />
    </div>
</div>
<div class="d-flex justify-content-center gap-2">
    <asp:Button ID="submitBtn" runat="server" Text="Submit" class="btn btn-success fw-bold" OnClick="SubmitBtn_Click" data-bs-dismiss="modal" />
    <asp:Button ID="CancelBtn" runat="server" type="button" class="btn btn-secondary fw-bold" OnClick="CancelBtn_Click" Text="Cancel" data-bs-dismiss="modal"></asp:Button>
</div>
