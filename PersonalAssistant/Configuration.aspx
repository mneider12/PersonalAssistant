<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Configuration.aspx.cs" Inherits="PersonalAssistant.Configuration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Configuration</title>
    <link rel="stylesheet" href="styles/Default.css" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:Hyperlink runat="server" Text="Home" NavigateUrl="~/Dashboard.aspx" />
        <div>
            <label for="numId">ID:</label>
            <input type="number" runat="server" id="numId" />
            <label for="txtName">Name:</label>
            <input type="text" runat="server" id="txtName"/>
        </div>
        <div>
            <button type="submit" runat="server" id="btnSave">Save</button>
        </div>
    </form>
</body>
</html>
