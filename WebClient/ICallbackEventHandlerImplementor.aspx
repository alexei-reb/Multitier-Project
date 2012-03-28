<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ICallbackEventHandlerImplementor.aspx.cs"
    Inherits="WebClient.ICallbackEventHandlerImplementor" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title></title>
    <script language="javascript" type="text/javascript">
        function btnPageMethodInvocation_clientHandler(request) {
            serverCall(request);
        }

        function onSuccessfullHandler(responce) {
            document.getElementById("content").innerHTML = responce;
        }

        function connect() {
            var ip = document.getElementById("ipAddress").value;
            var port = document.getElementById("port").value;
            serverCall("GetFullData|" + ip + "|" + port);
        }

        function getImage() {
            var id = document.getElementById("imageID").value;
             var ip = document.getElementById("ipAddress").value;
            var port = document.getElementById("port").value;
            serverCall("GetImage|" + ip + "|" + port + "|" + id);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="ConnectDiv">
        Server IP address:
        <input id="ipAddress" type="text" value="192.168.56.1" /><br />
        Server Port number:
        <input id="port" type="text" value="8008" />
        <br />
        <input type="button" value="Connect" onclick="connect()" /><br />
        <input type="button" value="GetImage" onclick="getImage()" />
        <input type="text" id="imageID" />
    </div>
    <div id="content">
    </div>
    </form>
</body>
</html>
