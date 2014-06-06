<%@ Page Title="" Language="C#" MasterPageFile="~/BaraDeNavigare_MasterPage.master" AutoEventWireup="true" CodeFile="OrganizatoriEveniment.aspx.cs" Inherits="OrganizatoriEveniment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div class="form-group" style ="width: 70%;margin-left: auto;margin-right: auto;">
    <ul class="breadcrumb" style="max-width:10%">
 <li class="active">  <asp:LinkButton ID="Inapoi" runat="server" OnClick="inapoi_Click" >inapoi</asp:LinkButton> </li>
</ul>
   


     <asp:Label ID="Label20" runat="server" Text="Introduceti username-ul utilizatorului pe care doriti sa-l invitati ca organizator al evenimentului "></asp:Label>
    <br/><br/><br/><br/>
    <div class="panel panel-primary" style="position:relative;top:10%;left:10%;max-width:50%">
        <div class="panel-heading">
    <asp:Button class="button_cauta" ID="ButtonInvitaOrganizatori" runat="server" Text="Invita" BorderColor="#FF9966" BorderStyle="Dashed" style="margin-left: 159px" OnClick="ButtonInvitaOrganizatori_Click" />
        <asp:TextBox ID="UserNameOrganizatorInvitat"  CssClass="inputs" runat="server" style="margin-left: 43px;max-width:20%" Width="134px"></asp:TextBox>
      </div>
        <div class="panel-body">
            <asp:Label ID="MesajEroare" runat="server"></asp:Label>
    <br />
    
     <asp:Label ID="LabelInvitatieTrimisa" runat="server"></asp:Label>
     <asp:GridView ID="listaOrganizatoriEvent" runat="server" CellPadding="3" 
         style="position:relative;left:20%"
         Height="133px" Width="365px" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px">
         <Columns>
                  <asp:TemplateField HeaderText="Elimina">
            <ItemTemplate>
                <asp:Button ID="bte" runat="server" value="Edit" Visible="False" Text="Elimina"></asp:Button>
            </ItemTemplate>
        </asp:TemplateField>
         </Columns>
         <FooterStyle BackColor="White" ForeColor="#000066" />
         <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
         <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
         <RowStyle ForeColor="#000066" />
         <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
         <SortedAscendingCellStyle BackColor="#F1F1F1" />
         <SortedAscendingHeaderStyle BackColor="#007DBB" />
         <SortedDescendingCellStyle BackColor="#CAC9C9" />
         <SortedDescendingHeaderStyle BackColor="#00547E" />
     </asp:GridView>
        </div>
    </div>
     <asp:Panel ID="PanelListaOrganizatori" runat="server"></asp:Panel>
         </div>
</asp:Content>

