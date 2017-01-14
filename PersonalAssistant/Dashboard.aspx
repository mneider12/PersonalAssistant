<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="PersonalAssistant.Dashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Dashboard</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Hyperlink runat="server" Text="Settings" NavigateUrl="~/Configuration.aspx" />
        <asp:HyperLink runat="server" Text="Finances" NavigateUrl="~/Finances/Finances.aspx" />
    </div>
    </form>
</body>
</html>
