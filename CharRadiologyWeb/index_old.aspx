<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index_old.aspx.cs" Inherits="CharRadiologyWeb.index_old" %>


<!DOCTYPE html>

<html lang="en-US">
<head>
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Schedule an Appointment</title>
    <meta name='robots' content='noindex,nofollow' />
    <link rel='dns-prefetch' href='//fonts.googleapis.com' />
    <link rel='dns-prefetch' href='//s.w.org' />
    <link rel="alternate" type="application/rss+xml" title="Charlotte Radiology &raquo; Feed" href="http://charlotte.s1077.sureserver.com/feed/" />
    <link rel="alternate" type="application/rss+xml" title="Charlotte Radiology &raquo; Comments Feed" href="http://charlotte.s1077.sureserver.com/comments/feed/" />
    <link rel="canonical" href="http://charlotte.s1077.sureserver.com/schedule-an-appointment/" />
    <script type="text/javascript">
        window._wpemojiSettings = { "baseUrl": "https:\/\/s.w.org\/images\/core\/emoji\/12.0.0-1\/72x72\/", "ext": ".png", "svgUrl": "https:\/\/s.w.org\/images\/core\/emoji\/12.0.0-1\/svg\/", "svgExt": ".svg", "source": { "concatemoji": "http:\/\/charlotte.s1077.sureserver.com\/wp-includes\/js\/wp-emoji-release.min.js?ver=5.3.2" } };
        !function (e, a, t) { var r, n, o, i, p = a.createElement("canvas"), s = p.getContext && p.getContext("2d"); function c(e, t) { var a = String.fromCharCode; s.clearRect(0, 0, p.width, p.height), s.fillText(a.apply(this, e), 0, 0); var r = p.toDataURL(); return s.clearRect(0, 0, p.width, p.height), s.fillText(a.apply(this, t), 0, 0), r === p.toDataURL() } function l(e) { if (!s || !s.fillText) return !1; switch (s.textBaseline = "top", s.font = "600 32px Arial", e) { case "flag": return !c([127987, 65039, 8205, 9895, 65039], [127987, 65039, 8203, 9895, 65039]) && (!c([55356, 56826, 55356, 56819], [55356, 56826, 8203, 55356, 56819]) && !c([55356, 57332, 56128, 56423, 56128, 56418, 56128, 56421, 56128, 56430, 56128, 56423, 56128, 56447], [55356, 57332, 8203, 56128, 56423, 8203, 56128, 56418, 8203, 56128, 56421, 8203, 56128, 56430, 8203, 56128, 56423, 8203, 56128, 56447])); case "emoji": return !c([55357, 56424, 55356, 57342, 8205, 55358, 56605, 8205, 55357, 56424, 55356, 57340], [55357, 56424, 55356, 57342, 8203, 55358, 56605, 8203, 55357, 56424, 55356, 57340]) }return !1 } function d(e) { var t = a.createElement("script"); t.src = e, t.defer = t.type = "text/javascript", a.getElementsByTagName("head")[0].appendChild(t) } for (i = Array("flag", "emoji"), t.supports = { everything: !0, everythingExceptFlag: !0 }, o = 0; o < i.length; o++)t.supports[i[o]] = l(i[o]), t.supports.everything = t.supports.everything && t.supports[i[o]], "flag" !== i[o] && (t.supports.everythingExceptFlag = t.supports.everythingExceptFlag && t.supports[i[o]]); t.supports.everythingExceptFlag = t.supports.everythingExceptFlag && !t.supports.flag, t.DOMReady = !1, t.readyCallback = function () { t.DOMReady = !0 }, t.supports.everything || (n = function () { t.readyCallback() }, a.addEventListener ? (a.addEventListener("DOMContentLoaded", n, !1), e.addEventListener("load", n, !1)) : (e.attachEvent("onload", n), a.attachEvent("onreadystatechange", function () { "complete" === a.readyState && t.readyCallback() })), (r = t.source || {}).concatemoji ? d(r.concatemoji) : r.wpemoji && r.twemoji && (d(r.twemoji), d(r.wpemoji))) }(window, document, window._wpemojiSettings);
    </script>
    <style type="text/css">
        img.wp-smiley,
        img.emoji {
            display: inline !important;
            border: none !important;
            box-shadow: none !important;
            height: 1em !important;
            width: 1em !important;
            margin: 0 .07em !important;
            vertical-align: -0.1em !important;
            background: none !important;
            padding: 0 !important;
        }
    </style>
    <link rel='stylesheet' id='charlotte-radiology-css' href='http://charlotte.s1077.sureserver.com/wp-content/themes/genesis-charlotte-radiology/style.css?ver=1.0.8' type='text/css' media='all' />
    <link rel='stylesheet' id='wp-block-library-css' href='http://charlotte.s1077.sureserver.com/wp-includes/css/dist/block-library/style.min.css?ver=5.3.2' type='text/css' media='all' />
    <link rel='stylesheet' id='font-awesome-5-css' href='http://charlotte.s1077.sureserver.com/wp-content/plugins/bb-plugin/fonts/fontawesome/5.12.0/css/all.min.css?ver=2.3.1.1' type='text/css' media='all' />
    <link rel='stylesheet' id='charlotte-radiology-fonts-css' href='https://fonts.googleapis.com/css?family=Alegreya+Sans%3A400%2C400i%2C500%2C700%2C700i%2C800%7CPlayfair+Display%3A400%2C700%2C700i&#038;display=swap&#038;ver=1.0.8' type='text/css' media='all' />
    <link rel='stylesheet' id='dashicons-css' href='http://charlotte.s1077.sureserver.com/wp-includes/css/dashicons.min.css?ver=5.3.2' type='text/css' media='all' />
    <link rel='stylesheet' id='charlotte-radiology-gutenberg-css' href='http://charlotte.s1077.sureserver.com/wp-content/themes/genesis-charlotte-radiology/lib/gutenberg/front-end.css?ver=1.0.8' type='text/css' media='all' />
    <%--     <link href="css/bootstrap.min.css" rel="stylesheet">--%>
    <!-- Latest compiled and minified CSS -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.4.1/css/bootstrap.min.css">

    <!-- jQuery library -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>

    <!-- Popper JS -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.16.0/umd/popper.min.js"></script>

    <!-- Latest compiled JavaScript -->
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.4.1/js/bootstrap.min.js"></script>
    <link href="css/grid.css" rel="stylesheet">
    <link href="css/styles.css" rel="stylesheet">
    <style id='charlotte-radiology-gutenberg-inline-css' type='text/css'>
        .ab-block-post-grid .ab-post-grid-items h2 a:hover {
            color: #378bb0;
        }

        .site-container .wp-block-button .wp-block-button__link {
            background-color: #378bb0;
        }

        .wp-block-button .wp-block-button__link:not(.has-background),
        .wp-block-button .wp-block-button__link:not(.has-background):focus,
        .wp-block-button .wp-block-button__link:not(.has-background):hover {
            color: #ffffff;
        }

        .site-container .wp-block-button.is-style-outline .wp-block-button__link {
            color: #378bb0;
        }

            .site-container .wp-block-button.is-style-outline .wp-block-button__link:focus,
            .site-container .wp-block-button.is-style-outline .wp-block-button__link:hover {
                color: #5aaed3;
            }

        .site-container .has-small-font-size {
            font-size: 12px;
        }

        .site-container .has-normal-font-size {
            font-size: 18px;
        }

        .site-container .has-large-font-size {
            font-size: 20px;
        }

        .site-container .has-larger-font-size {
            font-size: 24px;
        }

        .site-container .has-theme-primary-color,
        .site-container .wp-block-button .wp-block-button__link.has-theme-primary-color,
        .site-container .wp-block-button.is-style-outline .wp-block-button__link.has-theme-primary-color {
            color: #378bb0;
        }

        .site-container .has-theme-primary-background-color,
        .site-container .wp-block-button .wp-block-button__link.has-theme-primary-background-color,
        .site-container .wp-block-pullquote.is-style-solid-color.has-theme-primary-background-color {
            background-color: #378bb0;
        }

        .site-container .has-theme-secondary-color,
        .site-container .wp-block-button .wp-block-button__link.has-theme-secondary-color,
        .site-container .wp-block-button.is-style-outline .wp-block-button__link.has-theme-secondary-color {
            color: #e3ec99;
        }

        .site-container .has-theme-secondary-background-color,
        .site-container .wp-block-button .wp-block-button__link.has-theme-secondary-background-color,
        .site-container .wp-block-pullquote.is-style-solid-color.has-theme-secondary-background-color {
            background-color: #e3ec99;
        }
    </style>
    <link rel='stylesheet' id='simple-social-icons-font-css' href='http://charlotte.s1077.sureserver.com/wp-content/plugins/simple-social-icons/css/style.css?ver=3.0.1' type='text/css' media='all' />
    <script type='text/javascript' src='http://charlotte.s1077.sureserver.com/wp-includes/js/jquery/jquery.js?ver=1.12.4-wp'></script>
    <script type='text/javascript' src='http://charlotte.s1077.sureserver.com/wp-includes/js/jquery/jquery-migrate.min.js?ver=1.4.1'></script>
    <script type='text/javascript' src='http://charlotte.s1077.sureserver.com/wp-content/themes/genesis-charlotte-radiology/js/cr-functions.js?ver=5.3.2'></script>
    <script type='text/javascript' src='http://charlotte.s1077.sureserver.com/wp-content/plugins/simple-social-icons/svgxuse.js?ver=1.1.21'></script>
    <link rel='https://api.w.org/' href='http://charlotte.s1077.sureserver.com/wp-json/' />
    <link rel="EditURI" type="application/rsd+xml" title="RSD" href="http://charlotte.s1077.sureserver.com/xmlrpc.php?rsd" />
    <link rel="alternate" type="application/json+oembed" href="http://charlotte.s1077.sureserver.com/wp-json/oembed/1.0/embed?url=http%3A%2F%2Fcharlotte.s1077.sureserver.com%2Fschedule-an-appointment%2F" />
    <link rel="alternate" type="text/xml+oembed" href="http://charlotte.s1077.sureserver.com/wp-json/oembed/1.0/embed?url=http%3A%2F%2Fcharlotte.s1077.sureserver.com%2Fschedule-an-appointment%2F&#038;format=xml" />
    <link rel="icon" href="http://charlotte.s1077.sureserver.com/wp-content/uploads/2020/01/favicon.ico" sizes="32x32" />
    <link rel="icon" href="http://charlotte.s1077.sureserver.com/wp-content/uploads/2020/01/favicon.ico" sizes="192x192" />
    <link rel="apple-touch-icon-precomposed" href="http://charlotte.s1077.sureserver.com/wp-content/uploads/2020/01/favicon.ico" />
    <meta name="msapplication-TileImage" content="http://charlotte.s1077.sureserver.com/wp-content/uploads/2020/01/favicon.ico" />
    <style type="text/css" id="wp-custom-css">
        #physician-category-select {
            border: 1px solid #c0bfbb;
            vertical-align: middle;
            background: transparent;
            -webkit-appearance: none;
            appearance: none;
            padding-left: 15px;
            border-radius: 0;
        }

            #physician-category-select:after {
                content: '\2304';
                font-size: 30px;
                line-height: 23px;
                padding-right: 2px;
            }

        .hours dl {
            max-width: 300px;
        }

            .hours dl dt {
                float: left !important;
            }

            .hours dl dd {
                text-align: right !important;
            }

        .location-service-select select {
            border: 1px solid #c0bfbb;
            vertical-align: middle;
            background: #ffffff;
            -webkit-appearance: none;
            appearance: none;
            padding-left: 15px;
            border-radius: 0;
        }

            .location-service-select select:after {
                content: '\2304';
                font-size: 30px;
                line-height: 23px;
                padding-right: 2px;
            }
    </style>
