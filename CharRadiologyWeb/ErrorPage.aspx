<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ErrorPage.aspx.cs" Inherits="CharRadiologyWeb.ErrorPage" %>

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
    <form id="form1" runat="server">
<div class=”banner”>   
    <div class=”hero”>     
     <div id="header" class="jumbotron d-flex align-items-end">
      <div class="container">
                    
        <div class="row" style="padding-bottom: 20%;">
          <div class="col-md-12">
          </div>
        </div>
        <div class="row">
            <div class="col-xs-12 text-center">
               
      <div class="alert alert-danger" role="alert">
     
            <asp:Label ID="lblErrorMessage" runat="server" Text="Error Message" style="font-size: small; color: #CC0000"></asp:Label>
            </div>

<p class="text-center">Click the button to return to the Main Menu. Refresh your browser and clear your cache if the error continues.<br>
</p>

       </div>
    </div>
       
       <div class="row rowspace30">
    <div class="col-xs-12" style="text-align:center">
            <img class="img" src="images/bsod.jpg" alt="BSOD">

            <asp:HyperLink ID="HyperLink1" CssClass="btn btn-brown" runat="server" NavigateUrl="index.aspx">Click To Go Back To Main Menu</asp:HyperLink>
    </div>
            </div>
        </div>
      </div>
    </div>
<div id="links">&nbsp;</div>
    <div id="info">
      
        <div class="container">
            <div class="row">
               
                     <div class="col-md-12">
            <h3 style="color:white"><strong>Terms and Conditions <br>
                    
                        Additional text lorum ipsum.....</strong> </h3>
            
                </div>
                    
                
                
                
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
                  
                
                        <p> </p>
                </div>
            </div>
        </div>
    </div>

</div>
    </form>
</body>
</html>
