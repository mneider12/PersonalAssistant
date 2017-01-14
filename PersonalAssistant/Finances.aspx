<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Finances.aspx.cs" Inherits="PersonalAssistant.Finances" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Finances</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Watch List</h1>
            <!--Add contents of watch list here-->
            <div>

            </div>
            <input type="text" id="txtAddToWatchList" runat="server" />
            <asp:Button ID="btnAddToWatchList" runat="server" OnClick="btnAddToWatchList_Click" Text="Add" />
        </div>
    </form>
</body>
</html>
