<%@ Page Title="Powder details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PowderDetails.aspx.cs" Inherits="PudderVarsel.Web.PowderDetails" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <hgroup class="title">
        <h2>Detaljert puddervarsel for <%= Location %></h2>
    </hgroup>
    
      <asp:ListView runat="server" ID="powderDetailResult">
        <LayoutTemplate>
          <table cellpadding="2" border="1" runat="server" id="tblProducts">
            <tr id="Tr1" runat="server">
              <th id="Th4" runat="server">Dag</th>
              <th id="Th2" runat="server">Dato</th>
              <th id="Th3" runat="server">Nedbør (cm)</th>
                <th id="Th5" runat="server">Temperatur</th>
               <th id="Th1" runat="server">Symbol</th>
            </tr>
            <tr runat="server" id="itemPlaceholder" />
          </table>
        </LayoutTemplate>
        <ItemTemplate>
          <tr id="Tr2" runat="server">
            <td>
                <asp:TextBox ID="LinkButton1" Width="150" runat="Server" Enabled="False" Text='<%#Convert.ToDateTime(Eval("From")).ToString("ddd") %>' />
            </td>
            <td>
                <asp:LinkButton ID="Button" OnCommand="Date_Click" runat="Server" Text='<%#Convert.ToDateTime(Eval("From")).ToString("d") %>' CommandArgument='<%#Eval("From") %>' CssClass="bold" />
            </td>
            <td>
              <asp:TextBox Enabled="False" Width="150" ID="LastNameLabel" runat="Server" Text='<%#Eval("Precipitation") %>' />
            </td>
            <td>
              <asp:TextBox Enabled="False" Width="150" ID="TextBox1" runat="Server" Text='<%#Eval("Temperature") %>' />
            </td>
            <td>
                <asp:Image runat="server" ImageUrl='<%#"~/images/" + Eval("ImageUrl") %>' />
            </td>
          </tr>
        </ItemTemplate>

    </asp:ListView>
    
    <asp:Image ID="Image1" runat="server" />
    
    <h3>Nedbør neste tre dager: <%= TreDager %> cm</h3>
    <h3>Totalt denne perioden: <%= Totalt %> cm</h3>
    

<%--    <asp:Chart ID="Chart1" runat="server" Height="200px" Width="600px">
        <Series>
           <asp:Series Name="test1" ChartType="Column" ChartArea="ChartArea1">
           </asp:Series>
       </Series>
       <ChartAreas> 
           <asp:ChartArea Name="ChartArea1" BorderColor="64, 64, 64, 64" 
	BorderDashStyle="Solid" BackSecondaryColor="White" 
	BackColor="64, 165, 191, 228" 
	ShadowColor="Transparent" BackGradientStyle="TopBottom">  
               <area3dstyle Rotation="10" perspective="10" Inclination="15" 
	IsRightAngleAxes="False" wallwidth="0" IsClustered="False"></area3dstyle>  
               <axisy linecolor="64, 64, 64, 64">  
                   <labelstyle font="Trebuchet MS, 8.25pt, style=Bold" />  
                   <majorgrid linecolor="64, 64, 64, 64" />  
               </axisy>  
               <axisx linecolor="64, 64, 64, 64">  
                   <labelstyle font="Trebuchet MS, 8.25pt, style=Bold" />  
                   <majorgrid linecolor="64, 64, 64, 64" />  
               </axisx>  
           </asp:ChartArea>  
       </ChartAreas>
    </asp:Chart>
    --%>

</asp:Content>

