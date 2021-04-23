<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Admin.Master" AutoEventWireup="true" 
CodeBehind="UpdateUsers.aspx.cs" Inherits="Vote.Master.UpdateUsersPage" %>

<%@ Register TagPrefix="user" Namespace="Vote" Assembly="VoteLibrary" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  
  <div id="outer">
    <h1 id="H1" EnableViewState="false" runat="server"></h1>
    
    <div id="UpdateControls" class="update-controls" runat="server">
      
      <div id="main-tabs" class="tab-control htab-control jqueryui-tabs start-hidden shadow">
        
        <ul class="main-tabs tabs htabs unselectable">
          <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-adduser" onclick="this.blur()" id="TabAddUser" EnableViewState="false" runat="server">Add<br />Users</a></li>
          <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-changeuser" onclick="this.blur()" id="TabChangeUser" EnableViewState="false" runat="server">Change Password/<br />Delete User</a></li>
        </ul>
        
        <div id="tab-adduser" class="main-tab content-panel tab-panel htab-panel">
          
          <div class="inner-panel rounded-border">
            <div class="tab-panel-heading horz-center">
              <div class="center-inner">
                <h4 class="center-element">Add a User</h4>
              </div>
            </div>
            <hr/>
            <div class="content-area">
              <div class="data-area">
 
                <div class="input-element user-name">
                  <p class="fieldlabel">User Name <span class="reqd">◄</span></p>
                  <div class="databox textbox">
                    <user:TextBoxWithNormalizedLineBreaks ID="AddUsersUserName" spellcheck="false" 
                      CssClass="textbox-user-name shadow-2" runat="server"/>
                  </div>                
                </div>
 
                <div class="input-element user-password">
                  <p class="fieldlabel">Password <span class="reqd">◄</span></p>
                  <div class="databox textbox">
                    <user:TextBoxWithNormalizedLineBreaks ID="AddUsersUserPassword" spellcheck="false" 
                      CssClass="textbox-user-password shadow-2" runat="server"/>
                  </div>                
                </div>
                
                <div class="clear-both"></div>
 
                <div class="input-element user-security">
                  <p class="fieldlabel">Security Level <span class="reqd">◄</span></p>
                  <select id="AddUsersUserSecurity" runat="server" class="select-user-security">
                    <option value="">&lt;Select a Security Level&gt;</option>
                    <option value="MASTER">Master User</option>
                    <option value="ADMIN">State Admin</option>
                    <option value="COUNTY">County Admin</option>
                    <option value="LOCAL">Local Admin</option>
                  </select>
                </div>
                
                <div class="clear-both"></div>
 
                <div class="input-element user-state-code">
                  <p class="fieldlabel">State <span class="reqd">◄</span></p>
                  <select id="AddUsersUserStateCode" runat="server" class="select-user-state-code"></select>
                </div>
 
                <div class="input-element user-county-code">
                  <p class="fieldlabel">County <span class="reqd">◄</span></p>
                  <select id="AddUsersUserCountyCode" runat="server" class="select-user-county-code">
                  </select>
                </div>
 
                <div class="input-element user-local-key">
                  <p class="fieldlabel">Local District <span class="reqd">◄</span></p>
                  <select id="AddUsersUserLocalKey" runat="server" class="select-user-local-key">
                  </select>
                </div>
                
                <div class="clear-both"></div>

                    <div class="update-button">
		                  <input type="button" value="Add User" class="button-1 add-user-button">                  
                    </div>  

              </div>
            </div>
          </div>

        </div>
        
        <div id="tab-changeuser" class="main-tab content-panel tab-panel htab-panel">
          
          <div class="inner-panel rounded-border">
            <div class="tab-panel-heading horz-center">
              <div class="center-inner">
                <h4 class="center-element">Change Password or Delete User</h4>
              </div>
            </div>
            <hr/>
            <div class="content-area">
              <div class="user-scroller">
              </div>
            </div>
          </div>
        </div>
        
      </div>

    </div>

  </div>
  
  <div id="change-password-dialog" class="hidden">
    
    <input type="hidden" class="change-password-dialog-user-name"/>
  
    <div class="input-element old-password">
      <p class="fieldlabel">Old Password</p>
      <div class="databox textbox">
        <user:TextBoxWithNormalizedLineBreaks ID="ChangePasswordDialogOldPassword" spellcheck="false" 
          CssClass="change-password-dialog-old-password shadow-2" disabled="disabled" runat="server"/>
      </div>                
    </div>

    <div class="input-element new-password">
      <p class="fieldlabel">New Password <span class="reqd">◄</span></p>
      <div class="databox textbox">
        <user:TextBoxWithNormalizedLineBreaks ID="ChangePasswordDialogNewPassword" spellcheck="false" 
          CssClass="change-password-dialog-new-password shadow-2" runat="server"/>
      </div>                
    </div>
 
    <div class="update-button">
		  <input type="button" value="Change Password" class="button-1 change-password-button">                  
    </div>  
   
  </div>

</asp:Content>
