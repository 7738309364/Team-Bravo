<%@ Page Language="C#" AutoEventWireup="true" Codefile="User.aspx.cs" Inherits="Elearning.User" %>


<!DOCTYPE html>
<html>
<head runat="server">
    <title>User Dashboard</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="container mt-4">
        <h2 class="text-center mb-4">User Dashboard</h2>

        <!-- Three Cards -->
        <div class="row text-center mb-4">
            <div class="col-md-4">
                <div class="card bg-primary text-white">
                    <div class="card-body">
                        <h4>My Courses</h4>
                        <asp:Literal ID="ltTotalCourses" runat="server"></asp:Literal>
                    </div>
                </div>
            </div>

            <div class="col-md-4">
                <div class="card bg-success text-white">
                    <div class="card-body">
                        <h4>Completed Courses</h4>
                        <asp:Label ID="lblCompleted" runat="server" CssClass="fs-4"></asp:Label>
                    </div>
                </div>
            </div>

            <div class="col-md-4">
                <div class="card bg-danger text-white">
                    <div class="card-body">
                        <h4>Incomplete Courses</h4>
                        <asp:Label ID="lblIncomplete" runat="server" CssClass="fs-4"></asp:Label>
                    </div>
                </div>
            </div>
        </div>

        <!-- Subcourses with Stacked Bars -->
        <div class="card mb-4">
            <div class="card-header bg-secondary text-white">
                <h5 class="mb-0">Subcourses Progress</h5>
            </div>
            <div class="card-body">
                <asp:Repeater ID="rptSubcourses" runat="server">
                    <ItemTemplate>
                        <div class="mb-3">
                            <strong><%# Eval("SubcourseName") %></strong>
                            <div class="progress">
                                <div class="progress-bar bg-success" role="progressbar" style='width: <%# Eval("CompletedPercentage") %>%' aria-valuemin="0" aria-valuemax="100">
                                    <%# Eval("CompletedPercentage") %>%
                                </div>
                                <div class="progress-bar bg-danger" role="progressbar" style='width: <%# 100 - Convert.ToInt32(Eval("CompletedPercentage")) %>%'>
                                    <%# 100 - Convert.ToInt32(Eval("CompletedPercentage")) %>%
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>

        <!-- Pie Chart -->
        <div class="row justify-content-center">
            <div class="col-md-6">
                <canvas id="pieChart"></canvas>
            </div>
        </div>
    </div>
    </form>

    <script>
        function renderPieChart(completed, remaining) {
            var ctx = document.getElementById('pieChart').getContext('2d');
            new Chart(ctx, {
                type: 'pie',
                data: {
                    labels: ['Completed', 'Remaining'],
                    datasets: [{
                        data: [completed, remaining],
                        backgroundColor: ['#198754', '#dc3545']
                    }]
                }
            });
        }
    </script>
</body>
</html>
