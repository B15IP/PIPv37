<%@ Page Title="" Language="C#" MasterPageFile="~/BaraDeNavigare_MasterPage.master" AutoEventWireup="true" CodeFile="EvenimenteFeatured.aspx.cs" Inherits="EvenimenteFeatured" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="form-group" style ="width: 70%;margin-left: auto;margin-right: auto;">
     <ul class="breadcrumb" style="max-width:10%">
 <li class="active">  <asp:LinkButton ID="inapoi" runat="server" OnClick="inapoi_Click" >Inapoi</asp:LinkButton> </li>
</ul>
   
    <asp:Label ID="LabelMesajEroare" runat="server"></asp:Label>


    <asp:GridView ID="GridViewFeatured" runat="server" 
                BackColor="White" BorderColor="White"
                 BorderStyle="Ridge" BorderWidth="2px" 
                CellPadding="3" GridLines="None" 
                CellSpacing="1" 
                AllowPaging="true" Height="487px" Width="704px" >
           
                 <Columns>
              
             </Columns>
              <FooterStyle BackColor="#C6C3C6" ForeColor="Black" />
             <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#E7E7FF" />
             <PagerStyle ForeColor="Black" HorizontalAlign="Right" BackColor="#C6C3C6" />
             <RowStyle BackColor="#DEDFDE" ForeColor="Black" />
             <SelectedRowStyle BackColor="#9471DE" Font-Bold="True" ForeColor="White" />
             <SortedAscendingCellStyle BackColor="#F1F1F1" />
             <SortedAscendingHeaderStyle BackColor="#594B9C" />
             <SortedDescendingCellStyle BackColor="#CAC9C9" />
             <SortedDescendingHeaderStyle BackColor="#33276A" />
            </asp:GridView>
    </div>
</asp:Content>

