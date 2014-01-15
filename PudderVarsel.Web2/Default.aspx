<%@ Page Title="Pudddervarsel" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PudderVarsel.Web._Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    
<%--    <asp:TextBox ID="TextBoxSearch" runat="server"></asp:TextBox>

        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" TargetControlID="TextBoxSearch" MinimumPrefixLength="2" CompletionInterval="10" EnableCaching="true" 
        CompletionSetCount="3" UseContextKey="True" ServiceMethod="GetCompletionList"></asp:AutoCompleteExtender>--%>
    

    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1><%: Title %>.</h1>
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
    <style type="text/css">.hide { display: none; }</style>

    <asp:DropDownList ID="DropDownListDistance" runat="server" CssClass="bold" AutoPostBack="True" OnSelectedIndexChanged="DropDownListDistance_SelectedIndexChanged">
        <asp:ListItem Selected="True">Choose radius</asp:ListItem>
        <asp:ListItem>10</asp:ListItem>
        <asp:ListItem>50</asp:ListItem>
        <asp:ListItem>100</asp:ListItem>
        <asp:ListItem>200</asp:ListItem>
        <asp:ListItem>300</asp:ListItem>
        <asp:ListItem>500</asp:ListItem>
        <asp:ListItem>Norway</asp:ListItem>
    </asp:DropDownList>
    
    <asp:TextBox ID="TextBoxSearch" Enabled="False" runat="server" AutoPostBack="True" OnTextChanged="TextBoxSearch_TextChanged"></asp:TextBox>
    

    <asp:ListView ID="ListViewLocations" runat="server">
        <LayoutTemplate>
          <table id="Table1" cellpadding="2" border="1" runat="server">
            <tr id="Tr1" runat="server">
              <th id="Th2" runat="server">Location</th>
              <th id="Th3" runat="server">Percipitation</th>
                <th id="Th5" runat="server">Three days</th>
                <th id="Th4" runat="server">Distance</th>
                <th id="Th6" runat="server">Precipitation type</th>
                <th id="Th1" runat="server"></th>
            </tr>
            <tr runat="server" id="itemPlaceholder" />
          </table>
        </LayoutTemplate>
        <ItemTemplate>
          <tr id="Tr2" runat="server">
            <td>
              <%--<asp:LinkButton CommandArgument='<%#Eval("LocationUrl") %>' ID="LinkButton1" runat="Server" Text='<%#Eval("Name") %>' CssClass="bold" />--%>
              <asp:LinkButton CommandArgument="" ID="LocationLinkButton" runat="Server" Text='<%#Eval("Name") %>' CssClass="bold" />
            </td>
            <td>
              <asp:TextBox Enabled="False" ID="LastNameLabel" runat="Server" Text='<%#Eval("TotalPrecipitation") %>' Width="100px" CssClass="bold" />
            </td>
            <td>
              <asp:TextBox Enabled="False" ID="TextBox2" runat="Server" Text='<%#Eval("ThreeDaysPrecipitation") %>' Width="100px" CssClass="bold" />
            </td>
              <td>
              <asp:TextBox Enabled="False" ID="TextBox1" runat="Server" Text='<%#Eval("Distance", "{0:0.#}") %>' Width="100px" CssClass="bold" />
            </td>
            <td>
              <asp:TextBox Enabled="False" ID="TextBox3" runat="Server" Text='<%#Eval("PrecipitationType") %>' Width="100px" CssClass="bold" />
            </td>
            <td>
              <asp:LinkButton ID="Button" runat="Server" Text="Details" OnCommand="Details_Click" CommandArgument='<%#Eval("Name") %>' CssClass="bold" />
            </td>
          </tr>
        </ItemTemplate>

    </asp:ListView>
    
    
    <asp:TextBox id="time" runat="server" />
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
    </script>
</asp:Content>

