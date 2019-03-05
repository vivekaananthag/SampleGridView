<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SampleGridView.aspx.cs" Inherits="SampleGridView" %>

<%@ Register TagPrefix="GvUc" TagName="GvUserControl" Src="~/SampleGridViewUserControl.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form runat="server">
        <GvUc:GvUserControl runat="server" ID="GridViewSampleUserControl" />
    </form>
</body>
</html>
