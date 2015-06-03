<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChageOfBitsTest.aspx.cs" Inherits="ImageStenography.ChageOfBitsTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            Go from a number in 8 bits to a number in some other amount of bits.
            <br />
            <br />
            Original Number
            <asp:TextBox runat="server" ID="inputNumberConcealing"></asp:TextBox>
            <br />
            Number to Hide
            <asp:TextBox runat="server" ID="inputNumberConcealed"></asp:TextBox>
            <br />
            Number of bits: 
                <asp:DropDownList ID="encodingBits" runat="server">
                    <asp:ListItem Text="1" Value="1"></asp:ListItem>
                    <asp:ListItem Text="2" Value="2"></asp:ListItem>
                    <asp:ListItem Text="3" Value="3"></asp:ListItem>
                    <asp:ListItem Text="4" Value="4"></asp:ListItem>
                    <asp:ListItem Text="5" Value="5"></asp:ListItem>
                    <asp:ListItem Text="6" Value="6"></asp:ListItem>
                    <asp:ListItem Text="7" Value="7"></asp:ListItem>
                    <asp:ListItem Text="8" Value="8"></asp:ListItem>
                </asp:DropDownList>
            <br />
            <asp:Button ID="convertButton" runat="server" Text="Convert" OnClick="convertBits" />
            <br />
            Truncated orignial number:
            <asp:TextBox ID="conceleaingNumberOut" runat="server"></asp:TextBox>
            Compresssed hidden number:
            <asp:TextBox ID="concealedNumberOut" runat="server"></asp:TextBox>
            Compressed hidden number recovered:
            <asp:TextBox ID="concealedNumberRecoveredOut" runat="server"></asp:TextBox>
            <br />
            Output:
            <asp:TextBox ID="outputBox" runat="server"></asp:TextBox>

        </div>
    </form>
</body>
</html>