</head>
<body class="page-template-default page page-id-254 wp-custom-logo wp-embed-responsive header-full-width full-width-content genesis-breadcrumbs-hidden genesis-footer-widgets-visible has-no-blocks" itemscope itemtype="https://schema.org/WebPage">
    <div class="site-container">
        <ul class="genesis-skip-link">
            <li><a href="#genesis-nav-primary" class="screen-reader-shortcut">Skip to primary navigation</a></li>
            <li><a href="#genesis-content" class="screen-reader-shortcut">Skip to main content</a></li>
            <li><a href="#genesis-footer-widgets" class="screen-reader-shortcut">Skip to footer</a></li>
        </ul>
        <header class="site-header" itemscope itemtype="https://schema.org/WPHeader">
            <div class="wrap">
                <div class="title-area"><a href="http://charlotte.s1077.sureserver.com/" class="custom-logo-link" rel="home">
                    <img src="http://charlotte.s1077.sureserver.com/wp-content/uploads/2020/01/cr-logo.svg" class="custom-logo" alt="Charlotte Radiology Logo" /></a><p class="site-title">Charlotte Radiology</p>
                    <p class="site-description" itemprop="description">Experts in Imaging. Experts in patient care.</p>
                </div>
                <nav class="nav-secondary" aria-label="Secondary" itemscope itemtype="https://schema.org/SiteNavigationElement">
                    <div class="wrap">
                        <ul id="menu-top-bar" class="menu genesis-nav-menu menu-secondary js-superfish">
                            <li id="menu-item-329" class="menu-item menu-item-type-post_type menu-item-object-page menu-item-329"><a href="http://charlotte.s1077.sureserver.com/find-a-location/" itemprop="url"><span itemprop="name">Find a Location</span></a></li>
                            <li id="menu-item-330" class="menu-item menu-item-type-post_type menu-item-object-page current-menu-item page_item page-item-254 current_page_item menu-item-330"><a href="http://charlotte.s1077.sureserver.com/schedule-an-appointment/" aria-current="page" itemprop="url"><span itemprop="name">Schedule an Appointment</span></a></li>
                            <li id="menu-item-331" class="menu-item menu-item-type-custom menu-item-object-custom menu-item-331"><a href="https://mbxportal.com/land/crpa" itemprop="url"><span itemprop="name">Pay Your Bill</span></a></li>
                            <li id="menu-item-336" class="bg-blue menu-item menu-item-type-custom menu-item-object-custom menu-item-336"><a href="tel:17043673323" itemprop="url"><span itemprop="name">Call 704-367-3323</span></a></li>
                        </ul>
                    </div>
                </nav>
                <nav class="nav-primary" aria-label="Main" itemscope itemtype="https://schema.org/SiteNavigationElement" id="genesis-nav-primary">
                    <div class="wrap">
                        <ul id="menu-main" class="menu genesis-nav-menu menu-primary js-superfish">
                            <li id="menu-item-418" class="menu-item menu-item-type-post_type menu-item-object-page current-menu-ancestor current-menu-parent current_page_parent current_page_ancestor menu-item-has-children menu-item-418">
                                <a href="http://charlotte.s1077.sureserver.com/breast-services/" itemprop="url"><span itemprop="name">Breast Services</span></a>
                                <ul class="sub-menu">
                                    <li id="menu-item-293" class="menu-item menu-item-type-post_type menu-item-object-page menu-item-293"><a href="http://charlotte.s1077.sureserver.com/breast-services/breast-services-procedures/" itemprop="url"><span itemprop="name">Procedures / Treatments</span></a></li>
                                    <li id="menu-item-295" class="menu-item menu-item-type-post_type menu-item-object-page menu-item-295"><a href="http://charlotte.s1077.sureserver.com/breast-services/mammography-forms/" itemprop="url"><span itemprop="name">Forms</span></a></li>
                                    <li id="menu-item-908" class="menu-item menu-item-type-post_type menu-item-object-page menu-item-908"><a href="http://charlotte.s1077.sureserver.com/physicians/" itemprop="url"><span itemprop="name">Physicians</span></a></li>
                                    <li id="menu-item-297" class="menu-item menu-item-type-post_type menu-item-object-page menu-item-297"><a href="http://charlotte.s1077.sureserver.com/breast-services/mobile-mammography/" itemprop="url"><span itemprop="name">Mobile Mammography</span></a></li>
                                    <li id="menu-item-298" class="menu-item menu-item-type-post_type menu-item-object-page current-menu-item page_item page-item-254 current_page_item menu-item-298"><a href="http://charlotte.s1077.sureserver.com/schedule-an-appointment/" aria-current="page" itemprop="url"><span itemprop="name">Schedule an Appointment</span></a></li>
                                    <li id="menu-item-299" class="menu-item menu-item-type-post_type menu-item-object-page menu-item-299"><a href="http://charlotte.s1077.sureserver.com/find-a-location/" itemprop="url"><span itemprop="name">Find a Location</span></a></li>
                                </ul>
                            </li>
                            <li id="menu-item-436" class="menu-item menu-item-type-post_type menu-item-object-page menu-item-has-children menu-item-436">
                                <a href="http://charlotte.s1077.sureserver.com/vein-services/" itemprop="url"><span itemprop="name">Vein Services</span></a>
                                <ul class="sub-menu">
                                    <li id="menu-item-301" class="menu-item menu-item-type-post_type menu-item-object-page menu-item-301"><a href="http://charlotte.s1077.sureserver.com/vein-services/vein-services-online-risk-assessment/" itemprop="url"><span itemprop="name">Online Risk Assessment</span></a></li>
                                    <li id="menu-item-302" class="menu-item menu-item-type-post_type menu-item-object-page menu-item-302"><a href="http://charlotte.s1077.sureserver.com/vein-services/vein-services-procedures/" itemprop="url"><span itemprop="name">Procedures / Treatments</span></a></li>
                                    <li id="menu-item-303" class="menu-item menu-item-type-post_type menu-item-object-page menu-item-303"><a href="http://charlotte.s1077.sureserver.com/vein-services/vein-services-forms/" itemprop="url"><span itemprop="name">Forms</span></a></li>
                                    <li id="menu-item-307" class="menu-item menu-item-type-post_type menu-item-object-page menu-item-307"><a href="http://charlotte.s1077.sureserver.com/physicians/" itemprop="url"><span itemprop="name">Physicians</span></a></li>
                                    <li id="menu-item-311" class="menu-item menu-item-type-post_type menu-item-object-page menu-item-311"><a href="http://charlotte.s1077.sureserver.com/vein-services/vein-services-request-an-appointment/" itemprop="url"><span itemprop="name">Schedule an Appointment</span></a></li>
                                    <li id="menu-item-308" class="menu-item menu-item-type-post_type menu-item-object-page menu-item-308"><a href="http://charlotte.s1077.sureserver.com/find-a-location/" itemprop="url"><span itemprop="name">Find a Location</span></a></li>
                                </ul>
                            </li>
                            <li id="menu-item-851" class="menu-item menu-item-type-post_type menu-item-object-page menu-item-has-children menu-item-851">
                                <a href="http://charlotte.s1077.sureserver.com/vis/" itemprop="url"><span itemprop="name">Vascular Interventional Services</span></a>
                                <ul class="sub-menu">
                                    <li id="menu-item-319" class="menu-item menu-item-type-post_type menu-item-object-page menu-item-319"><a href="http://charlotte.s1077.sureserver.com/vis/what-is-interventional-radiology/" itemprop="url"><span itemprop="name">What is Interventional Radiology</span></a></li>
                                    <li id="menu-item-1031" class="menu-item menu-item-type-post_type menu-item-object-page menu-item-1031"><a href="http://charlotte.s1077.sureserver.com/vis/vis-services-procedures/" itemprop="url"><span itemprop="name">Procedures / Treatments</span></a></li>
                                    <li id="menu-item-315" class="menu-item menu-item-type-post_type menu-item-object-page menu-item-315"><a href="http://charlotte.s1077.sureserver.com/vis/vis-forms/" itemprop="url"><span itemprop="name">Forms</span></a></li>
                                    <li id="menu-item-321" class="menu-item menu-item-type-post_type menu-item-object-page menu-item-321"><a href="http://charlotte.s1077.sureserver.com/physicians/" itemprop="url"><span itemprop="name">Physicians</span></a></li>
                                    <li id="menu-item-317" class="menu-item menu-item-type-post_type menu-item-object-page menu-item-317"><a href="http://charlotte.s1077.sureserver.com/vis/vis-request-a-consult/" itemprop="url"><span itemprop="name">Schedule an Appointment</span></a></li>
                                    <li id="menu-item-320" class="menu-item menu-item-type-post_type menu-item-object-page menu-item-320"><a href="http://charlotte.s1077.sureserver.com/find-a-location/" itemprop="url"><span itemprop="name">Find a Location</span></a></li>
                                </ul>
                            </li>
                            <li id="menu-item-852" class="menu-item menu-item-type-post_type menu-item-object-page current-menu-ancestor current-menu-parent current_page_parent current_page_ancestor menu-item-has-children menu-item-852">
                                <a href="http://charlotte.s1077.sureserver.com/general-imaging/" itemprop="url"><span itemprop="name">General Imaging (CIS)</span></a>
                                <ul class="sub-menu">
                                    <li id="menu-item-324" class="menu-item menu-item-type-post_type menu-item-object-page menu-item-324"><a href="http://charlotte.s1077.sureserver.com/general-imaging/cis-services-procedures/" itemprop="url"><span itemprop="name">Procedures / Treatments</span></a></li>
                                    <li id="menu-item-323" class="menu-item menu-item-type-post_type menu-item-object-page menu-item-323"><a href="http://charlotte.s1077.sureserver.com/general-imaging/cis-forms/" itemprop="url"><span itemprop="name">Forms</span></a></li>
                                    <li id="menu-item-327" class="menu-item menu-item-type-post_type menu-item-object-page menu-item-327"><a href="http://charlotte.s1077.sureserver.com/physicians/" itemprop="url"><span itemprop="name">Physicians</span></a></li>
                                    <li id="menu-item-328" class="menu-item menu-item-type-post_type menu-item-object-page current-menu-item page_item page-item-254 current_page_item menu-item-328"><a href="http://charlotte.s1077.sureserver.com/schedule-an-appointment/" aria-current="page" itemprop="url"><span itemprop="name">Schedule an Appointment</span></a></li>
                                    <li id="menu-item-326" class="menu-item menu-item-type-post_type menu-item-object-page menu-item-326"><a href="http://charlotte.s1077.sureserver.com/find-a-location/" itemprop="url"><span itemprop="name">Find a Location</span></a></li>
                                </ul>
                            </li>
                            <li class="menu-item search-bar"></li>
                        </ul>
                    </div>
                </nav>
            </div>
        </header>
        <div class="site-inner">
            <div class="footer-widgets" id="genesis-footer-widgets-0">
                <p class="new-to-service" style="text-align: center; color: #D76E6B; font-size: 3.4rem; font-weight: 200;">
                    New to Charlotte Radiology
                    <br />
                    Breast Services?
                </p>
            </div>
            <div class="content-sidebar-wrap">
                <main class="content" id="genesis-content">
                    <%--                    <article class="post-254 page type-page status-publish entry" itemscope itemtype="https://schema.org/CreativeWork">
                        <header class="entry-header">
                            <h1 class="entry-title" itemprop="headline">Schedule an Appointment</h1>
                        </header><div class="entry-content" itemprop="text"></div>
                    </article>--%>
                    <div class="container">
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
                            <asp:Panel ID="Panel1" runat="server" CssClass="panel">
                                <div>
                                    <br />
                                    <div class="row">
                                        <div class="col-xs-11 col-sm-8 col-md-8 col-lg-8">

                                            <p>
                                                Whether looking to schedule your very first mammogram or new to the area and searching for an imaging specialist, we'd love to connect with you!  Enter your information below to sign up for e-news, women's health info & more.
                                            </p>
                                        </div>
                                    </div>
                                    <br /><br />
                                    <div class="row">

                                        <div class="form-group col-xs-11 col-sm-4 col-md-4 col-lg-4">
                                            <label for="txtFirstName">First Name</label>
                                            <asp:TextBox ID="txtFirstName" runat="server" placeholder="First Name" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="form-group col-xs-11 col-sm-4 col-md-4 col-lg-4">
                                            <label for="txtLastName">Last Name</label>
                                            <asp:TextBox ID="txtLastName" runat="server" placeholder="Last Name" class="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group col-xs-8 col-sm-4 col-md-4 col-lg-4">
                                            <label for="txtEmail">Email</label>
                                            <asp:TextBox ID="txtEmail" runat="server" placeholder="Email" class="form-control" TextMode="Email"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row">

                                        <div class="form-group col-xs-11 ">
                                            <asp:CheckBox ID="CheckBox1" runat="server" Text=" I would like to receive information from Charlotte Radiology about Breast Services. This may include a welcome email and regular newsletters." Checked="True" />
                                        </div>
                                    </div>
                                    <div class="row">

                                        <div class="form-group col-xs-11 ">
                                            <label for="btnSubmit">&nbsp;</label>
                                            <%--                        <input type="button" class="form-control btn btn-primary" id="submit" value="Submit PIN" onclick="clkSave()">--%>
                                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" Style="background-color: #D76E6B;" class="form-control btn btn-primary" OnClick="btnSubmit_Click" />
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:XmlDataSource ID="MySource" runat="server" DataFile="~/REF_STATES.xml"
                                XPath="US_STATES/state"></asp:XmlDataSource>
                        </form>
                    </div>
                </main>
            </div>
        </div>
        <div class="footer-widgets" id="genesis-footer-widgets">
            <h2 class="genesis-sidebar-title screen-reader-text">Footer</h2>
            <div class="wrap">
                <div class="widget-area footer-widgets-1 footer-widget-area">
                    <section id="text-2" class="widget widget_text">
                        <div class="widget-wrap">
                            <div class="textwidget">
                                <p><a href="http://charlotte.s1077.sureserver.com/">
                                    <img class="alignnone wp-image-835" src="http://charlotte.s1077.sureserver.com/wp-content/uploads/2020/01/cr-logo.svg" alt="Charlotte Radiology Logo" width="182" /></a></p>
                            </div>
                        </div>
                    </section>
                    <section id="simple-social-icons-2" class="widget simple-social-icons">
                        <div class="widget-wrap">
                            <ul class="aligncenter">
                                <li class="ssi-linkedin"><a href="http://www.linkedin.com/company/charlotte-radiology" target="_blank" rel="noopener noreferrer">
                                    <svg role="img" class="social-linkedin" aria-labelledby="social-linkedin-2"><title id="social-linkedin-2">LinkedIn</title>
                                        <use xlink:href="http://charlotte.s1077.sureserver.com/wp-content/plugins/simple-social-icons/symbol-defs.svg#social-linkedin"></use></svg></a></li>
                                <li class="ssi-twitter"><a href="https://twitter.com/CLTRadiology" target="_blank" rel="noopener noreferrer">
                                    <svg role="img" class="social-twitter" aria-labelledby="social-twitter-2"><title id="social-twitter-2">Twitter</title>
                                        <use xlink:href="http://charlotte.s1077.sureserver.com/wp-content/plugins/simple-social-icons/symbol-defs.svg#social-twitter"></use></svg></a></li>
                                <li class="ssi-instagram"><a href="https://www.instagram.com/cltradiology/" target="_blank" rel="noopener noreferrer">
                                    <svg role="img" class="social-instagram" aria-labelledby="social-instagram-2"><title id="social-instagram-2">Instagram</title>
                                        <use xlink:href="http://charlotte.s1077.sureserver.com/wp-content/plugins/simple-social-icons/symbol-defs.svg#social-instagram"></use></svg></a></li>
                                <li class="ssi-facebook"><a href="https://www.facebook.com/CharlotteRadiology" target="_blank" rel="noopener noreferrer">
                                    <svg role="img" class="social-facebook" aria-labelledby="social-facebook-2"><title id="social-facebook-2">Facebook</title>
                                        <use xlink:href="http://charlotte.s1077.sureserver.com/wp-content/plugins/simple-social-icons/symbol-defs.svg#social-facebook"></use></svg></a></li>
                                <li class="ssi-youtube"><a href="https://www.youtube.com/user/CharlotteRadiology" target="_blank" rel="noopener noreferrer">
                                    <svg role="img" class="social-youtube" aria-labelledby="social-youtube-2"><title id="social-youtube-2">YouTube</title>
                                        <use xlink:href="http://charlotte.s1077.sureserver.com/wp-content/plugins/simple-social-icons/symbol-defs.svg#social-youtube"></use></svg></a></li>
                            </ul>
                        </div>
                    </section>
                </div>
                <div class="widget-area footer-widgets-2 footer-widget-area">
                    <section id="nav_menu-2" class="widget widget_nav_menu">
                        <div class="widget-wrap">
                            <div class="menu-footer-1-container">
                                <ul id="menu-footer-1" class="menu">
                                    <li id="menu-item-343" class="menu-item menu-item-type-post_type menu-item-object-page menu-item-343"><a href="http://charlotte.s1077.sureserver.com/find-a-location/" itemprop="url">Find a Location</a></li>
                                    <li id="menu-item-344" class="menu-item menu-item-type-post_type menu-item-object-page current-menu-item page_item page-item-254 current_page_item menu-item-344"><a href="http://charlotte.s1077.sureserver.com/schedule-an-appointment/" aria-current="page" itemprop="url">Schedule an Appointment</a></li>
                                    <li id="menu-item-346" class="menu-item menu-item-type-custom menu-item-object-custom menu-item-346"><a href="https://mbxportal.com/land/crpa" itemprop="url">Pay Your Bill</a></li>
                                    <li id="menu-item-850" class="menu-item menu-item-type-post_type menu-item-object-page menu-item-850"><a href="http://charlotte.s1077.sureserver.com/insurance-information/" itemprop="url">Insurance Information</a></li>
                                </ul>
                            </div>
                        </div>
                    </section>
                </div>
                <div class="widget-area footer-widgets-3 footer-widget-area">
                    <section id="nav_menu-3" class="widget widget_nav_menu">
                        <div class="widget-wrap">
                            <div class="menu-footer-2-container">
                                <ul id="menu-footer-2" class="menu">
                                    <li id="menu-item-417" class="menu-item menu-item-type-post_type menu-item-object-page menu-item-417"><a href="http://charlotte.s1077.sureserver.com/breast-services/" itemprop="url">Breast Services</a></li>
                                    <li id="menu-item-348" class="menu-item menu-item-type-post_type menu-item-object-page menu-item-348"><a href="http://charlotte.s1077.sureserver.com/vein-services/vein-services-procedures/" itemprop="url">Vein Services</a></li>
                                    <li id="menu-item-349" class="menu-item menu-item-type-post_type menu-item-object-page menu-item-349"><a href="http://charlotte.s1077.sureserver.com/vis/vis-services-procedures/" itemprop="url">Vascular Interventional Services</a></li>
                                    <li id="menu-item-352" class="menu-item menu-item-type-post_type menu-item-object-page menu-item-352"><a href="http://charlotte.s1077.sureserver.com/general-imaging/cis-services-procedures/" itemprop="url">General Imaging</a></li>
                                </ul>
                            </div>
                        </div>
                    </section>
                </div>
                <div class="widget-area footer-widgets-4 footer-widget-area">
                    <section id="nav_menu-4" class="widget widget_nav_menu">
                        <div class="widget-wrap">
                            <div class="menu-footer-3-container">
                                <ul id="menu-footer-3" class="menu">
                                    <li id="menu-item-853" class="menu-item menu-item-type-post_type menu-item-object-page menu-item-853"><a href="http://charlotte.s1077.sureserver.com/about-us/" itemprop="url">About Us</a></li>
                                    <li id="menu-item-857" class="menu-item menu-item-type-post_type menu-item-object-page menu-item-857"><a href="http://charlotte.s1077.sureserver.com/referring-physicians/" itemprop="url">Referring Physicians</a></li>
                                    <li id="menu-item-860" class="menu-item menu-item-type-post_type menu-item-object-page menu-item-860"><a href="http://charlotte.s1077.sureserver.com/contact-us/" itemprop="url">Contact Us</a></li>
                                    <li id="menu-item-855" class="menu-item menu-item-type-post_type menu-item-object-page menu-item-855"><a href="http://charlotte.s1077.sureserver.com/careers/" itemprop="url">Careers</a></li>
                                    <li id="menu-item-856" class="menu-item menu-item-type-post_type menu-item-object-page menu-item-856"><a href="http://charlotte.s1077.sureserver.com/faq/" itemprop="url">FAQ</a></li>
                                    <li id="menu-item-854" class="menu-item menu-item-type-post_type menu-item-object-page menu-item-854"><a href="http://charlotte.s1077.sureserver.com/additional-resources/" itemprop="url">Additional Resources</a></li>
                                </ul>
                            </div>
                        </div>
                    </section>
                </div>
            </div>
        </div>
        <footer class="site-footer" itemscope itemtype="https://schema.org/WPFooter">
            <div class="wrap">
                <p>&#x000A9;&nbsp;2020 <a href="https://www.charlotteradiology.com/">Charlotte Radiology</a></p>
            </div>
        </footer>
    </div>
    <style type="text/css" media="screen">
        #simple-social-icons-2 ul li a, #simple-social-icons-2 ul li a:hover, #simple-social-icons-2 ul li a:focus {
            background-color: #f4f4f2 !important;
            border-radius: 0px;
            color: #378bb0 !important;
            border: 0px #ffffff solid !important;
            font-size: 25px;
            padding: 13px;
        }

            #simple-social-icons-2 ul li a:hover, #simple-social-icons-2 ul li a:focus {
                background-color: #f4f4f2 !important;
                border-color: #ffffff !important;
                color: #323232 !important;
            }

            #simple-social-icons-2 ul li a:focus {
                outline: 1px dotted #f4f4f2 !important;
            }
    </style>
    <script type='text/javascript' src='http://charlotte.s1077.sureserver.com/wp-includes/js/hoverIntent.min.js?ver=1.8.1'></script>
    <script type='text/javascript' src='http://charlotte.s1077.sureserver.com/wp-content/themes/genesis/lib/js/menu/superfish.min.js?ver=1.7.10'></script>
    <script type='text/javascript' src='http://charlotte.s1077.sureserver.com/wp-content/themes/genesis/lib/js/menu/superfish.args.min.js?ver=3.2.1'></script>
    <script type='text/javascript' src='http://charlotte.s1077.sureserver.com/wp-content/themes/genesis/lib/js/skip-links.min.js?ver=3.2.1'></script>
    <script type='text/javascript'>
        /* <![CDATA[ */
        var genesis_responsive_menu = { "mainMenu": "Menu", "menuIconClass": "dashicons-before dashicons-menu", "subMenu": "Submenu", "subMenuIconClass": "dashicons-before dashicons-arrow-down-alt2", "menuClasses": { "others": [".nav-primary"] } };
/* ]]> */
    </script>
    <script type='text/javascript' src='http://charlotte.s1077.sureserver.com/wp-content/themes/genesis/lib/js/menu/responsive-menus.min.js?ver=1.1.3'></script>
    <script type='text/javascript' src='http://charlotte.s1077.sureserver.com/wp-includes/js/wp-embed.min.js?ver=5.3.2'></script>
</body>
</html>
