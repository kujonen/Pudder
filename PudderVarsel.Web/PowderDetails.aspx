<%@ Page Title="Powder details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PowderDetails.aspx.cs" Inherits="PudderVarsel.Web.PowderDetails" %>
<%@ Import Namespace="System.Globalization" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

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
        <h2><%= Location %></h2>
    </hgroup>
    
      <asp:ListView runat="server" ID="powderDetailResult">
        <LayoutTemplate>
            <p runat="server" id="itemPlaceholder"></p>
        </LayoutTemplate>
        <ItemTemplate>
            <%----%>
            <div class="table-powder-details">
            <%--<table width="100%" style="border: 20px; border-style: solid;">--%>
                <table width="100%">
                  <tr>
                    <td>
                        <asp:LinkButton ForeColor="#1181A4" ID="Button" OnCommand="Date_Click" runat="Server" Text='<%# Convert.ToDateTime(Eval("Day")).ToString("ddd") + " " + 
                        Convert.ToDateTime(Eval("Day")).ToString("dd/MM") %>' CommandArgument='<%#Eval("Day") %>' />
                    </td>
                    <td>
                      <asp:Label ID="LastNameLabel" runat="Server"  Text='<%#Eval("Powder") + "cm" %>' />
                    </td>
                   <%-- <td style="border-left: 0px; border-right: 0px; border-bottom: 2px; border-top: 0px; width: 35%; height: 40px; border-color: #1181A4; border-style: solid">
                      <asp:Label ID="Label1" runat="Server" Width="170" Text='<%#Eval("Precipitation") + "mm" %>' />
                    </td>--%>
                    <td>
                      <asp:Label ID="TextBox1" runat="Server" Text='<%#Eval("AverageTemperature") + "°" %>' />
                    </td>
                    <td>
                        <asp:Image Width="50px" Height="50px" ImageAlign="Top" runat="server" ImageUrl='<%#"~/images/" + Eval("ImageUrl") %>' />
                    </td>
                  </tr>
                </table>
            </div>
        </ItemTemplate>
          
    </asp:ListView>
    
    <p style="font-weight: bold">Avstand fra din posisjon: <%= Distance %> km</p>

    <p>Sist oppdatert: <%= OppdatertDato %> <br/>
        Ny oppdatering: <%= NesteOppdatering %> <br/>
        Siste forsøk: <%= SisteDataHenting %> 
        Hentet fra met: <%= HentetFraMet %> 
    </p>
    <p></p>
    

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

