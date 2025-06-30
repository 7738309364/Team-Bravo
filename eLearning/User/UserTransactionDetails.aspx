<%@ Page Title="" Language="C#" MasterPageFile="~/User/User.Master" AutoEventWireup="true" CodeBehind="UserTransactionDetails.aspx.cs" Inherits="eLearning.User.UserTransactionDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <h1 style="text-align:center;text-decoration:underline">Your transaction history</h1>
        <p>User ID :
            <asp:TextBox ID="TextBox1" runat="server" AutoPostBack="True" OnTextChanged="TextBox1_TextChanged"></asp:TextBox>
        </p>
        <p>
            <asp:GridView ID="GridView1" runat="server" Visible="false">
            </asp:GridView>
        </p>

    </div>
</asp:Content>
