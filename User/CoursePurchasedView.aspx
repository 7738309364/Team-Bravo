<%@ Page Title="" Language="C#" MasterPageFile="~/User/User.Master" AutoEventWireup="true" CodeBehind="CoursePurchasedView.aspx.cs" Inherits="eLearning.User.CoursePurchasedView" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.11.8/dist/umd/popper.min.js" integrity="sha384-I7E8VVD/ismYTF4hNIPjVp/Zjvgyol6VFvRkX/vR+Vc4jQkC+hVqc2pM8ODewa9r" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.7/dist/js/bootstrap.min.js" integrity="sha384-7qAoOXltbVP82dhxHAUje59V5r2YsVfBafyUDxEdApLPmcdhBPg1DKg1ERo0BZlK" crossorigin="anonymous"></script>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.7/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-Xm+6iOcoMl0A1uXlZpFffzqpF4p6QEtTeRWG+K7tvzOd5ESo9Wy2QO7REoL4CWRB" crossorigin="anonymous" />

    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:GridView ID="GridViewCourses" runat="server" AutoGenerateColumns="False" Width="913px" OnRowCommand="GridViewCourses_RowCommand" >
    <Columns>
        <asp:BoundField DataField="RowNumber" HeaderText="No." />
        <asp:BoundField DataField="SubCourse_Name" HeaderText="Sub Course" />
        <asp:TemplateField HeaderText="Action">
            <ItemTemplate>
                <asp:Button ID="btnPlay" runat="server" Text="Play" 
                    CommandName="Play"
                    CommandArgument='<%# Eval("Subcourse_Id") + "|" + Eval("Course_Id") %>' 
                     CssClass="btn btn-success" />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>

</asp:Content>
