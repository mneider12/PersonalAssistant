﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Finances.aspx.cs" Inherits="PersonalAssistant.Finances" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Finances</title>
    <link rel="stylesheet" href="styles/Default.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Watch List</h1>
            <!--Add contents of watch list here-->
            <div>
                <table id="tblWatchList" runat="server">
                    <tr>
                        <th>Ticker</th>
                    </tr>
                </table>
            </div>
            <input type="text" id="txtAddToWatchList" runat="server" class="ticker" />
            <asp:Button ID="btnAddToWatchList" runat="server" OnClick="btnAddToWatchList_Click" Text="Add" />
        </div>
    </form>
</body>
</html>