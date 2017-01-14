<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Finances.aspx.cs" Inherits="PersonalAssistant.Finances.Finances" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Finances</title>
    <link rel="stylesheet" href="../styles/Default.css" />
    <script type="text/javascript" src="../scripts/modal.js" ></script>
</head>
<body onload="attachModal('divOrderFormContainer','divOrderFormContent','btnAddOrder','spnCloseOrderForm')">
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
            <div id="divOrder">
                <button id="btnAddOrder"  />
            </div>
            <div id="divOrderFormContainer" class="modal" >
                <div id="divOrderFormContent" class="modal-content" >
                    <span id="spnCloseOrderForm" class="close">&times;</span>
                    <label for="dateOrderDate">Date: </label>
                    <input type="date" id="dateOrderDate" runat="server" />
                    <label for="txtOrderTicker">Ticker: </label>
                    <input type="text" id="txtOrderTicker" runat="server" />
                    <label for="numOrderShares">Shares: </label>
                    <input type="number" id="numOrderShares" runat="server" />
                    <label for="numOrderPrice">Order Price: </label>
                    <input type="number" id="numOrderPrice" runat="server" />
                </div>
            </div>
    </form>
</body>
</html>
