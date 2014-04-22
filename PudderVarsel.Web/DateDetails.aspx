﻿<%@ Page Language="C#" Title="Date details" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="DateDetails.aspx.cs" Inherits="PudderVarsel.Web.DateDetails" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <hgroup class="title">
        <%--<h1><%: Title %>.</h1>--%>
        <h3><%= Location %> <%= Date %></h3>
    </hgroup>
    
      <asp:ListView runat="server" ID="dateDetailResult">
        <LayoutTemplate>
          <table cellpadding="2" border="1" runat="server" id="tblProducts">
            <tr id="Tr1" runat="server">
              <th id="Th2" runat="server">Fra</th>
                <th id="Th1" runat="server">Til</th>
              <th id="Th3" runat="server">Perticipation</th>
                <th id="Th4" runat="server">Temperature</th>
            </tr>
            <tr runat="server" id="itemPlaceholder" />
          </table>
        </LayoutTemplate>
        <ItemTemplate>
          <tr id="Tr2" runat="server">
            <td>
              <asp:TextBox Enabled="False" Width="50px"  ID="LabelTo" runat="Server" Text='<%#Convert.ToDateTime(Eval("From")).ToString("HH:mm") %>' />
            </td>
            <td>
              <asp:TextBox Enabled="False" Width="50px"  ID="TextBox1" runat="Server" Text='<%#Convert.ToDateTime(Eval("To")).ToString("HH:mm") %>' />
            </td>
            <td>
              <asp:TextBox Enabled="False" Width="50px" ID="LabelFrom" runat="Server" Text='<%#Eval("Precipitation") %>' />
            </td>
              <td>
                  <asp:TextBox Enabled="False" Width="50px" ID="TextBox2" runat="Server" Text='<%#Eval("Temperature") %>' />
              </td>
              <td>
                  <asp:Image runat="server" ImageUrl='<%#"~/images/" + Eval("ImageUrl") %>' />
              </td>
          </tr>
        </ItemTemplate>

    </asp:ListView>
</asp:Content>