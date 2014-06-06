<%@ Page Title="" Language="C#" MasterPageFile="~/BaraDeNavigare_MasterPage.master" AutoEventWireup="true" CodeFile="homepage.aspx.cs" Inherits="homepage" %>


<%@ Register Assembly="GMaps" Namespace="Subgurim.Controles" TagPrefix="cc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderFacebook" Runat="Server">
<li><asp:ImageButton ID="btnLoginFacebook" runat="server" ImageUrl="./images/FacebookIcon.png" OnClick="LoginFacebook" /></li>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <link rel="stylesheet" type="text/css" href="chestii.css">
     <link rel="stylesheet" type="text/css" href="bootstrap.css">
       <link rel="stylesheet" type="text/css" href="bootstrap.min.css">

    <style type="text/css">
v\:*{
    behavior:url(#default#VML);
}
</style>
    <div class="form-group" style ="width: 90%;margin-left: auto;margin-right: auto;">
    <div class="jumbotron">
    <div id="home">
        <div class="sidebar">
        </div>
        <div class="content">

            <asp:GridView ID="listaInvitatii" style="position:relative;left:2%" runat="server" BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Horizontal" Width="380px" >
             <AlternatingRowStyle BackColor="#F7F7F7" />
           <Columns>
               <asp:TemplateField HeaderText="Accept">
            <ItemTemplate>
                <asp:Button ID="bta" runat="server"  Visible="False" Text="Accepta"></asp:Button>
            </ItemTemplate>
        </asp:TemplateField>
                  <asp:TemplateField HeaderText="Reject">
            <ItemTemplate>
                <asp:Button ID="btr" runat="server"  Visible="False" Text ="Refuza"></asp:Button>
            </ItemTemplate>
        </asp:TemplateField>
                 <asp:TemplateField HeaderText="Reject">
            <ItemTemplate>
                <asp:Button ID="denumire" runat="server"  Visible="False" Text ="Denumire"></asp:Button>
            </ItemTemplate>
        </asp:TemplateField>
             </Columns>
              <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
             <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
             <PagerStyle ForeColor="#4A3C8C" HorizontalAlign="Right" BackColor="#E7E7FF" />
             <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
             <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
             <SortedAscendingCellStyle BackColor="#F4F4FD" />
             <SortedAscendingHeaderStyle BackColor="#5A4C9D" />
             <SortedDescendingCellStyle BackColor="#D8D8F0" />
             <SortedDescendingHeaderStyle BackColor="#3E3277" />
              </asp:GridView>

          
          <br/>
             <div style="position:relative;left:2%;top:15%">
                 
            <asp:GridView ID="GridViewFeatured" runat="server" 
                 
                CellPadding="3" GridLines="None" 
                CellSpacing="1" 
                CssClass="mGrid"
                AllowPaging="true"  Width="704px" 
                >
                 <Columns>
             </Columns>
              <FooterStyle BackColor="#C6C3C6" ForeColor="Black" />
             <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#E7E7FF" HorizontalAlign="Center" />
             <PagerStyle ForeColor="Black" HorizontalAlign="Left" BackColor="#C6C3C6" />
             <RowStyle BackColor="#DEDFDE" ForeColor="Black" HorizontalAlign="Center" />
             <SelectedRowStyle BackColor="#9471DE" Font-Bold="True" ForeColor="White" />
             <SortedAscendingCellStyle BackColor="#F1F1F1" />
             <SortedAscendingHeaderStyle BackColor="#594B9C" />
             <SortedDescendingCellStyle BackColor="#CAC9C9" />
             <SortedDescendingHeaderStyle BackColor="#33276A" />
            </asp:GridView>
                  
                 <asp:GridView ID="GridViewParticipa" runat="server" 
                 
                CellPadding="3" GridLines="None" 
                CellSpacing="1" 
                CssClass="mGrid"
                AllowPaging="true"  Width="704px" >
                 <Columns>
             </Columns>
              <FooterStyle BackColor="#C6C3C6" ForeColor="Black" />
             <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#E7E7FF" HorizontalAlign="Center" />
             <PagerStyle ForeColor="Black" HorizontalAlign="Left" BackColor="#C6C3C6" />
             <RowStyle BackColor="#DEDFDE" ForeColor="Black" HorizontalAlign="Center" />
             <SelectedRowStyle BackColor="#9471DE" Font-Bold="True" ForeColor="White" />
             <SortedAscendingCellStyle BackColor="#F1F1F1" />
             <SortedAscendingHeaderStyle BackColor="#594B9C" />
             <SortedDescendingCellStyle BackColor="#CAC9C9" />
             <SortedDescendingHeaderStyle BackColor="#33276A" />
            </asp:GridView>
                    
            </div>
             <div class="form-group" style="position:absolute;left:63%;top:35%">
<asp:TextBox ID="SearchBox" CssClass="inputs" runat="server" placeholder="cauta" MaxLength="20" ></asp:TextBox>
            <br/>
         
        <br />
<asp:Button ID="ButtonSearch" class="button_cauta" runat="server" Text="Cauta" OnClick="ButtonSearch_Click" ToolTip="0"></asp:Button>

            <br /><br />
        <br /> <br />
            <asp:Panel ID="Panelsearch" runat="server"></asp:Panel>
</div>
          <%--  <div style="position:relative;left:3%;top:15%">--%>
                

<%--<asp:Button ID="ButtonEvenimenteCreateAscunde" CssClass="button_ascunde" runat="server" Text="Ascunde"  OnClick="ButtonCreateAscunde_Click"></asp:Button>
            <br />--%>
                
                 <asp:Panel ID="Panelcreate" runat="server"></asp:Panel>
      
               <%-- </div>  --%>  

          <%--   <div style="position:relative;left:25%;top:15%">--%>
            <div style="position:relative;left:63%">
          
  
                 </div>
            <%--<asp:Button ID="Button_EvenimenteOrganizateAscunde" CssClass="button_ascunde" runat="server" Text="Ascunde" OnClick="ButtonEvenimenteOrganizateAscunde_Click"></asp:Button>
    --%>   
            
            <asp:GridView ID="GridView_listaEvenimenteOrganizate" ToolTip ="0" style="position:relative;left:2%" CssClass="mGrid" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" Visible="false" Width="533px" >
             <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
             <Columns>
                  <asp:TemplateField HeaderText="Renunta">
            <ItemTemplate>
                <asp:Button ID="bta0" runat="server"  Visible="False" Text="Renunta"></asp:Button>
            </ItemTemplate>
        </asp:TemplateField>
                 <asp:TemplateField HeaderText="Pagina Eveniment">
            <ItemTemplate>
                <asp:Button ID="accesez_eveniment" runat="server"  Visible="False" Text="Click"></asp:Button>
            </ItemTemplate>
        </asp:TemplateField>
             </Columns>
             <EditRowStyle BackColor="#999999" />
             <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
             <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
             <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
             <RowStyle BackColor="#F7F6F3" ForeColor="#333333" HorizontalAlign="Center" />
             <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
             <SortedAscendingCellStyle BackColor="#E9E7E2" />
             <SortedAscendingHeaderStyle BackColor="#506C8C" />
             <SortedDescendingCellStyle BackColor="#FFFDF8" />
             <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
         </asp:GridView>
     
                  <%--</div>--%>

<br />
            

             
 <br />
            <div style="position:absolute;left:63%;top:55%" >
<asp:Button ID="ButtonCreareEveniment" class="btn btn-success btn-lg" runat="server" Text="Creaza Eveniment" OnClick="Button_CreazaEveniment"></asp:Button>
        <br /> <br /> <asp:Button ID="ButtonEvenimenteCreate" CssClass="btn btn-danger btn-lg" runat="server" Text="Evenimente Create" OnClick="ButtonCreate_Click" ToolTip="0"></asp:Button>   
         <br /> <br /> <asp:Button ID="Button_EvenimenteOrganizate" CssClass="btn btn-primary btn-lg" runat="server" Text="Evenimente organizate" ToolTip="0" OnClick="Button_EvenimenteOrganizate_Click" />
  
            </div>



            <asp:SqlDataSource ID="SQL_listaInvitatii" runat="server"></asp:SqlDataSource>
            <asp:Label ID="LabelMesajEroare" runat="server"></asp:Label>
            </div>
       </div>
    </div>
    
</div>
</asp:Content>



 
