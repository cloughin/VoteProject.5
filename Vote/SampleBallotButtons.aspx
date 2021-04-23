<%@ Page Language="C#" MasterPageFile="~/Public.Master" AutoEventWireup="true" CodeBehind="SampleBallotButtons.aspx.cs" Inherits="Vote.SampleBallotButtonsPage" %>

<%@ Register Src="/Controls/PageHeadingResponsive.ascx" TagName="PageHeading" TagPrefix="user" %>
<%@ Register Src="/Controls/SampleBallotButton1.ascx" TagName="SampleBallotButton1" TagPrefix="user" %>
<%@ Register Src="/Controls/SampleBallotButton2.ascx" TagName="SampleBallotButton2" TagPrefix="user" %>
<%@ Register Src="/Controls/SampleBallotButton3.ascx" TagName="SampleBallotButton3" TagPrefix="user" %>
<%@ Register Src="/Controls/SampleBallotButton4.ascx" TagName="SampleBallotButton4" TagPrefix="user" %>
<%@ Register Src="/Controls/SampleBallotButton5.ascx" TagName="SampleBallotButton5" TagPrefix="user" %>
<%@ Register Src="/Controls/SampleBallotButton6.ascx" TagName="SampleBallotButton6" TagPrefix="user" %>
<%@ Register Src="/Controls/SampleBallotButton7.ascx" TagName="SampleBallotButton7" TagPrefix="user" %>
<%@ Register Src="/Controls/SampleBallotButton8.ascx" TagName="SampleBallotButton8" TagPrefix="user" %>
<%@ Register Src="/Controls/SampleBallotButton9.ascx" TagName="SampleBallotButton9" TagPrefix="user" %>

<asp:Content ID="HeadTopContent" ContentPlaceHolderID="MasterHeadTopContent" runat="server">
</asp:Content>

