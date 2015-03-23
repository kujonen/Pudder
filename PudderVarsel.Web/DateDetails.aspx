<%@ Page Language="C#" Title="Date details" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="DateDetails.aspx.cs" Inherits="PudderVarsel.Web.DateDetails" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">

    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h2><asp:LinkButton ForeColor="White" CssClass="big" CommandArgument=''  OnCommand="Home_Click" ID="LastNameLabel" runat="Server" Text="Puddervarsel" /></h2>
            </hgroup>
        </div>
    </section>
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <hgroup class="title">
        <%--<h1><%: Title %>.</h1>--%>
        <h3><%= Location %> <%= Date %></h3>
    </hgroup>
    
      <asp:ListView runat="server" ID="dateDetailResult">
        <LayoutTemplate>
<%--          <table cellpadding="2" border="1" runat="server" id="tblProducts">
            <tr id="Tr1" runat="server">
              <th id="Th2" runat="server">Fra</th>
                <th id="Th1" runat="server">Til</th>
              <th id="Th3" runat="server">Perticipation</th>
                <th id="Th4" runat="server">Temperature</th>
            </tr>
            <tr runat="server" id="itemPlaceholder" />
          </table>--%>
            <p runat="server" id="itemPlaceholder"></p>
        </LayoutTemplate>
        <ItemTemplate>
            <div class="table-powder-details">
            <table>
                  <tr id="Tr2" runat="server">
                    <td>
                      <asp:Label Enabled="False" Width="150px" ID="LabelTo" runat="Server" Text='<%#Convert.ToDateTime(Eval("From")).ToString("HH:mm") %>' />
                    </td>
                    <td>
                      <asp:Label Enabled="False" Width="150px" ID="TextBox1" runat="Server" Text='<%#Convert.ToDateTime(Eval("To")).ToString("HH:mm") %>' />
                    </td>
                    <td>
                      <asp:Label Enabled="False" Width="200px" ID="LabelFrom" runat="Server" Text='<%#Eval("Powder") + "cm" %>' />
                    </td>
                      <td>
                          <asp:Label Enabled="False" Width="150px" ID="TextBox2" runat="Server" Text='<%#Eval("Temperature") + "°" %>' />
                      </td>
                      <td style="padding-top: 8px">
                          <asp:Image Width="50px" Height="50px" runat="server" ImageUrl='<%#"~/images/" + Eval("ImageUrl") %>' />
                      </td>
                  </tr>
               </table>
            </div>
        </ItemTemplate>

    </asp:ListView>
</asp:Content>