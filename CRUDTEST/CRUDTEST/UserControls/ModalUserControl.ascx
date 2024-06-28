<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModalUserControl.ascx.cs" Inherits="CRUDTEST.UserControls.ModalUserControl" %>

<div class="phonebook-container container-fluid text-center mb-2">
    <asp:UpdatePanel ID="AddContactBtnUpdatePanel" runat="server">
        <ContentTemplate>
            <asp:Button ID="AddContactBtn" runat="server" Text="Add a contact" OnClick="AddContactBtn_Click"
                class="btn btn-success fw-bold" />
        </ContentTemplate>
        <div class="modal fade" id="contactModal">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-body">
                        <div class="text-center form-container container-fluid">
                            <asp:UpdatePanel ID="FormUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                                <ContentTemplate>
                                    <div class="container-fluid field-form d-flex  flex-column justify-content-center gap-2 mb-3">
                                        <div runat="server" id="formAlert" class="alert alert-danger fw-bolder text-black" visible="false" role="alert">
                                            Please fill out all required fields!
                                        </div>
                                        <div class="form-group d-flex flex-column">
                                            <asp:Label ID="lblFirstName" CssClass="fw-bold" runat="server" Text="First Name:"></asp:Label>
                                            <p class="required-field">Field is required*</p>
                                            <input type="text" id="textFirstName" runat="server" class="form-control" />
                                        </div>
                                        <div class="form-group d-flex flex-column">
                                            <asp:Label ID="lblLastName" CssClass="fw-bold" runat="server" Text="Last Name:"></asp:Label>
                                            <p class="required-field">Field is required*</p>
                                            <input type="text" id="textLastName" runat="server" class="form-control" />
                                        </div>
                                        <div class="form-group d-flex flex-column">
                                            <asp:Label ID="lblEmailAddress" runat="server" CssClass="fw-bold mb-3" Text="Email Address:"></asp:Label>
                                            <input id="textEmailAddress" runat="server" class="form-control" type="email" />
                                        </div>
                                        <div class="form-group d-flex flex-column">
                                            <asp:Label ID="lblAge" runat="server" CssClass="fw-bold mb-3" Text="Age:"></asp:Label>
                                            <input id="textAge" runat="server" class="form-control" type="number" min="1" max="100" />
                                        </div>
                                        <asp:UpdatePanel ID="PhoneNumUpdatePanel" runat="server">
                                            <ContentTemplate>
                                                <div class="form-group d-flex flex-column phone-numbers-container">
                                                    <h4 class="fw-bold mb-3">Phone numbers:</h4>
                                                    <input runat="server" class="form-control" title="numbers" id="textPhoneNumber" type="text" pattern="\d*" maxlength="15" />
                                                    <div>
                                                        <asp:Button ID="AddOrEditPhoneNumBtn" CssClass="btn btn-success mb-2 mt-2" runat="server"
                                                            Text="Save" OnCommand="AddOrEditPhoneNumBtn_Command" />
                                                    </div>
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
                                                                <asp:LinkButton ClientIDMode="AutoID" ID="RemoveNumberBtn" class="fw-bold text-dark" runat="server"
                                                                    OnCommand="RemoveNumberBtn_Command"
                                                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Number") %>'>
                                                            <i class="fa-solid fa-trash-can" style="color: #ff0000;"></i> 
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ClientIDMode="AutoID" ID="UpdatePhoneNumBtn" runat="server" OnCommand="UpdatePhoneNumBtn_Command"
                                                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Number") + "," + DataBinder.Eval(Container.DataItem, "Id") %>'>
                                                            <i class="fa-solid fa-pen-to-square" style="color: #FFD43B;"></i> 
                                                                </asp:LinkButton>
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
                                        <asp:Button ID="submitBtn" runat="server" Text="Submit" class="btn btn-success fw-bold" OnClientClick="return validateForm()" OnClick="Submit_Click" />
                                        <asp:Button ID="CancelUpdBtn" runat="server" type="button" class="btn btn-secondary" formnovalidate="" OnClick="CancelUpdBtn_Click" OnClientClick="hideContactModal()" Text="Cancel"></asp:Button>
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


    </asp:UpdatePanel>

    <h1 class="text-center">My Contacts:
    </h1>
</div>

