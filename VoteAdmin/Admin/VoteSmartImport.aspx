<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Admin.Master" 
AutoEventWireup="true" CodeBehind="VoteSmartImport.aspx.cs" 
Inherits="Vote.Admin.VoteSmartImportPage" %> 

<%@ Register Src="/Controls/NavigateJurisdiction.ascx" TagName="NavigateJurisdiction" TagPrefix="user" %>
<%@ Register Src="/Controls/NoJurisdiction.ascx" TagName="NoJurisdiction" TagPrefix="user" %>
<%@ Register Src="/Controls/ElectionControl.ascx" TagName="ElectionControl" TagPrefix="user" %>
<%@ Register Src="/Controls/SelectJurisdictionButton.ascx" TagName="SelectJurisdictionButton" TagPrefix="user" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <input id="ClientStateCode" class="client-state-code" type="hidden" runat="server" />
  <input id="ClientStateName" class="client-state-name" type="hidden" runat="server" />
  <div id="outer">
    <h1 id="H1" EnableViewState="false" runat="server"></h1>
    <div class="sub-heading headings">
      <h2 id="H2" EnableViewState="false" class="jurisdiction-message" runat="server"></h2>
      <h4 id="CredentialMessage" EnableViewState="false" class="credential-message" runat="server"></h4>
    </div>
    <user:SelectJurisdictionButton ID="SelectJurisdictionButton" runat="server" AddClasses="True"
     Tooltip="Import election data for a different state, county or local jurisdiction -- subject to your sign-in credentials."/>

    <div class="clear-both"></div>

    <user:NoJurisdiction ID="NoJurisdiction" runat="server" Visible="false" />

    <div id="UpdateControls" class="update-controls" runat="server">
      <div class="election-control-outer">
        <div class="mc mc-container mc-group-electioncontrol election-control-container rounded-border shadow"
         id="SelectElectionControl" runat="server">
         <a class="select-election-toggler tiptip" title="Show or hide the menu of elections"><div class="toggler"></div>
         <div class="election-control-heading rounded">Select Election</div></a>
          <asp:UpdatePanel ID="ElectionControlUpdatePanel" 
              UpdateMode="Conditional" runat="server">
              <ContentTemplate>
                <div id="ContainerElectionControl" runat="server">
                <input id="SelectedElectionKey" runat="server" class="selected-election-key" type="hidden" />
                <div class="election-control-slider">
                  <user:ElectionControl ID="ElectionControl" runat="server" />
                </div>
                </div>
              </ContentTemplate>
            </asp:UpdatePanel>
        </div>
      </div>

      <h3 id="selected-election-head" class="hidden"></h3>
      
      <div class="rounded-border boxed-group vusa-office-candidate-boxed-group hidden">
        <div class="boxed-group-label">Select VoteUSA Office and Candidate</div>
        <div class="col1">
          <select size="1" class="vusa-offices-dropdown"></select>
          <select size="1" class="vusa-candidates-dropdown hidden"></select>
        </div>
        <div class="col2">
          <input type="button" value="&#x25c4; Prev"
            class="prev-vusa-candidate-button button-1 button-smallest"/>
          <input type="button" value="Next &#x25ba;"
            class="next-vusa-candidate-button button-1 button-smallest"/>
        </div>
        <div style="clear:both"></div>
      </div>
    
      <div class="rounded-border boxed-group vs-election-boxed-group hidden">
        <div class="boxed-group-label">Select VoteSmart Election</div>
        <div class="refresh">
          <input type="button" value="Refresh VoteSmart Elections"
            id="RefreshElectionsButton"
            class="refresh-vs-elections-button button-1 button-smallest" runat="server"/>
          <div class="refresh-elections-date hidden"></div>
        </div>
        <select size="1" class="vs-elections-dropdown"></select>
        <div style="clear:both"></div>
      </div>
    
      <div class="rounded-border boxed-group vs-candidate-boxed-group hidden">
        <div class="boxed-group-label">Select VoteSmart Candidate</div>
        <div class="refresh">
          <input type="button" value="Refresh VoteSmart Candidate List for This Election"
            id="RefreshCandidatesButton"
            class="refresh-vs-candidates-button button-1 button-smallest" runat="server"/>
          <div class="refresh-candidates-date hidden"></div>
        </div>
        <select class="vs-candidates-dropdown"></select>
        <div style="clear:both"></div>
      </div>
    
      <div class="rounded-border boxed-group data-comparison-boxed-group hidden">
        <div class="boxed-group-label">VoteUSA/VoteSmart Data Comparison</div>
        <div class="button-group-1">
          <input type="button" value="Update"
            class="update-button button-1 button-smaller"
            disabled="disabled"/>
          <input type="button" value="Copy VoteSmart Data Into All Empty VoteUSA Fields"
            class="copy-empty-button button-2 button-smallest"/>
        </div>
        <div class="refresh">
          <input type="button" value="Refresh VoteSmart Candidate Data"
            class="refresh-vs-candidate-button button-1 button-smallest"
            disabled="disabled"/>
          <div class="refresh-candidate-date hidden"></div>
        </div>
        <div class="name comparison">
          <div class="vusa-data">
            <div class="img"></div>
            <h3></h3>
            <input type="hidden" class="politician-key" />
          </div>
          <div class="copy-buttons"></div>
          <div class="vs-data">
            <div class="img"></div>
            <h3></h3>
          </div>
        </div>
        <div class="birthdate comparison">
          <div class="vusa-data">
            <h3>VoteUSA Birth Date</h3>
            <input type="text" class="value" />
          </div>
          <div class="copy-buttons">
            <img class="replace" src="/images/replace.png" title="Replace VoteUSA Birth Date with VoteSmart Birth Date" alt="Replace" />
          </div>
          <div class="vs-data">
            <h3>VoteSmart Birth Date</h3>
            <div class="value"></div>
          </div>
        </div>
        <div class="email comparison">
          <div class="vusa-data linked">
            <h3>VoteUSA Email</h3>
            <div class="icon-box"><a class="icon" title="Open email link" href="/" target="link"></a></div>
            <input type="text" class="value" />
          </div>
           <div class="copy-buttons">
            <img class="replace" src="/images/replace.png" title="Replace VoteUSA Email with VoteSmart Email/Webmail" alt="Replace" />
          </div>
          <div class="vs-data linked">
            <h3>VoteSmart Email/Webmail</h3>
            <div class="icon-box"><a class="icon" title="Open email link" href="/" target="link"></a></div>
            <div class="value"></div>
          </div>
        </div>
        <div class="website comparison">
          <div class="vusa-data linked">
            <h3>VoteUSA Website</h3>
            <div class="icon-box"><a class="icon" title="Open website" href="/" target="link"></a></div>
            <input type="text" class="value" />
          </div>
          <div class="copy-buttons">
            <img class="replace" src="/images/replace.png" title="Replace VoteUSA Website with VoteSmart Website" alt="Replace" />
          </div>
          <div class="vs-data linked">
            <h3>VoteSmart Website</h3>
            <div class="icon-box"><a class="icon" title="Open website" href="/" target="link"></a></div>
            <div class="value"></div>
          </div>
        </div>
        <div class="facebook comparison">
          <div class="vusa-data linked">
            <h3>VoteUSA Facebook</h3>
            <div class="icon-box"><a class="icon" title="Open Facebook page" href="/" target="link"></a></div>
            <input type="text" class="value" />
          </div>
          <div class="copy-buttons">
            <img class="replace" src="/images/replace.png" title="Replace VoteUSA Facebook with VoteSmart Facebook" alt="Replace" />
          </div>
          <div class="vs-data linked">
            <h3>VoteSmart Facebook</h3>
            <div class="icon-box"><a class="icon" title="Open Facebook page" href="/" target="link"></a></div>
            <div class="value"></div>
          </div>
        </div>
        <div class="twitter comparison">
          <div class="vusa-data linked">
            <h3>VoteUSA Twitter</h3>
            <div class="icon-box"><a class="icon" title="Open Twitter page" href="/" target="link"></a></div>
            <input type="text" class="value" />
          </div>
          <div class="copy-buttons">
            <img class="replace" src="/images/replace.png" title="Replace VoteUSA Twitter with VoteSmart Twitter" alt="Replace" />
          </div>
          <div class="vs-data linked">
            <h3>VoteSmart Twitter</h3>
            <div class="icon-box"><a class="icon" title="Open Twitter page" href="/" target="link"></a></div>
            <div class="value"></div>
          </div>
        </div>
        <div class="youtube comparison">
          <div class="vusa-data linked">
            <h3>VoteUSA YouTube</h3>
            <div class="icon-box"><a class="icon" title="Open YouTube video" href="/" target="link"></a></div>
            <input type="text" class="value" />
          </div>
          <div class="copy-buttons">
            <img class="replace" src="/images/replace.png" title="Replace VoteUSA YouTube with VoteSmart YouTube" alt="Replace" />
          </div>
          <div class="vs-data linked">
            <h3>VoteSmart YouTube</h3>
            <div class="icon-box"><a class="icon" title="Open YouTube video" href="/" target="link"></a></div>
            <div class="value"></div>
          </div>
        </div>
        <div class="family comparison">
          <div class="vusa-data">
            <h3>VoteUSA Personal and Family</h3>
            <textarea class="value"></textarea>
          </div>
          <div class="copy-buttons">
            <img class="prepend" src="/images/prepend.png" title="Prepend VoteSmart Family to VoteUSA Personal and Family" alt="Prepend" />
            <img class="replace" src="/images/replace.png" title="Replace VoteUSA Personal and Family with VoteSmart Family" alt="Replace" />
            <img class="append" src="/images/append.png" title="Append VoteSmart Family to VoteUSA Personal and Family" alt="Append" />
          </div>
          <div class="vs-data">
            <h3>VoteSmart Family</h3>
            <textarea class="value" readonly="readonly"></textarea>
          </div>
        </div>
        <div class="education comparison">
          <div class="vusa-data">
            <h3>VoteUSA Educational Background</h3>
            <textarea class="value"></textarea>
          </div>
          <div class="copy-buttons">
            <img class="prepend" src="/images/prepend.png" title="Prepend VoteSmart Education to VoteUSA Educational Background" alt="Prepend" />
            <img class="replace" src="/images/replace.png" title="Replace VoteUSA Educational Backgroundy with VoteSmart Education" alt="Replace" />
            <img class="append" src="/images/append.png" title="Append VoteSmart Education to VoteUSA Educational Backgroundy" alt="Append" />
          </div>
          <div class="vs-data">
            <h3>VoteSmart Education</h3>
            <textarea class="value" readonly="readonly"></textarea>
          </div>
        </div>
        <div class="professional comparison">
          <div class="vusa-data">
            <h3>VoteUSA Professional Experience</h3>
            <textarea class="value"></textarea>
          </div>
          <div class="copy-buttons">
            <img class="prepend" src="/images/prepend.png" title="Prepend VoteSmart Profession to VoteUSA Professional Experiencey" alt="Prepend" />
            <img class="replace" src="/images/replace.png" title="Replace VoteUSA Professional Experience with VoteSmart Profession" alt="Replace" />
            <img class="append" src="/images/append.png" title="Append VoteSmart Profession to VoteUSA Professional Experiencey" alt="Append" />
          </div>
          <div class="vs-data">
            <h3>VoteSmart Profession</h3>
            <textarea class="value" readonly="readonly"></textarea>
          </div>
        </div>
        <div class="military comparison">
          <div class="vusa-data">
            <h3>VoteUSA Military Service</h3>
            <textarea class="value"></textarea>
          </div>
          <div class="vs-data">
          </div>
        </div>
        <div class="civic comparison">
          <div class="vusa-data">
            <h3>VoteUSA Civic Involvement</h3>
            <textarea class="value"></textarea>
          </div>
          <div class="copy-buttons">
            <img class="prepend" src="/images/prepend.png" title="Prepend VoteSmart Org Membership to VoteUSA Civic Involvement" alt="Prepend" />
            <img class="replace" src="/images/replace.png" title="Replace VoteUSA Civic Involvement with VoteSmart Org Membership" alt="Replace" />
            <img class="append" src="/images/append.png" title="Append VoteSmart Org Membership to VoteUSA Civic Involvement" alt="Append" />
          </div>
          <div class="vs-data">
            <h3>VoteSmart Org Membership</h3>
            <textarea class="value" readonly="readonly"></textarea>
          </div>
        </div>
        <div class="political comparison">
          <div class="vusa-data">
            <h3>VoteUSA Political Experience</h3>
            <textarea class="value"></textarea>
          </div>
          <div class="copy-buttons">
            <img class="prepend" src="/images/prepend.png" title="Prepend VoteSmart Political to VoteUSA Political Experience" alt="Prepend" />
            <img class="replace" src="/images/replace.png" title="Replace VoteUSA Political Experience with VoteSmart Political" alt="Replace" />
            <img class="append" src="/images/append.png" title="Append VoteSmart Political to VoteUSA Political Experience" alt="Append" />
          </div>
          <div class="vs-data">
            <h3>VoteSmart Political</h3>
            <textarea class="value" readonly="readonly"></textarea>
          </div>
        </div>
        <div class="religion comparison">
          <div class="vusa-data">
            <h3>VoteUSA Religious Affiliation</h3>
            <textarea class="value"></textarea>
          </div>
          <div class="copy-buttons">
            <img class="prepend" src="/images/prepend.png" title="Prepend VoteSmart Religion to VoteUSA Religious Affiliationy" alt="Prepend" />
            <img class="replace" src="/images/replace.png" title="Replace VoteUSA Religious Affiliation with VoteSmart Religion" alt="Replace" />
            <img class="append" src="/images/append.png" title="Append VoteSmart Religion to VoteUSA Religious Affiliation" alt="Append" />
          </div>
          <div class="vs-data">
            <h3>VoteSmart Religion</h3>
            <textarea class="value" readonly="readonly"></textarea>
          </div>
        </div>
        <div class="accomplishments comparison">
          <div class="vusa-data">
            <h3>VoteUSA Accomplishments and Awards</h3>
            <textarea class="value"></textarea>
          </div>
          <div class="vs-data">
          </div>
        </div>
        <div class="photo comparison">
          <div class="vusa-data">
            <h3>VoteUSA Photo</h3>
            <div class="img"></div>
          </div>
          <div class="copy-buttons">
            <img class="replace" src="/images/replace.png" title="Replace VoteUSA Photo with VoteSmart Photo" alt="Replace" />
          </div>
          <div class="vs-data">
            <h3>VoteSmart Photo</h3>
            <div class="value"></div>
            <div class="img"></div>
          </div>
        </div>
        <div style="clear:both"></div>
      </div>

    </div>  

  </div>

  <user:NavigateJurisdiction ID="NavigateJurisdiction1" runat="server" />

</asp:Content>
