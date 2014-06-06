<%@ Page Title="" Language="C#" MasterPageFile="~/BaraDeNavigare_MasterPage.master" AutoEventWireup="true" CodeFile="profil.aspx.cs" Inherits="profil" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div class="form-group" style ="width: 70%;margin-left: auto;margin-right: auto;">
    <div id="body">
        <div class="content">
        <asp:Panel ID="Panelprofil" runat="server" Visible="true">
            
            <div class="panel panel-primary">  
  <div class="panel-body">
                <asp:Label ID="Label1" runat="server" Text="Cont"></asp:Label>
                <asp:Label ID="Labelacont" runat="server" Text=""></asp:Label>
                <br />
                <asp:Label ID="Label2" runat="server" Text="Nume"></asp:Label>
                <asp:Label ID="LabelNume" runat="server" Text=""></asp:Label>
                <asp:TextBox ID="TextBoxNume" CssClass="form-control" style="max-width:15%" runat="server" Visible="false"></asp:TextBox>
                <asp:Button ID="ButtonNume" class="btn btn-warning" runat="server" OnClick="ButtonNume_Click" Text="Editeaza" />
           <br />
                <asp:Label ID="Label4" runat="server" Text="Prenume"></asp:Label>
                <asp:Label ID="LabelPrenume" runat="server" Text="Prenume"></asp:Label>
                <asp:TextBox ID="TextBoxPrenume" style="max-width:15%" CssClass="form-control" runat="server" Visible="false"></asp:TextBox>
                <asp:Button ID="ButtonPrenume" class="btn btn-warning" runat="server" OnClick="ButtonPrenume_Click" Text="Editeaza" />
           <br />
                <asp:Label ID="Label6" runat="server" Text="Email"></asp:Label>
                <asp:Label ID="LabelEmail" runat="server" Text="Email"></asp:Label>
                
           <br />
            <asp:LinkButton ID="LinkButton2" runat="server" OnClick="Buttonschimbaparola"><p>Schimba parola</p></asp:LinkButton>
                </div>
                </div>
            
            <asp:Panel ID="Panelschimbaparola" runat="server" Visible="false">
                    <asp:Label ID="Label3" runat="server" Text="Parola veche"></asp:Label>
                    <asp:TextBox ID="TextBoxparola" style="max-width:15%" CssClass="form-control" runat="server"></asp:TextBox>
                    <br />
                    <asp:Label ID="Label5" runat="server" Text="Parola noua"></asp:Label>
                    <asp:TextBox ID="TextBoxparolan" style="max-width:15%" CssClass="form-control" runat="server"></asp:TextBox>
                    <br />
                    <asp:Label ID="Label7" runat="server" Text="Confirmare parola noua"></asp:Label>
                    <asp:TextBox ID="TextBoxparolan2" style="max-width:15%" CssClass="form-control" runat="server"></asp:TextBox>
                    <br />
                    <asp:Button ID="Buttonschimba" runat="server" class="btn btn-warning" Text="Schimba" OnClick="Buttonschimba_Click" />
                    <br />
                    <asp:Label ID="Labelparolaeroare" runat="server" Text="" style="color:red"></asp:Label>
                </asp:Panel>
              <br />
             </asp:Panel>
        </div>
        <div class="sidebar">
        <asp:Panel ID="PanelInvitatii" runat="server">
         <asp:GridView ID="GridView_listaEvenimenteOrganizate" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None">
             <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
             <Columns>
                  <asp:TemplateField HeaderText="Renunta">
                    <ItemTemplate>
                    <asp:Button ID="bta" runat="server" CssClass="btn_edit" value="Edit" Visible="False" Text="Renunta"></asp:Button>
                    </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="Pagina Eveniment">
                    <ItemTemplate>
                    <asp:Button ID="accesez_eveniment" CssClass="btn_edit" runat="server" value="Edit" Visible="False" Text="Click"></asp:Button>
                    </ItemTemplate>
                  </asp:TemplateField>
             </Columns>
             <EditRowStyle BackColor="#999999" />
             <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
             <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
             <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
             <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
             <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
             <SortedAscendingCellStyle BackColor="#E9E7E2" />
             <SortedAscendingHeaderStyle BackColor="#506C8C" />
             <SortedDescendingCellStyle BackColor="#FFFDF8" />
             <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
         </asp:GridView>
         <asp:Label ID="LabelMesajEroare" runat="server"></asp:Label>
        </asp:Panel>
            </div>
        </div>
         </div>
</asp:Content>

