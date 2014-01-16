<%@ Page Title="Pudddervarsel" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PudderVarsel.Web._Default" %>

<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>--%>


<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    
<%--    <asp:TextBox ID="TextBoxSearch" runat="server"></asp:TextBox>

        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" TargetControlID="TextBoxSearch" MinimumPrefixLength="2" CompletionInterval="10" EnableCaching="true" 
        CompletionSetCount="3" UseContextKey="True" ServiceMethod="GetCompletionList"></asp:AutoCompleteExtender>--%>
    

    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1><%: Title %></h1>
                <%--<h2>Modify this template to jump-start your ASP.NET application.</h2>--%>
            </hgroup>
            <%--<p>
                To learn more about ASP.NET, visit <a href="http://asp.net" title="ASP.NET Website">http://asp.net</a>.
                The page features <mark>videos, tutorials, and samples</mark> to help you get the most from ASP.NET.
                If you have any questions about ASP.NET visit
                <a href="http://forums.asp.net/18.aspx" title="ASP.NET Forum">our forums</a>.
            </p>--%>
        </div>
    </section>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <style type="text/css">.hide { display: none; }

 .Textbox { font-size: 10px;
     font-weight: bold;
 }
    </style>

    <asp:DropDownList ID="DropDownListDistance" runat="server" CssClass="bold" AutoPostBack="True" OnSelectedIndexChanged="DropDownListDistance_SelectedIndexChanged">
        <asp:ListItem Selected="True">Velg søkeradius</asp:ListItem>
        <asp:ListItem>10</asp:ListItem>
        <asp:ListItem>50</asp:ListItem>
        <asp:ListItem>100</asp:ListItem>
        <asp:ListItem>200</asp:ListItem>
        <asp:ListItem>300</asp:ListItem>
        <asp:ListItem>500</asp:ListItem>
        <asp:ListItem>Norway</asp:ListItem>
    </asp:DropDownList>
    
    <asp:Label runat="server" Text="eller"></asp:Label>
    <asp:TextBox ID="TextBoxSearch" Text="Navn på alpinsenter" runat="server" CssClass="Textbox" AutoPostBack="True" OnTextChanged="TextBoxSearch_TextChanged" Width="110" OnClick="this.value=''"></asp:TextBox>
    <br/>
    <asp:Button ID="ButtonSearch" Text="Finn pudder" Enabled="False" runat="server" OnClick="ButtonSearch_Click" />
    

    <asp:ListView ID="ListViewLocations" runat="server">
        <LayoutTemplate>
          <table id="Table1" cellpadding="2" border="1" runat="server" width="150">
            <tr id="Tr1" runat="server">
              <th id="Th2" runat="server"><asp:Label runat="server" Text="Sted" CssClass="small"></asp:Label></th>
              <th id="Th3" runat="server">Nedbør (cm)</th>
                <th id="Th5" runat="server">Neste tre dager</th>
<%--                <th id="Th4" runat="server">Avstand (km)</th>
                <th id="Th6" runat="server"></th>
                <th id="Th1" runat="server"></th>--%>
            </tr>
            <tr runat="server" id="itemPlaceholder" />
          </table>
        </LayoutTemplate>
        <ItemTemplate>
          <tr id="Tr2" runat="server">
            <td>
              <asp:LinkButton CommandArgument='<%#Eval("Name") %>'  OnCommand="Details_Click" ID="LocationLinkButton" runat="Server" Text='<%#Eval("Name") %>' CssClass="small" Width="100px"/>
            </td>
            <td>
              <asp:TextBox Enabled="False" ID="LastNameLabel" runat="Server" Text='<%#Eval("TotalPrecipitation") %>' Width="50px" CssClass="bold" />
            </td>
            <td>
              <asp:TextBox Enabled="False" ID="TextBox2" runat="Server" Text='<%#Eval("ThreeDaysPrecipitation") %>' Width="50px" CssClass="bold" />
            </td>
              <%--<td>
              <asp:TextBox Enabled="False" ID="TextBox1" runat="Server" Text='<%#Eval("Distance", "{0:0.#}") %>' Width="50px" CssClass="bold" />
            </td>
            <td>
                <asp:Image ID="Image1" runat="server" ImageUrl='<%#"~/images/" + Eval("ImageUrl") %>' />
            </td>--%>
          </tr>
        </ItemTemplate>

    </asp:ListView>
    
    <asp:TextBox id="latitude" runat="server" CssClass="hide" />
    <asp:TextBox runat="server" id="longitude" CssClass="hide" />
    
    <script type="text/javascript">

        function openNewWin(url) {
            var x = window.open(url, 'mynewwin', 'width=600,height=600,resizable=yes');
            if (x != null) {
                x.focus();
            }
        }

        function setText(val, e) {
            document.getElementById(e).value = val;
        }

        function insertText(val, e) {
            document.getElementById(e).value += val;
        }

        var nav = null;

        function requestPosition() {
            if (nav == null) {
                nav = window.navigator;
            }
            if (nav != null) {
                var geoloc = nav.geolocation;
                if (geoloc != null) {
                    
                    geoloc.getCurrentPosition(successCallback);
                }
                else {
                    alert("geolocation not supported");
                    document.getElementById("DropDownListDistance").disabled = true;
                    
                }
            }
            else {
                alert("Navigator not found");
            }
        }

        function successCallback(position) {
            
            setText(position.coords.latitude, "MainContent_latitude");
            setText(position.coords.longitude, "MainContent_longitude");
            var pageId = '<%=  Page.ClientID %>';
            __doPostBack(pageId, 'LocationOK');

        }
        
        function Select() {
            document.getElementById("TextBoxSearch").value = "";

        }
    </script>
</asp:Content>

