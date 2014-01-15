<%@ Page Title="Powder details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PowderDetails.aspx.cs" Inherits="PudderVarsel.Web.PowderDetails" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <hgroup class="title">
        <%--<h1><%: Title %>.</h1>--%>
        <h2>Detailed powder forecast for <%# Location %></h2>
    </hgroup>
    
      <asp:ListView runat="server" ID="powderDetailResult">
        <LayoutTemplate>
          <table cellpadding="2" border="1" runat="server" id="tblProducts">
            <tr id="Tr1" runat="server">
              <th id="Th2" runat="server">Date</th>
              <th id="Th3" runat="server">Percipitation</th>
            </tr>
            <tr runat="server" id="itemPlaceholder" />
          </table>
        </LayoutTemplate>
        <ItemTemplate>
          <tr id="Tr2" runat="server">
            <td>
                <asp:LinkButton ID="Button" OnCommand="Date_Click" runat="Server" Text='<%#Convert.ToDateTime(Eval("From")).ToString("d") %>' CommandArgument='<%#Eval("From") %>' CssClass="bold" />
            </td>
            <td>
              <asp:TextBox Enabled="False" ID="LastNameLabel" runat="Server" Text='<%#Eval("Precipitation") %>' />
            </td>
          </tr>
        </ItemTemplate>

    </asp:ListView>
</asp:Content>