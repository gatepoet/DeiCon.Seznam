<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SocketTest._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<script type="text/javascript" src="http://www.google.com/jsapi"></script>
<script type="text/javascript">    google.load("jquery", "1.4.2"); </script>
<script src="Scripts/jqtouch.min.js" type="application/x-javascript" charset="utf-8"></script>
<style type="text/css" media="screen">@import "jqtouch/jqtouch.min.css";</style>
<style type="text/css" media="screen">@import "themes/jqt/theme.min.css";</style>
    <title></title>
    <script type="text/javascript" charset="utf-8">
        var jQT = new $.jQTouch({
            icon: 'jqtouch.png',
            addGlossToIcon: true,
            startupScreen: 'jqt_startup.png',
            statusBar: 'black-translucent',
            preloadImages: [
                'themes/jqt/img/back_button.png',
                'themes/jqt/img/back_button_clicked.png',
                'themes/jqt/img/button_clicked.png',
                'themes/jqt/img/grayButton.png',
                'themes/jqt/img/whiteButton.png',
                'themes/jqt/img/loading.gif'
                ]
        });
    </script>
</head>
<body>
        <div id="home" class="current"> 
            <div class="toolbar"> 
                <h1>Seznam</h1> 
                <a class="button slideup" id="infoButton" href="#about">About</a> 
            </div>
            <div class="info">
                Logged in as: User1
            </div>
            <ul class="rounded"> 
                <li class="arrow"><a href="#ui">My lists</a> <small class="counter">4</small></li> 
                <li class="arrow"><a href="#animations">Shared Llsts</a> <small class="counter">8</small></li> 
                <li class="arrow"><a href="#ajax">AJAX</a> <small class="counter">3</small></li> 
                <li class="arrow"><a href="#callbacks">Callback Events</a> <small class="counter">3</small></li> 
                <li class="arrow"><a href="#extensions">Extensions</a> <small class="counter">4</small></li> 
                <li class="arrow"><a href="#demos">Demos</a> <small class="counter">2</small></li> 
            </ul> 
            <h2>External Links</h2> 
            <ul class="rounded"> 
                <li class="forward"><a href="http://www.jqtouch.com/" target="_blank">Homepage</a></li> 
                <li class="forward"><a href="http://www.twitter.com/jqtouch" target="_blank">Twitter</a></li> 
                <li class="forward"><a href="http://code.google.com/p/jqtouch/w/list" target="_blank">Google Code</a></li> 
            </ul> 
        </div> 
        <form id="login" action="login.aspx" method="POST" class="form">
            <div class="toolbar">
                <h1>Log in</h1>
                <a class="back" href="#">Ajax</a>
            </div>
            <ul class="rounded">
                <li><input type="text" name="zip" value="" placeholder="Zip Code" /></li>
            </ul>
            <a style="margin:0 10px;color:rgba(0,0,0,.9)" href="#" class="submit whiteButton">Submit</a>
        </form>
</body>
</html>
