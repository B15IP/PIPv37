<%@ Page Title="" Language="C#" MasterPageFile="~/BaraDeNavigare_MasterPage.master" AutoEventWireup="true" CodeFile="Eveniment.aspx.cs" Inherits="Eveniment" %>


<%-- ---------------------------------------------------------------------------------%>
<%@ Register Assembly="GMaps" Namespace="Subgurim.Controles" TagPrefix="cc1" %>
<%--~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~--%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link rel="stylesheet" type="text/css" href="chestii.css" />
    <link rel="stylesheet" type="text/css" href="bootstrap.css" />
    <link rel="stylesheet" type="text/css" href="bootstrap.min.css" />
    <div class="form-group" style ="width: 70%;margin-left: auto;margin-right: auto;">
    <div class="navbar navbar-inverse">
        <div class="navbar-collapse collapse navbar-inverse-collapse">
            <ul class="nav navbar-nav">
                <li>
                    <asp:LinkButton ID="ButtonInformatii" runat="server" Text="Informatii" OnClick="ButtonInformatii_Click" /></li>
                <li>
                    <asp:LinkButton ID="ButtonInscriere" runat="server" Text="Inscriere" OnClick="ButtonInscriere_Click" /></li>
                <li>
                    <asp:LinkButton ID="ButtonResurse" runat="server" Text="Resurse" OnClick="ButtonResurse_Click" /></li>
                <li>
                    <asp:LinkButton ID="ButtonForum" runat="server" Text="Forum" OnClick="ButtonForum_Click" /></li>
                <li>
                    <asp:LinkButton ID="Button_EvenimenteFeatured" runat="server" Text="Evenimente Asemanatoare" ToolTip="0" OnClick="Button_EvenimenteFeatured_Click" /></li>
                <li>
                    <asp:LinkButton ID="ButtonInvitaOrganizatori" runat="server" OnClick="ButonInvitaOrganizatori_Click" Visible="false" Text="Lista organizatori" />
                </li>
            </ul>
        </div>
    </div>
    <div id="body">
        <asp:Panel ID="PanelInformatii" runat="server" Visible="true">
            <br />


            <div class="panel panel-primary">

                <div class="panel-heading">
                    <asp:Label ID="Label1" runat="server" class="panel-title" Text="Numele evenimentului : "></asp:Label>
                    <asp:Label ID="numeEvenimentLabel" runat="server" Text="" Font-Bold="True" Font-Size="12"></asp:Label>
                    <asp:TextBox ID="numeEveniment" Style="max-width: 15%" class="form-control" runat="server"></asp:TextBox>
                    <asp:Button class="btn btn-warning" ID="ButtonEditeazaNume" runat="server" Text="Editeaza Numele" OnClick="ButtonEditeazaNume_Click" />
                    <asp:Button class="btn btn-warning" ID="ButtonSubmitNume" runat="server" Text="Ok" OnClick="ButtonSubmitNume_Click" />

                </div>

            </div>
            <ul class="nav nav-pills">
                <li class="active">
                    <asp:Label ID="numarParticipanti" runat="server">Participanti
                        <asp:Label runat="server" ID="nr_part" class="badge">42</asp:Label></asp:Label></li>
            </ul>
            <br />
            <div class="panel panel-primary" style="width: 50%">
                <div class="panel-body">
                    <asp:Label ID="Label3" runat="server" Text="Eveniment creat de : "></asp:Label>
                    <asp:Label ID="proprietarLabel" runat="server" Text=""></asp:Label>
                </div>
            </div>






            <asp:Label ID="Label6" runat="server" Text="Etichete"></asp:Label>
            <asp:Label ID="etichetaEvenimentLabel" runat="server" Text=""></asp:Label>
            <asp:TextBox class="form-control" ID="etichetaEveniment" Style="max-width: 15%" runat="server"></asp:TextBox>
            <asp:Button ID="ButtonEditeazaEtichete" class="btn btn-warning" runat="server" Text="Editeaza Etichetele" OnClick="ButtonEditeazaEtichete_Click" />
            <asp:Button ID="ButtonSubmitEtichete" class="btn btn-warning" runat="server" Text="Ok" OnClick="ButtonSubmitEtichete_Click" />

            <%--<table id="tabel_descriere_locatie_data" class="table table-striped table-hover " style="position:relative;left:15%;max-width: 80%">
                <tbody>
                    <tr class="active">
                        <td>--%>

                            <div class="panel panel-primary" style="max-width: 80%">
                                <div class="panel-heading">
                                    <asp:Label ID="Label7" runat="server" Text="Descrierea evenimentului"></asp:Label>
                                </div>
                                <div class="panel-body">
                                    <asp:Label ID="descriereEvenimentLabel" runat="server" Text=""></asp:Label>

                                    <asp:TextBox class="form-control" ID="descriereEveniment" runat="server" TextMode="multiline" Columns="20" Rows="5" Height="100px" Width="200px"></asp:TextBox>
                                    <asp:Button ID="ButtonEditeazaDescriere" class="btn btn-warning" runat="server" Text="Editeaza Descrierea" OnClick="ButtonEditeazaDescriere_Click" />
                                    <asp:Button ID="ButtonSubmitDescriere" runat="server" class="btn btn-warning" Text="Ok" OnClick="ButtonSubmitDescriere_Click" />
                                </div>
                            </div>
                       <%-- </td>

                        <td>--%>
                            <table class="table table-striped table-hover " style="max-width: 50%">
                                <tbody>
                                    <tr class="info">
                                        <td>

                                            <asp:Label ID="Label8" runat="server" Text="Orasul"></asp:Label>
                                            <asp:Label ID="orasEvenimentLabel" runat="server" Text=""></asp:Label>


                                            <asp:TextBox class="form-control" Style="max-width: 15%" ID="orasEveniment" runat="server"></asp:TextBox>
                                            <asp:Button ID="ButtonEditeazaOras" class="btn btn-warning" runat="server" Text="Editeaza Orasul" OnClick="ButtonEditeazaOras_Click" />
                                            <asp:Button ID="ButtonSubmitOras" class="btn btn-warning" runat="server" Text="Ok" OnClick="ButtonSubmitOras_Click" />
                                            <br />

                                            <asp:Label ID="Label9" runat="server" Text="Judetul"></asp:Label>
                                            <asp:Label ID="judetEvenimentLabel" runat="server" Text=""></asp:Label>

                                            <asp:TextBox class="form-control" ID="judetEveniment" Style="max-width: 15%" runat="server"></asp:TextBox></>
                <asp:Button ID="ButtonEditeazaJudet" class="btn btn-warning" runat="server" Text="Editeaza Judetul" OnClick="ButtonEditeazaJudet_Click" />
                                            <asp:Button ID="ButtonSubmitJudet" class="btn btn-warning" runat="server" Text="Ok" OnClick="ButtonSubmitJudet_Click" />
                                            <br />

                                            <asp:Label ID="Label10" runat="server" Text="Tara"></asp:Label>
                                            <asp:Label ID="taraEvenimentLabel" runat="server" Text=""></asp:Label>

                                            <asp:TextBox ID="taraEveniment" Style="max-width: 15%" class="form-control" runat="server"></asp:TextBox>
                                            <asp:Button ID="ButtonEditeazaTara" class="btn btn-warning" runat="server" Text="Editeaza Tara" OnClick="ButtonEditeazaTara_Click" />
                                            <asp:Button ID="ButtonSubmitTara" class="btn btn-warning" runat="server" Text="Ok" OnClick="ButtonSubmitTara_Click" />

                                        </td>
                                    </tr>
                                    <%----------------------------%>
                                    <br />
                                    <tr >
                                        <td>
                                            <div class="panel panel-primary" style="max-width: 70%; position: relative">
                                                <div class="panel-body">
                                                    <asp:Label ID="Label2" runat="server" Text="Ziua :   "></asp:Label>
                                                    <asp:Label ID="ziuaEvenimentLabel" runat="server" Text=""></asp:Label>
                                                    <asp:TextBox ID="ziuaEveniment" Style="max-width: 15%" class="form-control" runat="server"></asp:TextBox>
                                                    <br />
                                                    <asp:Label ID="Label4" runat="server" Text="Luna :   "></asp:Label>
                                                    <asp:Label ID="lunaEvenimentLabel" runat="server" Text=""></asp:Label>
                                                    <asp:TextBox ID="lunaEveniment" Style="max-width: 15%" class="form-control" Text="" runat="server"></asp:TextBox>
                                                    <asp:DropDownList ID="lista_luni" runat="server">
                                                        <asp:ListItem Value="1" Text="ianuarie" />
                                                        <asp:ListItem Value="2" Text="februarie" />
                                                        <asp:ListItem Value="3" Text="martie" />
                                                        <asp:ListItem Value="4" Text="aprilie" />
                                                        <asp:ListItem Value="5" Text="mai" />
                                                        <asp:ListItem Value="6" Text="iunie" />
                                                        <asp:ListItem Value="7" Text="iulie" />
                                                        <asp:ListItem Value="8" Text="august" />
                                                        <asp:ListItem Value="9" Text="septembrie" />
                                                        <asp:ListItem Value="10" Text="octombrie" />
                                                        <asp:ListItem Value="11" Text="noiembrie" />
                                                        <asp:ListItem Value="12" Text="decembrie" />
                                                    </asp:DropDownList>
                                                    <br />
                                                    <asp:Label ID="Label5" runat="server" Text="Anul :   "></asp:Label>
                                                    <asp:Label ID="anEvenimentLabel" runat="server" Text=""></asp:Label>
                                                    <asp:TextBox ID="anEveniment" class="form-control" runat="server" Style="max-width: 15%"></asp:TextBox>
                                                    <br />
                                                    <asp:Label ID="Label6ora" runat="server" Text="Ora :    "></asp:Label>
                                                    <asp:Label ID="oraEvenimentLabel" runat="server" Text=""></asp:Label>
                                                    <asp:TextBox ID="oraEveniment" class="form-control" runat="server" Style="max-width: 15%"></asp:TextBox>
                                                    <asp:Label ID="Label7minut" runat="server" Text="Minut :   "></asp:Label>
                                                    <asp:Label ID="minutEvenimentLabel" runat="server" Text=""></asp:Label></>
                 <asp:TextBox ID="minutEveniment" class="form-control" runat="server" Style="max-width: 15%"></asp:TextBox>
                                                    <asp:Button ID="ButtonEditeazaData" class="btn btn-warning" runat="server" Text="Editeaza Data" OnClick="ButtonEditeazaData_Click" />
                                                    <asp:Button ID="ButtonSubmitData" class="btn btn-warning" runat="server" Text="Ok" OnClick="ButtonSubmitData_Click" />                                                 
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <asp:Button ID="ButtonStergeEveniment" class="btn btn-danger" runat="server" Text="Sterge Eveniment" OnClick="ButtonIntrebareSterge_Click" />
                            <asp:Label ID="Label11sterge" runat="server" Text="Sunteti siguri?"></asp:Label>
                            <asp:Button ID="ButtonStergeEvenimentOK" class="btn btn-warning" runat="server" Text="Da" OnClick="ButtonStergeEveniment_Click" />
                            <asp:Button ID="ButtonStergeEvenimentNO" class="btn btn-danger" runat="server" Text="Nu" OnClick="ButtonIntrebareReturn_Click" />
                            <br />
                     <%--   </td>
                    </tr>
                </tbody>
            </table>--%>
            <br />


            <asp:Label ID="MesajRaspuns" runat="server" Text=""></asp:Label>

            <%-- ---------------------------------------------------------------------------------%>
            
             <asp:TextBox ID="textbox_latitudine" placeholder="latitudine" class="form-control" runat="server" Style="max-width: 15%" Visible =" false"></asp:TextBox>
             <asp:TextBox ID="textbox_longitudine" placeholder="longitudine" class="form-control" runat="server" Style="max-width: 15%" Visible =" false"></asp:TextBox>
               <asp:Button ID="ButtonIntroduceDateLocatie" class="btn btn-warning" runat="server" Text="Submit" OnClick="ButtonSubmitLocatie_Click" Visible =" false" />
                                                   
         <%--   <asp:Button ID="ButtonLocatie" class="btn btn-warning" runat="server" Text="Editeaza Locatie" OnClick="ButtonEditeazaLocatie_Click" />
            --%>                

            <cc1:GMap ID="GMap1" Visible="True" runat="server" Width="600px" Height="350px" />


            <%--~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~--%>
        </asp:Panel>

        <%--<iframe src="https://www.google.com/maps/embed?pb=!1m14!1m12!1m3!1d182321.4491300392!2d26.09463755!3d44.43782575!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!5e0!3m2!1sro!2sro!4v1401134618176" width="700" height="400" frameborder="0" style="border:0"></iframe>--%>

        <asp:Panel ID="PanelInscriere" runat="server" Visible="false">
            <div class="panel panel-success" style="max-width:50%">
  <div class="panel-heading">
            <asp:Panel ID="PanelRoluri" runat="server"></asp:Panel>
      </div>
                </div>
            <asp:Button ID="ButtonAdaugaRol2" class="btn btn-warning" runat="server" Text="Adauga Rol" OnClick="ButtonAdaugaRol2_Click1" />
            <br />
            <div class="well well-lg" id="div_rol" visible =" false" runat="server" style="position: relative; left: 20%; max-width: 40%">
                <asp:Label ID="LabelDenumire" class="control-label" for="inputWarning" runat="server" Text="Denumire:"></asp:Label><asp:TextBox ID="DenumireRol" class="form-control" Style="max-width: 50%" runat="server"></asp:TextBox>
                <br />
                <asp:Label ID="LabelDescriere" class="control-label" for="inputWarning" runat="server" Text="Descriere:"></asp:Label><asp:TextBox ID="DescriereRol" TextMode="multiline" Columns="20" Rows="5" Height="69px" class="form-control" Style="max-width: 50%" runat="server"></asp:TextBox>
                <br />
                <asp:Label ID="LabelMaxUsers" class="control-label" for="inputWarning" runat="server" Text="Numar maxim de participanti:"></asp:Label><asp:TextBox class="form-control" Style="max-width: 50%" ID="MaxUsers" runat="server"></asp:TextBox>
                <br />
                <asp:Button ID="ButtonSubmitRol2" class="btn btn-success" OnClick="ButtonSubmitRol2_Click" runat="server" Text="Submit" />
                <br />

            </div>
            <br />
            <asp:Button ID="ButtonAfisareParticipanti" class="btn btn-success" runat="server" Text="Afiseaza Participanti" />
            <br /><br />
                    <asp:Panel ID="PanelParticipanti" runat="server">
                      
                    </asp:Panel>
                
            <asp:Label ID="LabelEroareRol" runat="server" Text=""></asp:Label>
        </asp:Panel>

        <asp:Panel ID="PanelResurse" runat="server" Visible="false">
            <div class="jumbotron">

                <asp:Panel ID="PanelShow" runat="server"></asp:Panel>

                <asp:Label ID="lblFile" runat="server" Font-Bold="True">Fisier:</asp:Label>
                <input id="filMyFile" class="btn btn-warning" type="file" runat="server" />
                <p></p>

                <asp:Button ID="cmdSend" runat="server" CssClass="btn btn-warning" Text="Upload" />
                <p></p>
                <asp:Label ID="lblInfo" runat="server" Font-Bold="True" Visible="True"></asp:Label>
            </div>
        </asp:Panel>


        <asp:Panel ID="PanelForum" runat="server" Visible="false">
            <div class="panel panel-success" style="max-width: 70%">
                <div class="panel-body">
                    <asp:Button ID="ButtonComentariu" class="btn btn-success" runat="server" Text="Scrieti un comentariu" OnClick="ButtonComentariu_Click" />
                    <br />
                    <asp:Panel ID="PanelComentariuNou" runat="server" Visible="false">
                        <asp:TextBox ID="TextBoxComentariuNou" CssClass="form-group has-success" runat="server" Columns="100" MaxLength="500" Rows="5" TextMode="multiline"></asp:TextBox>
                        <br />
                        <asp:Button ID="ButtonScrieComentariu" class="btn btn-danger" runat="server" Text="Posteaza" OnClick="ButtonScrieComentariu_Click" />
                        <br />
                    </asp:Panel>
                    <div class="panel panel-success" style="max-width: 50%">
                        <div class="panel-heading">
                            <asp:Panel ID="PanelComentarii" runat="server" Visible="true"></asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>

    </div>
        </div>
</asp:Content>

