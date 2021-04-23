<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ElectionExplorer.aspx.cs" 
Inherits="Vote.ElectionExplorerPage" %>

<!DOCTYPE html>

<html>
<head runat="server">
  <title>Election Explorer</title>
  <script src="/js/jq/modernizr.custom.63696.js" type="text/javascript"></script> 
  <script data-main="/js/require-main.js" src="/js/require.js" type="text/javascript"></script> 
  <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.1/themes/base/jquery-ui.css" />
  <link rel="stylesheet" href="/css/vote/adminMaster.css">
</head>
<body id="body"  class="admin-page election-explorer">
  <form id="form" runat="server">
 
  <asp:ScriptManager ID="ScriptManager" runat="server"
    AjaxFrameworkMode="Enabled" AsyncPostBackTimeout="1200">
  </asp:ScriptManager>

  <div id="outer">

    <div id="container">
  
      <div class="entry">
        <img class="red-arrow step-1" src="/images/redarrow.gif" />
        <div class="col col1 rounded-border">
          <div class="col number">
            <img src="/images/1.30.png" alt="1"/>
          </div>
          <div class="col controls">
            <div class="address-controls">
              <em><asp:Label runat="server" AssociatedControlID="AddressEntry"
              Text="Find your address:" /></em>
              <asp:TextBox ID="AddressEntry" runat="server" CssClass="address-entry"/>
            <input type="button" value="Find" class="lookup-address button-2 button-smallest" />
            </div>
            <div class="or"><p><em>&mdash; OR &mdash;</em></p>
            <p>Select state, county and legislative districts:</p>
            </div>
            <div class="location-controls">
              <div class="col state-controls">
                <asp:Label runat="server" AssociatedControlID="StateList"
                  Text="State" /><br />
                <asp:DropDownList ID="StateList" CssClass="state-list" runat="server"/> 
              </div>
              <div class="col county-controls">
                <asp:Label runat="server" AssociatedControlID="CountyList"
                  Text="County" /><br />
                <asp:DropDownList ID="CountyList" CssClass="county-list" runat="server"/> 
              </div>
              <div class="clear-both"></div>
            </div>
            <div class="district-controls">
              <div class="col congress-controls">
                <asp:Label runat="server" AssociatedControlID="CongressList"
                  Text="Congress" /><br />
                <asp:DropDownList ID="CongressList" CssClass="congress-list" runat="server"/> 
              </div>
              <div class="col state-senate-controls">
                <asp:Label runat="server" AssociatedControlID="StateSenateList"
                  Text="State Senate" /><br />
                <asp:DropDownList ID="StateSenateList" CssClass="state-senate-list" runat="server"/> 
              </div>
              <div class="col state-house-controls">
                <asp:Label runat="server" AssociatedControlID="StateHouseList"
                  Text="State House" /><br />
                <asp:DropDownList ID="StateHouseList" CssClass="state-house-list" runat="server"/> 
              </div>
              <div class="clear-both"></div>
            </div>
          </div>
        </div>
        <div class="col col2">
          <div class="election-controls rounded-border">
            <div class="col number">
              <img src="/images/2.30.png" alt="2" />
            </div>
            <div class="col controls">
              <em><asp:Label runat="server" AssociatedControlID="ElectionList"
                Text="Select an election:" /></em><br />
              <asp:DropDownList ID="ElectionList" CssClass="election-list" runat="server"/> 
            </div>
            <div class="clear-both"></div>
          </div>
          <div class="clear-both"></div>
          <div class="report-controls rounded-border">
            <div class="col number">
              <img src="/images/3.30.png" alt="3" />
            </div>
            <div class="col controls">
              <input type="button" value="Get the Vote-USA Election Explorer" 
              class="get-explorer button-1 button-smaller" />
            </div>
            <div class="clear-both"></div>
          </div>
        </div>
        <div class="clear-both"></div>
      </div>
      <hr/>

      <h1 id="H1" runat="server"></h1>
      <div class="report-placeholder">
        <asp:PlaceHolder id="ReportPlaceHolder" runat="server" />
      </div>
    </div>
  </div>
  </form>
  
  <div id="select-candidates-dialog" class="hidden">
    <div class="candidate-list">
      
    </div>
    <div class="controls">
      <input type="button" value="Ok" 
        class="ok-button button-1 button-smallest" />
    </div>
  </div>
</body>
</html>
