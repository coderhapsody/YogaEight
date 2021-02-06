<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintPreview.aspx.cs" Inherits="GoGym.FrontEnd.Reports.PrintPreview" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        a
        {
            text-decoration: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scmScriptManager" runat="server" />
    <div>
        <rsweb:ReportViewer ID="rptReport" runat="server" Font-Names="Verdana" ShowParameterPrompts="false"
            Font-Size="8pt" ProcessingMode="Remote" SizeToReportContent="true" KeepSessionAlive="true" HyperlinkTarget="_blank" 
            PageCountMode="Actual" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" 
            InteractivityPostBackMode="AlwaysAsynchronous" AsyncRendering="false"
            Width="100%" Height="100%" >
        </rsweb:ReportViewer>
    </div>
    </form>
</body>
</html>
