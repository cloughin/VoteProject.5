<%@ Page Language="C#" AutoEventWireup="true" 
 MasterPageFile="~/Master/Admin.Master"
CodeBehind="ImagesHeadshots.aspx.cs" Inherits="Vote.Master.ImagesHeadshots" %>

<%@ Register TagPrefix="user" Namespace="Vote" Assembly="VoteLibrary" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
  <style type="text/css">
    body#body
    {
      margin: 0 0 10px 10px;
    }
    body table.tableAdmin {
      margin-top: 10px;
    }
  </style>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">

  <asp:UpdatePanel ID="UpdatePanel" runat="server">
    <Triggers>
       <asp:PostBackTrigger ControlID="ButtonUploadPicture" />
       <asp:PostBackTrigger ControlID="ButtonUploadPictureProfile" />
       <asp:AsyncPostBackTrigger ControlID="ButtonNext" />
       <asp:AsyncPostBackTrigger ControlID="ButtonNoCrop" />
       <asp:AsyncPostBackTrigger ControlID="ButtonAsIs" />
       <asp:AsyncPostBackTrigger ControlID="ButtonRevertToLog" />
       <asp:AsyncPostBackTrigger ControlID="RadioButtonListProcessMethod" />
    </Triggers>

    <ContentTemplate>
      
      <table class="tableAdmin" id="Top" cellspacing="6" cellpadding="0" border="0">
        <tr>
          <td align="left" class="HLarge" colspan="2">
            Image HEADSHOT Maintenance
            <asp:Label ID="LabelPolitician" runat="server" CssClass="TLargeBoldColor"></asp:Label>
            <asp:Label ID="LabelPoliticianKey" runat="server" CssClass="T"></asp:Label></td>
        </tr>
        <tr>
        <td class="T">
            <asp:Label ID="LabelOffice" runat="server" CssClass="TBold"></asp:Label>&nbsp;
          Office Level: <asp:Label ID="LabelOfficeLevel" runat="server"></asp:Label>
          <%-- &nbsp;&nbsp;Profile Uploaded:--%>
          <asp:Label ID="LabelUploadDate" runat="server" CssClass="hidden"></asp:Label>
          <asp:PlaceHolder ID="UploadDatePlaceHolder" runat="server"></asp:PlaceHolder>
        </td>
        </tr>
        <tr>
          <td align="left" colspan="2">
            <asp:Label ID="Msg" runat="server"></asp:Label></td>
        </tr>
        <tr>
          <td class="TBold">
            <asp:Label ID="LabelRows" runat="server" CssClass="TBold"></asp:Label>
            PoliticiansImages Rows</td>
        </tr>
      </table>

      <div class="TBold" style="margin: 10px 0 10px 4px;float:left;text-align:center">
        <span id="ProfileImageHeading" runat="server">Newly-Uploaded Profile:<br />&nbsp;</span><br/>
        <asp:Image ID="OriginalImage" runat="server" /><br/>
        <span id="OriginalImageSize" runat="server">{0}x{1}</span>
      </div>

      <div id="LoggedImageTable" runat="server" class="TBold" style="margin: 10px 0 10px 4px;float:left;text-align:center">
        <span id="LoggedImageHeading" runat="server">Most Recent Logged Profile<br/>({0} by {1}):</span><br />
        <asp:Image ID="LoggedImage" runat="server" /><br/>
        <span id="LoggedImageSize" runat="server">{0}x{1}</span>
      </div>
   
      <div class="T" style="margin: 10px 0 0 4px;clear:both">
      <table id="TableProfile" cellpadding="0" cellspacing="0" class="tableAdmin" runat="server">
        <tr>
          <td class="HSmall" colspan="2">
            &nbsp;Actions for this Politician</td>
        </tr>
        <tr>
          <td class="TBoldColor" colspan="2">
            <br/>When uploading BOTH a profile and headshot image, upload the profile first, because
            headshots are automatically created, thus overwriting any existing headshots.</td>
        </tr>
        <tr>
          <td class="T">
            <asp:Button ID="ButtonNext" CssClass="Buttons" runat="server" Text="Go to Next Politician Image" OnClick="ButtonNext_Click" Width="200px" /></td>
          <td class="T">
            Upload any new headshot and/or profile images below. Make sure there is an image for the
            Original Headshot. Then use this button to do the next
            politician.<br/><br/></td>
        </tr>
        <tr>
          <td align="left" class="HSmall" colspan="2">
              Profile Images</td>
        </tr>
      </table>
      </div>
    
      <div class="TBold" style="margin: 10px 0 10px 4px;float:left">
        200x275<br />
        <asp:Image ID="Image200" runat="server" />
      </div>
    
      <div class="TBold" style="margin: 10px 0 10px 4px;float:left">
        300x400<br />
        <asp:Image ID="Image300" runat="server" />
      </div>
     
      <div class="T" style="margin: 10px 0 0 4px;clear:both">
        <b>New Profile Image:</b><br />
        <input id="ImageFileProfile" runat="server" class="TextBoxInput" name="ImageFileProfile" size="95"
           type="file" /><br />
        <input id="ButtonUploadPictureProfile" runat="server" class="Buttons" name="ButtonUploadPictureProfile"
          style="width: 222px" type="submit"
          value="Upload Profile Image" onserverclick="ButtonUploadPictureProfile_ServerClick" /><br/>
        <input id="ButtonRevertToLog" runat="server" class="Buttons" name="ButtonRevertToLog"
          style="width: 222px" type="submit"
          value="Revert to Logged Profile" onserverclick="ButtonRevertToLog_ServerClick" />
      </div>

      <table id="TableHeadshot" cellpadding="0" cellspacing="0" class="tableAdmin" runat="server">
        <tr>
          <td align="left" class="HSmall" >
              Headshot Images</td>
        </tr>
      </table>
    
      <div class="TBold" style="margin: 10px 0 10px 4px;float:left">
        Max Width 15<br />
        <asp:Image ID="Image15" runat="server" />
      </div>
    
      <div class="TBold" style="margin: 10px 0 10px 4px;float:left">
        Max Width 20<br />
        <asp:Image ID="Image20" runat="server" />
      </div>
    
      <div class="TBold" style="margin: 10px 0 10px 4px;float:left">
        Max Width 25<br />
        <asp:Image ID="Image25" runat="server" />
      </div>
    
      <div class="TBold" style="margin: 10px 0 10px 4px;float:left">
        Max Width 35<br />
        <asp:Image ID="Image35" runat="server" />
      </div>
    
      <div class="TBold" style="margin: 10px 0 10px 4px;float:left">
        Max Width 50<br />
        <asp:Image ID="Image50" runat="server" />
      </div>
    
      <div class="TBold" style="margin: 10px 0 10px 4px;float:left">
        Max Width 75<br />
        <asp:Image ID="Image75" runat="server" />
      </div>
    
      <div class="TBold" style="margin: 10px 0 10px 4px;float:left">
        Max Width 100<br />
        <asp:Image ID="Image100" runat="server" />
      </div>
     
      <div class="T" style="margin: 10px 0 0 4px;clear:both">
        <b>New Headshot Image:</b><br />
        <input id="ImageFile" runat="server" class="TextBoxInput" name="ImageFile" size="95"
           type="file" /><br />
        <input id="ButtonUploadPicture" runat="server" class="Buttons" name="ButtonUploadPicture"
          style="width: 222px" type="submit"
          value="Upload Headshot Image" onserverclick="ButtonUploadPicture_ServerClick" />
        <br />
      </div>

      <table id="Table1" cellpadding="0" cellspacing="0" class="tableAdmin" runat="server">
        <tr>
          <td class="T">
        <asp:Button ID="ButtonNoCrop" CssClass="Buttons" runat="server" Text="Use Profile Without Cropping" OnClick="ButtonNoCrop_Click" Width="200px" /></td>
          <td class="T">
            The profile image will be used to make and store a new set of headshot images. Use
            the Go to Next Image Button go to the next politician.</td>
        </tr>
        <tr>
          <td class="T">
        <asp:Button ID="ButtonAsIs" CssClass="Buttons" runat="server" Text="Use Headshots As Is" OnClick="ButtonAsIs_Click" Width="200px" /></td>
          <td class="T">
            The existing headshot images will be retained and marked as created today. Use the
            Go to Next Image to
            go to the next politician.</td>
        </tr>
      </table>
 
      <table id="Table3" style="clear:both" cellpadding="0" cellspacing="0" class="tableAdmin" 
        runat="server">
        <tr>
          <td class="HSmall" colspan="2">
            &nbsp;Instructions</td>
        </tr>
        <tr><td class="T">
          This form is primarily used to create or update new or outdated headshot 
          images from existing profile images in the PoliticiansImages Table. It can also 
          be used to capture and upload newer profile images or to browse the profile and 
          headshot images in the PoliticiansImages Table. Use the radio 
          button list to use this utility in the other two ways. When you select either of these,
          additional criteria are requested.</td></tr>
      </table>

      <table id="Table2" cellpadding="0" cellspacing="0" class="tableAdmin" runat="server" border="1">
        <tr>
          <td align="left" class="HSmall" colspan="2" width="250">
            Processing Modes</td>
        </tr>
          <tr>
            <td align="left" class="T" width="250">
              <asp:RadioButtonList ID="RadioButtonListProcessMethod" runat="server" AutoPostBack="True"
                CssClass="RadioButtons" OnSelectedIndexChanged="RadioButtonListProcessMethod_SelectedIndexChanged">
                <asp:ListItem Value="New" Selected="True">New Headshots &amp; Profile Images</asp:ListItem>
                <asp:ListItem Value="State">A State Office Level Images Review</asp:ListItem>
                <asp:ListItem Value="Single">Single Politician</asp:ListItem>
              </asp:RadioButtonList></td>
            <td align="left" class="T" valign="top">
              <strong> New Headshots &amp; Profile Images Selection:</strong>
              This option processes
              ALL politicians whose profile image was uploaded
              after headshots were created for the politician or where there is a profile image but no headshots. The politicians
              are processed one at a time, in order of office importance (office level).</td>
          </tr>
          <tr>
             <td align="left" class="TBold">
               StateCode:&nbsp;
               <user:TextBoxWithNormalizedLineBreaks ID="TextBoxStateCode" runat="server" CssClass="TextBoxInput" Width="35px" OnTextChanged="TextBoxStateCode_TextChanged"></user:TextBoxWithNormalizedLineBreaks><br />
              Office Level:&nbsp;
               <user:TextBoxWithNormalizedLineBreaks ID="TextBoxOfficeLevel" runat="server" CssClass="TextBoxInput" Width="20px" OnTextChanged="TextBoxOfficeLevel_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
               <br />
               <asp:CheckBox ID="CheckBoxOutOfDateOnly" runat="server" Text="Only Missing/Old Headshots"
                 TextAlign="Left" 
                 oncheckedchanged="CheckBoxOutOfDateOnly_CheckedChanged" /></td>
             <td align="left" class="T" colspan="2" valign="top">
               <strong> State Office Level Review Selection: </strong>
              This option can be used to
               review all the PoliticianImages Table rows for a StateCode and office level. Enter
               the StateCode and Office Level. Check the checkbox
               to review only politicians with missing or old headshots. Then click Go to Next Image.</td>
          </tr>
          <tr>
            <td align="left" class="TBold">
              Politician key:<br /><user:TextBoxWithNormalizedLineBreaks ID="TextBoxPoliticianKey" runat="server" CssClass="TextBoxInput" Width="201px"></user:TextBoxWithNormalizedLineBreaks></td>
            <td align="left" class="T" valign="top">
              <strong>Single Politician Selection: </strong>
              This option allows you to view and upload images
              for a single politician. Enter the PoliticianKey in the adjacent textbox and click Next.</td>
          </tr>
        </table>
    </ContentTemplate>
  </asp:UpdatePanel>
</asp:Content>
