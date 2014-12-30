<%@ Page Title="Dado Validators Demo" Language="C#" MasterPageFile="~/master/main.master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Web.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphStyle" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="server">

	<h1>Dado Validators Demo</h1>

	<asp:Panel runat="server" DefaultButton="btnSubmit">

		<table cellpadding="0" cellspacing="0" border="0" width="100%">
			<colgroup>
				<col width="20%" />
				<col width="30%" />
				<col width="20%" />
				<col width="30%" />
			</colgroup>
			<tr>
				<th valign="top" align="right">Required Field Validator:</th>
				<td valign="top">
					<asp:TextBox ID="txtRequired" runat="server" />
					<Dado:RequiredFieldValidator runat="server" ControlToValidate="txtRequired" ValidationGroup="vlgSubmit" />
				</td>
				<th valign="top" align="right">Length Validator:</th>
				<td valign="top">
					<asp:TextBox ID="txtLength" runat="server" Text="Can you deal with that?" />
					<Dado:LengthValidator runat="server" ControlToValidate="txtLength" ValidationGroup="vlgSubmit" MinimumLength="23" MaximumLength="23" />
				</td>
			</tr>
			<tr>
				<th valign="top" align="right">Email Validator:</th>
				<td valign="top">
					<asp:TextBox ID="txtEmail" runat="server" Text="test@gmail.com" />
					<Dado:EmailValidator runat="server" ControlToValidate="txtEmail" ValidationGroup="vlgSubmit" />
				</td>
				<th valign="top" align="right">Zip Code Validator:</th>
				<td valign="top">
					<asp:TextBox ID="txtZipCode" runat="server" Text="90210" />
					<Dado:ZipCodeValidator runat="server" ControlToValidate="txtZipCode" ValidationGroup="vlgSubmit" />
				</td>
			</tr>
			<tr>
				<th valign="top" align="right">Timespan Validator:</th>
				<td valign="top">
					<asp:TextBox ID="txtTime" runat="server" Text="5:00 pm" />
					<Dado:TimeValidator runat="server" ControlToValidate="txtTime" ValidationGroup="vlgSubmit" />
				</td>
				<th valign="top" align="right">RegEx Validator:</th>
				<td valign="top">
					<asp:TextBox ID="txtRegEx" runat="server" Text="404" />
					<Dado:RegularExpressionValidator runat="server" ControlToValidate="txtRegEx" ValidationGroup="vlgSubmit" ValidationExpression="[^A-z]*" ErrorMessage="Please enter only numeric values." />
				</td>
			</tr>
			<tr>
				<th valign="top" align="right">Phone Validator:</th>
				<td valign="top">
					<asp:TextBox ID="txtPhoneNumber" runat="server" Text="(555) 896-4571 x345" />
					<Dado:PhoneValidator ID="vldPhoneNumber" runat="server" ControlToValidate="txtPhoneNumber" ValidationGroup="vlgSubmit" AllowExtension="true" />
				</td>
				<th valign="top" align="right"></th>
				<td valign="top"></td>
			</tr>
			<tr>
				<th valign="top" align="right">File Type Validator:</th>
				<td valign="top">
					<asp:FileUpload ID="fupFileType" runat="server" />
					<Dado:FileTypeValidator runat="server" ControlToValidate="fupFileType" ValidationGroup="vlgSubmit" FileExtensions=".txt, .jpg, .jpeg" Operator="Positive" />
				</td>
				<th valign="top" align="right">Image Validator:</th>
				<td valign="top">
					<asp:FileUpload ID="fupImage" runat="server" />
					<Dado:ImageValidator runat="server" ControlToValidate="fupImage" ValidationGroup="vlgSubmit" ExactWidth="200" ExactHeight="250" />
					<Dado:RequiredFieldValidator runat="server" ControlToValidate="fupImage" ValidationGroup="vlgSubmit" />
				</td>
			</tr>
			<tr>
				<th valign="top" align="right">Int32 Validator:</th>
				<td valign="top">
					<asp:TextBox ID="txtInt32" runat="server" />
					<Dado:TypeValidator runat="server" ControlToValidate="txtInt32" ValidationGroup="vlgSubmit" Type="Int32" />
				</td>
				<th valign="top" align="right">SByte Validator:</th>
				<td valign="top">
					<asp:TextBox ID="txtSByte" runat="server" />
					<Dado:TypeValidator runat="server" ControlToValidate="txtSByte" ValidationGroup="vlgSubmit" Type="SByte" />
				</td>
			</tr>
			<tr>
				<th valign="top" align="right">DateTime Validator:</th>
				<td valign="top">
					<asp:TextBox ID="txtDateTime" runat="server" />
					<Dado:CompareValidator runat="server" ControlToValidate="txtDateTime" ValidationGroup="vlgSubmit" Type="Date" Operator="DataTypeCheck" ErrorMessage="Please enter a valid date." />
				</td>
				<th valign="top" align="right">URI Validator:</th>
				<td valign="top">
					<asp:TextBox ID="txtUri" runat="server" />
					<Dado:UriValidator runat="server" ControlToValidate="txtUri" ValidationGroup="vlgSubmit" Kind="Absolute" TryToFix="true" />
				</td>
			</tr>
			<tr>
				<th valign="top" align="right">Checkbox List:</th>
				<td valign="top">
					<asp:CheckBoxList ID="cblCheckBoxList" runat="server" RepeatLayout="Flow">
						<asp:ListItem Text="Check Box (empty)" Value="" />
						<asp:ListItem Text="Check Box 1" Value="1" />
						<asp:ListItem Text="Check Box 2" Value="2" />
						<asp:ListItem Text="Check Box 3" Value="3" />
					</asp:CheckBoxList>
					<Dado:RequiredFieldValidator runat="server" ControlToValidate="cblCheckBoxList" ValidationGroup="vlgSubmit" />
				</td>
				<th valign="top" align="right">Radio Button List:</th>
				<td valign="top">
					<asp:RadioButtonList ID="rblRadioButtonList" runat="server" RepeatLayout="Flow">
						<asp:ListItem Text="Radio Button (empty)" Value="" />
						<asp:ListItem Text="Radio Button 1" Value="1" />
						<asp:ListItem Text="Radio Button 2" Value="2" />
						<asp:ListItem Text="Radio Button 3" Value="3" />
					</asp:RadioButtonList>
					<Dado:RequiredFieldValidator runat="server" ControlToValidate="rblRadioButtonList" ValidationGroup="vlgSubmit" />
				</td>
			</tr>
			<tr>
				<th valign="top" align="right">CheckBox Validator:</th>
				<td valign="top">
					<asp:CheckBox ID="ckbCheckBox" runat="server" Text="I agree with the policies and terms." />
					<Dado:RequiredFieldValidator runat="server" ControlToValidate="ckbCheckBox" ValidationGroup="vlgSubmit" />
				</td>
				<td colspan="2"></td>
			</tr>
		</table>

		<asp:Button ID="btnSubmit" runat="server" ValidationGroup="vlgSubmit" Text="Submit" />

		<asp:Literal ID="litTest" runat="server" />

	</asp:Panel>

</asp:Content>