<asp:Content ID="HeadBottomContent" ContentPlaceHolderID="MasterHeadBottomContent" runat="server">
  <style type="text/css">
    
    .content .intro {
      padding-left: 20px;
    }
    .button-box
    {
      padding: 15px 20px;
      background: #F4F4F4;
      border: 1px solid #DFDFDF;
      border-radius: 4px;
      -moz-border-radius: 4px;
      -webkit-border-radius: 4px;
    }
    .selection-box
    {
      width: 700px;
    }
    .button-box p
    {
      font-family: "HelveticaNeueBold","HelveticaNeue-Bold","Helvetica Neue",Helvetica,"open sans",arial,sans-serif;
      font-size: 12px;
      font-style: normal;
      font-weight: 700;
      color: black;
      margin: 0 !important;
    }
    .button-group
    {
      float: left;
    }
    .button-group-2,
    .button-group-3
    {
      margin-left: 24px;
    }
    .style-x
    {
      margin-top: 26px;
    }
    .radio-div
    {
      float: left;
      width: 30px;
      height: 30px;
    }
    .sample-button
    {
      float: left;
    }
    .code-box
    {
      margin-top:10px;
      padding: 16px 20px;
      float: left;
    }
    #code-area
    {
      max-width: 515px;
      height: 178px;
      background: #FCF8E3;
      margin: 10px 0;
      font-size: 13px;     
      font-family: tahoma,mono,sans-serif;
      border: 1px solid #CCC;
      border-radius: 4px;
      -moz-border-radius: 4px;
      -webkit-border-radius: 4px;
      line-height: 1.5em;
      padding: 4px;
      outline: none;
      box-shadow: 0 3px 5px #000000 inset;
      box-shadow: 0 3px 5px rgba(0, 0, 0, 0.05) inset;
      -moz-box-shadow: 0 3px 5px #000000 inset;
      -moz-box-shadow: 0 3px 5px rgba(0,0,0,0.05) inset;
      -webkit-box-shadow: 0 3px 5px #000000 inset;
      -webkit-box-shadow: 0 3px 5px rgba(0, 0, 0, 0.05) inset;
    }

    @media only screen and (max-width : 759px) 
    {
      .selection-box {
        width: 280px;
                     }
      .button-group-2, .button-group-3 {
        margin-left: 0;
                                       }    
    }

    @media only screen and (max-width : 400px) 
    {
      .content .intro {
        padding-left: 0;
                                       }    
    }
  /* small & medium*/
  </style>
  <script type="text/javascript">
    function findLineEnder(str) {
      // returns either '\r\n', '\n', '\r' or null (if none found)
      if (str.indexOf('\r\n') >= 0) return '\r\n';
      if (str.indexOf('\n') >= 0) return '\n';
      if (str.indexOf('\r') >= 0) return '\r';
      return null;
    }

    function adjustFormatting(str) {
      var lineEnder = findLineEnder(str);
      var lines;
      if (lineEnder) {
        // force comment onto new line
        str = str.replace(/<!--/g, lineEnder + '<!--');
        lines = str.split(lineEnder);
      }
      else // treat as single long line
        lines = [str];
      // trim and eliminate empty lines, from end so we can still index
      for (var n = lines.length - 1; n >= 0; n--) {
        lines[n] = $.trim(lines[n]);
        if (!lines[n]) lines.splice(n, 1);
      }
      return lines.join(lineEnder);
    }

    function setRadio(obj) {
      var id = obj.parentElement.id;
      id = id.replace("sample-button", "radio-style");
      $("#" + id).click();
      return false;
    }

    function modifyButtons() {
      $('.vote-usa-sample-ballot-button').click(function () { return setRadio(this); });
      $('.vote-usa-compare-candidates-button').click(function () { return setRadio(this); });
      $('.vote-usa-logo-button').click(function () { return setRadio(this); });
    }

    $(function () {
      // Attach events to radios
      $('.radios').click(function (/*e*/) {
        var id = this.id;
        var inx = id.substr(id.length - 1, 1);
        var codeElement = $('#sample-button-' + inx);
        $('#code-area').val(adjustFormatting(codeElement.html())).select();
      });
      // default to first button
      $('#radio-style-1').click();
      modifyButtons();
    });
  </script>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <h1>Get Buttons for Our Interactive Ballot Choices</h1>

  <div class="intro">
    <p>Help your visitors get the voter information they need by pasting one of these buttons into your site.</p>
    <div class="selection-box button-box">
      <p>Select a button style</p>
      <div class="button-group button-group-1">
        <div class="style-1 style-x">
          <div class="radio-div">
            <input id="radio-style-1" class="radios" name="radios" type="radio" />
          </div>
          <div id="sample-button-1" class="sample-button">
            <user:SampleBallotButton1 ID="SampleBallotButton1" runat="server" />
          </div>
        </div>
        <div style="clear:both"></div>
        <div class="style-2 style-x">
          <div class="radio-div">
            <input id="radio-style-2" class="radios" name="radios" type="radio" />
          </div>
          <div id="sample-button-2" class="sample-button">
            <user:SampleBallotButton2 ID="SampleBallotButton2" runat="server" />
          </div>
        </div>
        <div style="clear:both"></div>
        <div class="style-3 style-x">
          <div class="radio-div">
            <input id="radio-style-3" class="radios" name="radios" type="radio" />
          </div>
          <div id="sample-button-3" class="sample-button">
            <user:SampleBallotButton3 ID="SampleBallotButton3" runat="server" />
          </div>
        </div>
      </div>
      <div class="button-group button-group-2">
        <div class="style-1 style-x">
          <div class="radio-div">
            <input id="radio-style-4" class="radios" name="radios" type="radio" />
          </div>
          <div id="sample-button-4" class="sample-button">
            <user:SampleBallotButton4 ID="SampleBallotButton4" runat="server" />
          </div>
        </div>
        <div style="clear:both"></div>
        <div class="style-2 style-x">
          <div class="radio-div">
            <input id="radio-style-5" class="radios" name="radios" type="radio" />
          </div>
          <div id="sample-button-5" class="sample-button">
            <user:SampleBallotButton5 ID="SampleBallotButton5" runat="server" />
          </div>
        </div>
        <div style="clear:both"></div>
        <div class="style-3 style-x">
          <div class="radio-div">
            <input id="radio-style-6" class="radios" name="radios" type="radio" />
          </div>
          <div id="sample-button-6" class="sample-button">
            <user:SampleBallotButton6 ID="SampleBallotButton6" runat="server" />
          </div>
        </div>
      </div>
      <div class="button-group button-group-3">
        <div class="style-1 style-x">
          <div class="radio-div">
            <input id="radio-style-7" class="radios" name="radios" type="radio" />
          </div>
          <div id="sample-button-7" class="sample-button">
            <user:SampleBallotButton7 ID="SampleBallotButton7" runat="server" />
          </div>
        </div>
        <div style="clear:both"></div>
        <div class="style-2 style-x">
          <div class="radio-div">
            <input id="radio-style-8" class="radios" name="radios" type="radio" />
          </div>
          <div id="sample-button-8" class="sample-button">
            <user:SampleBallotButton8 ID="SampleBallotButton8" runat="server" />
          </div>
        </div>
        <div style="clear:both"></div>
        <div class="style-3 style-x">
          <div class="radio-div">
            <input id="radio-style-9" class="radios" name="radios" type="radio" />
          </div>
          <div id="sample-button-9" class="sample-button">
            <user:SampleBallotButton9 ID="SampleBallotButton9" runat="server" />
          </div>
        </div>
      </div>
      <div style="clear:both"></div>
    </div>
    <div style="clear:both"></div>
    <div class="code-box button-box">
      <p>Copy and paste this code into your page. The button will appear wherever you place the code.</p>
      <textarea id="code-area" rows="5" wrap="off" onclick="select()"></textarea>
    </div>
    <div style="clear:both"></div>
  </div>
</asp:Content>

<asp:Content ID="BottomContent" ContentPlaceHolderID="MasterBottomContent" runat="server">
</asp:Content>
