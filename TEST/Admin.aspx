<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="Elearning.Admin" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Admin Dashboard</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="container mt-4">

        <!-- First Three Cards -->
        <div class="row mb-4">
            <div class="col-md-4">
                <div class="card text-center bg-primary text-white">
                    <div class="card-body">
                        <h5>Total Courses</h5>
                        <asp:Label ID="lblTotalCourses" runat="server" Font-Bold="true" Font-Size="XX-Large"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="card text-center bg-success text-white">
                    <div class="card-body">
                        <h5>Total Subcourses</h5>
                        <asp:Label ID="lblTotalSubcourses" runat="server" Font-Bold="true" Font-Size="XX-Large"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="card text-center bg-warning text-white">
                    <div class="card-body">
                        <h5>Total Subscriptions</h5>
                        <asp:Label ID="lblTotalSubscriptions" runat="server" Font-Bold="true" Font-Size="XX-Large"></asp:Label>
                    </div>
                </div>
            </div>
        </div>

        <!-- Next Three Cards -->
        <div class="row mb-4">
            <div class="col-md-4">
                <div class="card text-center bg-info text-white">
                    <div class="card-body">
                        <h5>Active Users</h5>
                        <asp:Label ID="lblActiveUsers" runat="server" Font-Bold="true" Font-Size="XX-Large"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="card text-center bg-danger text-white">
                    <div class="card-body">
                        <h5>Inactive Users</h5>
                        <asp:Label ID="lblInactiveUsers" runat="server" Font-Bold="true" Font-Size="XX-Large"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="card text-center bg-dark text-white">
                    <div class="card-body">
                        <h5>Sold Courses</h5>
                        <asp:Label ID="lblSoldCourses" runat="server" Font-Bold="true" Font-Size="XX-Large"></asp:Label>
                    </div>
                </div>
            </div>
        </div>

        <!-- Course Summary Section -->
        <div class="row mb-4">
            <div class="col-md-4">
                <asp:TextBox ID="txtCourse" runat="server" CssClass="form-control" Placeholder="Enter Course Name"></asp:TextBox>
            </div>
            <div class="col-md-4">
                <asp:DropDownList ID="ddlCourses" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlCourses_SelectedIndexChanged"></asp:DropDownList>
            </div>
        </div>

        <div class="row mb-4">
            <div class="col-md-12">
                <asp:GridView ID="gvSubcourseSummary" runat="server" CssClass="table table-bordered table-striped"></asp:GridView>
            </div>
        </div>

        <!-- New Courses Card -->
        <div class="row mb-4">
            <div class="col-md-12">
                <div class="card">
                    <div class="card-header bg-secondary text-white">
                        New Courses Launched
                    </div>
                    <div class="card-body">
                        <asp:BulletedList ID="blNewCourses" runat="server"></asp:BulletedList>
                    </div>
                </div>
            </div>
        </div>

        <!-- Transaction Bar Graph -->
        <div class="row mb-4">
            <div class="col-md-3">
                <asp:DropDownList ID="ddlYears" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlYears_SelectedIndexChanged"></asp:DropDownList>
            </div>
            <div class="col-md-9">
                <canvas id="transactionChart"></canvas>
            </div>
        </div>

    </div>
    </form>
</body>
</html>

