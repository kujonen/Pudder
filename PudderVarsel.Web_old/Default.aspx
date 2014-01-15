<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PudderVarsel.Web._Default" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1><%: Title %>.</h1>
                <h2>Modify this template to jump-start your ASP.NET application.</h2>
            </hgroup>
            <p>
                To learn more about ASP.NET, visit <a href="http://asp.net" title="ASP.NET Website">http://asp.net</a>.
                The page features <mark>videos, tutorials, and samples</mark> to help you get the most from ASP.NET.
                If you have any questions about ASP.NET visit
                <a href="http://forums.asp.net/18.aspx" title="ASP.NET Forum">our forums</a>.
            </p>
        </div>
    </section>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:ListView runat="server">
        <LayoutTemplate>
          <table id="Table1" cellpadding="2" border="1" runat="server">
            <tr id="Tr1" runat="server">
              <th id="Th2" runat="server">Location</th>
              <th id="Th3" runat="server">Percipitation</th>
                <th id="Th4" runat="server">Distance</th>
                <th id="Th1" runat="server"></th>
            </tr>
            <tr runat="server" id="itemPlaceholder" />
          </table>
        </LayoutTemplate>
        <ItemTemplate>
          <tr id="Tr2" runat="server">
            <td>
              <asp:LinkButton CommandArgument='<%#Eval("LocationUrl") %>' ID="LocationLinkButton" runat="Server" Text='<%#Eval("Name") %>' CssClass="bold" />
            </td>
            <td>
              <asp:TextBox Enabled="False" ID="LastNameLabel" runat="Server" Text='<%#Eval("Precipitation") %>' Width="100px" CssClass="bold" />
            </td>
              <td>
              <asp:TextBox Enabled="False" ID="TextBox1" runat="Server" Text='<%#Eval("Distance", "{0:0.#}") %>' Width="100px" CssClass="bold" />
            </td>
            <td>
              <asp:LinkButton ID="Button" runat="Server" Text="Details" OnCommand="Details_Click" CommandArgument='<%#Eval("Name") %>' CssClass="bold" />
            </td>
          </tr>
        </ItemTemplate>

    </asp:ListView>
    
    
    
<%--    <ListView x:Name="ListViewLocations" Grid.Row="1" Height="400" ItemsSource="{Binding}" ItemClick="ListViewLocations_OnItemClick" IsItemClickEnabled="True">
            <ListView.ItemTemplate>
                <DataTemplate>
                    <StackPanel Orientation="Horizontal">

                            <Border Background="{StaticResource ListViewItemPlaceholderBackgroundThemeBrush}" Width="110" Height="110">
                            <TextBlock Text="{Binding TotalPrecipitation}" FontSize="20" FontStyle="Oblique" />
                            </Border>

                        <StackPanel Grid.Column="2" VerticalAlignment="Top" Margin="10,0,0,0">
                            <TextBlock Text="{Binding Name}" Style="{StaticResource TitleTextStyle}" TextWrapping="NoWrap"/>
                            <TextBlock Text="{Binding Distance}" Style="{StaticResource CaptionTextStyle}" TextWrapping="NoWrap"/>
                            <TextBlock Text="{Binding ThreeDaysPrecipitation}" Style="{StaticResource BodyTextStyle}" MaxHeight="60"/>
                        </StackPanel>
                    </StackPanel>


                </DataTemplate>
            </ListView.ItemTemplate>

        </ListView>--%>
</asp:Content>
