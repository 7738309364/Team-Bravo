<%@ Page Title="" Language="C#" MasterPageFile="~/User/User.Master" AutoEventWireup="true" CodeBehind="VideoPlayer.aspx.cs" Inherits="eLearning.User.VideoPlayer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.11.8/dist/umd/popper.min.js" integrity="sha384-I7E8VVD/ismYTF4hNIPjVp/Zjvgyol6VFvRkX/vR+Vc4jQkC+hVqc2pM8ODewa9r" crossorigin="anonymous"></script>
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.7/dist/js/bootstrap.min.js" integrity="sha384-7qAoOXltbVP82dhxHAUje59V5r2YsVfBafyUDxEdApLPmcdhBPg1DKg1ERo0BZlK" crossorigin="anonymous"></script>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.7/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-Xm+6iOcoMl0A1uXlZpFffzqpF4p6QEtTeRWG+K7tvzOd5ESo9Wy2QO7REoL4CWRB" crossorigin="anonymous" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div style="width: 100%; display: table;">
    <!-- Top row: video + topics grid -->
    <div style="display: table-row;">
        <div style="display: table-cell; width: 70%; vertical-align: top;">
            <iframe id="videoPlayer"
                src=""
                frameborder="0"
                style="background: #000; width: 107%; height: 446px;"
                allow="autoplay; encrypted-media"
                allowfullscreen>
            </iframe>
        </div>

        <div style="display: table-cell; width: 50%; vertical-align: top; padding-left: 90px;">
            
            <asp:GridView ID="GridViewTopics" runat="server" AutoGenerateColumns="False" OnRowCommand="GridViewTopics_RowCommand" BorderWidth="1px" BorderStyle="Solid" CellPadding="5" Width="265px">
                <Columns>
                    <asp:TemplateField HeaderText="Topic">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkPlay" runat="server" CommandName="PlayVideo"
                                CommandArgument='<%# Eval("Topic_Id")+"|"+Eval("Video_URL")+ "|" + Container.DataItemIndex  %>'
                                Text='<%# Eval("Topic_Name") %>'>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>

<!-- Buttons row -->
<div style="margin-top: 10px;">
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Button ID="btnMCQ" runat="server" Text="MCQ" OnClick="btnMCQ_Click" />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Button ID="btnAssignment" runat="server" Text="Assignment" OnClick="btnAssignment_Click" />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Button ID="btnReview" runat="server" Text="Review" OnClick="btnReview_Click" />
</div>

<!-- Panels row -->
<div style="margin-top: 10px;">
    <asp:Panel ID="PanelMCQ" runat="server" BorderStyle="Solid" BorderWidth="1px" Padding="5" Visible="false">
        <asp:Repeater ID="RepeaterMCQ" runat="server">
            <ItemTemplate>
                <div style="border: 1px solid #ccc; padding: 5px; margin-bottom: 5px;">
                    <b>Q: <%# Eval("Question") %></b><br />
                    A) <%# Eval("Opt1") %><br />
                    B) <%# Eval("Opt2") %><br />
                    C) <%# Eval("Opt3") %><br />
                    D) <%# Eval("Opt4") %><br /><br />
                    <asp:TextBox ID="txtAnswer" runat="server" />
                    <asp:HiddenField ID="hdnCorrect" runat="server" Value='<%# Eval("CorrectAnswer") %>' />
                    <asp:Literal ID="litResult" runat="server" />
                </div>

            </ItemTemplate>
        </asp:Repeater>
        <asp:Button ID="btnSubmit" runat="server" Text="Submit Answers" OnClick="btnSubmit_Click" />
    </asp:Panel>

    <asp:Panel ID="PanelAssignment" runat="server" BorderStyle="Solid" BorderWidth="1px" Padding="5" Visible="false">
        <asp:Label ID="lblAssignmentName" runat="server" Text="Assignment Name"></asp:Label><br />
        <asp:HyperLink ID="lnkView" runat="server" Target="_blank" Text="View"></asp:HyperLink>
        <asp:HyperLink ID="lnkDownload" runat="server" Text="Download" Download="true"></asp:HyperLink><br /><br />
        <asp:FileUpload ID="FileUpload1" runat="server" /><br />
        <asp:Button ID="btnUpload" runat="server" Text="Upload Assignment" OnClick="btnUpload_Click" /><br />
        <asp:Label ID="lblMessage" runat="server"></asp:Label>
    </asp:Panel>

    <asp:Panel ID="PanelReview" runat="server" BorderStyle="Solid" BorderWidth="1px" Padding="5" Visible="false">
        <asp:Label ID="lblRating" runat="server" Text="Rating (1-5):"></asp:Label><br />
        <asp:TextBox ID="RatingId" runat="server"></asp:TextBox><br />
        <asp:Label ID="lblComment" runat="server" Text="Comment:"></asp:Label><br />
        <asp:TextBox ID="CommentId" runat="server" TextMode="MultiLine" Rows="4"></asp:TextBox><br />
        <asp:Button ID="ReviewSubmitbtn" runat="server" Text="Submit" OnClick="ReviewSubmitbtn_Click" />
    </asp:Panel>
</div>

<asp:HiddenField ID="HiddenTopicId" runat="server" />
<asp:HiddenField ID="HiddenAssignmentId" runat="server" />


</asp:Content>

