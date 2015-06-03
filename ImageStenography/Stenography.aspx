<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Stenography.aspx.cs" Inherits="ImageStenography.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Image Stenography</title>
</head>
<body>
    Upload an image to obtain the concealed image or upload to images to conceal one inside the other.
    
    <br /><br />
    <form id="form1" runat="server">
        

        <div>
            <!--Select a main image to conceal an image in or recover a concealed image -->
            Image: <asp:TextBox ID="concealingImageURLTextBox" runat="server" AutoPostBack="true" OnTextChanged="concealingImageURLAdded"></asp:TextBox> <br />
            OR <asp:FileUpload id="concealingFileUpload" runat="server" /> <br />
            <!--<asp:Button ID="concealingImageUploadButton" runat="server" Text="Upload" OnClick="UploadButtonPressed" />  -->
            <asp:Label ID="concealingImageUploadedText" Text="upload an image" runat="server"></asp:Label>
            
        </div>
            <br />
        <asp:Panel ID="secondImageUploadPanel" runat="server" Visible="false">
            <!--Upload the image to conceal. Only visible when the conceal option is selected -->
            Image to conceal: <asp:TextBox ID="concealedImageURL" runat="server" AutoPostBack="true" OnTextChanged="concealedImageURLAdded"></asp:TextBox> <br />
            OR <asp:FileUpload id="concealedFileUpload" runat="server" /> <br />
            <!--<asp:Button ID="concealedImageUploadButton" runat="server" Text="Upload" OnClick="UploadButtonPressed" /> -->
            <asp:Label ID="concealedImageUploadedText" Text="upload an image" runat="server"></asp:Label> 
            
        </asp:Panel>
        <br />
        <div>
            <!--Select whether to conceal or recover an image -->
            <asp:RadioButtonList ID="typeOfOperation" runat="server" OnSelectedIndexChanged="optionSelectedChanged" AutoPostBack="true" >
                <asp:ListItem Text="Conceal an Image" Value="conceal" Enabled="true"></asp:ListItem>
                <asp:ListItem Text="Recover an image" Value="recover" Enabled="true"></asp:ListItem>
                <asp:ListItem Text="See how an image will look when concealed" Vale="test" Enabled="true"></asp:ListItem>
            </asp:RadioButtonList>
            <br />
            <asp:CheckBox ID="normalizeOption" Text="Normalize" runat="server"  />
        </div>
        
        <br />
        <div>
            <!--Select the number of bits the concealed image should take up in each color -->
            With the concealed images represnted by the 
            <asp:DropDownList ID="encodingBits" runat="server">
                <asp:ListItem Text="0" Value="0"></asp:ListItem>
                <asp:ListItem Text="1" Value="1"></asp:ListItem>
                <asp:ListItem Text="2" Value="2"></asp:ListItem>
                <asp:ListItem Text="3" Value="3"></asp:ListItem>
                <asp:ListItem Text="4" Value="4"></asp:ListItem>
                <asp:ListItem Text="5" Value="5"></asp:ListItem>
                <asp:ListItem Text="6" Value="6"></asp:ListItem>
                <asp:ListItem Text="7" Value="7"></asp:ListItem>
                <asp:ListItem Text="8" Value="8"></asp:ListItem>
            </asp:DropDownList>
            least significant bit(s) of the main image.
            <br />
            (note: more than 3 bits is not recommended)
        </div>
        <br />
        <div>
            <asp:Button ID="executeMainProgram" runat="server" Text="GO!" OnClick="UploadButtonPressed" />
        </div>
        <br />

        <!-- Test deleting of uploaded files.
        <asp:Button ID="uploadButton" runat="server" Text="Upload Images" OnClick="UploadButtonPressed" />
        <asp:Button ID="tempDeleteImages" runat="server" Text="Delete uploaded images" OnClick="disposeOfUploadedImages"  Visible="true"/>
        Test of images via url -->
        <div>
            <asp:Image ID="imageTest" runat="server" ImageUrl="http://i.imgur.com/HLEmek1.jpg" Width="300" Height="500" />
            <asp:Image ID="imageTwo" runat="server" ImageUrl="http://i.imgur.com/HLEmek1.jpg" Width="300" Height="500" />
        </div>
        
    </form>
   
</body>
</html>
