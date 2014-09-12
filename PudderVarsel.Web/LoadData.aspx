<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoadData.aspx.cs" Inherits="PudderVarsel.Web.LoadData" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Puddervarsel data</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button runat="server" Text="Load data" ID="LoadDataButton" OnClick="LoadDataButton_Click"/>
        <asp:Button runat="server" Text="Cancel"/>
    </div>
    </form>
</body>
</html>
