<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="CharRadiologyWeb.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <!-- The above 3 meta tags *must* come first in the head; any other head content must come *after* these tags -->
    <title>Charlotte Radiology</title>
      
      <link href="https://fonts.googleapis.com/css?family=Montserrat:300,400,500,600,700,800&amp;subset=latin-ext" rel="stylesheet">
    <!-- Bootstrap -->
    <link href="css/bootstrap.min.css" rel="stylesheet">
    <link href="css/styles.css" rel="stylesheet">
    <link href="css/grid.css" rel="stylesheet">
    <link rel="shortcut icon" href="favicon.png" type="image/x-icon" />
    <!-- HTML5 shim and Respond.js for IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
      <script src="https://oss.maxcdn.com/html5shiv/3.7.3/html5shiv.min.js"></script>
      <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <![endif]-->
</head>
<body>
    <div class="banner">   
    <div class="hero">     
     <div id="header" class="jumbotron d-flex align-items-end">
      <div class="container">
                    
        <div class="row" style="padding-bottom: 20%;">
          <div class="col-md-12">
          </div>
        </div>
        <div class="row">
            <div class="col-lg-12 text-center">
                <h1 class="formobile">Enter your <strong>Info</strong> below</h1>
                <h2 class="formobile"></strong></span></h2>

                <h2 id="mobile">Enter your <strong>Info</strong> below</h2>
            </div>
        </div>
      </div>
    </div>
<div id="links">&nbsp;</div>
    <div id="info">
      
        <div class="container">
            <div class="row">
    <form id="form1" runat="server">
                <telerik:RadScriptManager runat="server"></telerik:RadScriptManager>
            <telerik:RadAjaxPanel ID="pnl" runat="server">
                <telerik:RadNotification ID="RadNotification1" runat="server" RenderMode="Lightweight"
                    Position="Center" Animation="Fade" EnableRoundedCorners="true" EnableShadow="true" Width="50%"
                    Title="Notification Title" Text="RadNotification is a light control which can be used to display a notification message"
                    Style="z-index: 2" AutoCloseDelay="20000">
                </telerik:RadNotification>
            </telerik:RadAjaxPanel>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server"></telerik:RadAjaxManager>

                    <div class="clearfix"></div>
        <asp:Panel ID="Panel1" runat="server">
                            <div class="form-group col-xs-11 col-sm-4 col-md-4 col-lg-4">
                                <label for="txtFirstName">First Name</label>
                                <asp:TextBox ID="txtFirstName" runat="server" placeholder="First Name" class="form-control" ></asp:TextBox>
                            </div>
                            <div class="form-group col-xs-11 col-sm-4 col-md-4 col-lg-4">
                                <label for="txtLastName">Last Name</label>
                                <asp:TextBox ID="txtLastName" runat="server" placeholder="Last Name" class="form-control" ></asp:TextBox>
                            </div>
                            <div class="form-group col-xs-11 col-sm-4 col-md-4 col-lg-4">
                                <label for="txtEmail">Email</label>
                                <asp:TextBox ID="txtEmail" runat="server" placeholder="Email" class="form-control" TextMode="Email"></asp:TextBox>
                            </div>
                  <br />
                            <div class="form-group col-xs-11 ">
                                <asp:CheckBox ID="CheckBox1" runat="server" Text=" I would like to receive information from Charlotte Radiology about Breast Services. This may include a welcome email and regular newsletters." Checked="True" />
                                </div>
                  <br />
                            <div class="form-group col-xs-11 ">
                                <asp:CheckBox ID="CheckBox2" runat="server" Text=" I would like to receive information from Charlotte Radiology about Vein Services. This may include a welcome email and regular newsletters." Checked="True" />
                                </div>
            
                     <div class="form-group col-xs-11 ">
                        <label for="btnSubmit">&nbsp;</label>
<%--                        <input type="button" class="form-control btn btn-primary" id="submit" value="Submit PIN" onclick="clkSave()">--%>
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="form-control btn btn-primary" OnClick="btnSubmit_Click" />
                   </div>
         </asp:Panel>
        <asp:XmlDataSource ID="MySource" runat="server" DataFile="~/REF_STATES.xml" 
             XPath="US_STATES/state" ></asp:XmlDataSource>
               </form>
                 <div class="clearfix">
              </div>
            </div>
        </div>


        <div class="container">
            <div class="row">
               
               
            </div>
        </div>
    </div>
    <div class="section" id="footer">
        <div class="container">
            <div class="row">
                    <div class="col-md-2">&nbsp;</div>
                <div class="col-md-8">
                
                        <p>
                            <br>
                         </p>
                </div>
            </div>
        </div>
    </div>
</div>
</div>

    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
    <script src="js/jquery.min.js"></script>
    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script src="js/bootstrap.min.js"></script>
</body>
</html>
