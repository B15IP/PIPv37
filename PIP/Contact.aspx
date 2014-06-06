<%@ Page Title="" Language="C#" MasterPageFile="~/BaraDeNavigare_MasterPage.master" AutoEventWireup="true" CodeFile="Contact.aspx.cs" Inherits="Contact" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="form-group" style ="position:relative;left:20%;top:10%">
    <asp:Label ID="Label10" class="control-label" for="inputDefault" runat="server" ForeColor="#FF9933" Text="Pentru orice problema ne puteti contacta completand datele in campurile de mai jos si apasand Send"></asp:Label>
    <br />
    <br />
    <asp:TextBox ID="nume_sender" class="form-control" style="max-width:15%" placeholder="Nume Prenume" runat="server"></asp:TextBox>
    <br />
    <br />
    <asp:TextBox ID="email" class="form-control"  style="max-width:15%" placeholder="Email" runat="server"></asp:TextBox>
    <br />
    <br />
     <asp:TextBox ID="subiect" class="form-control"  style="max-width:15%"  placeholder="Subject" runat="server"></asp:TextBox>
    <br />
    <br />
    <asp:TextBox id="mesaj" class="form-control" style="max-width:15%"  placeholder="Mesaj" TextMode="multiline" Columns="20" Rows="5" Height="69px" Width="181px" runat="server"  />
    <br />
    <br />
    <asp:Label ID="MesajEroare" runat="server"></asp:Label>
    <br />
    <asp:Button ID="Button_sendEmail" class="btn btn-success" runat="server" Text="Send" OnClick="Button_sendEmail_Click" />
    <br />
        </div>
</asp:Content>

