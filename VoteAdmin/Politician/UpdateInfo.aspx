<%@ Page Language="C#" MasterPageFile="~/Master/Admin.Master"  
ValidateRequest="false" AutoEventWireup="true" CodeBehind="UpdateInfo.aspx.cs" 
EnableViewState="false"
Inherits="VoteAdmin.Politician.UpdateInfoPage" %>

<asp:Content ID="HeadContent" EnableViewState="false" ContentPlaceHolderID="MasterHeadContent" runat="server">
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
<%--    
  <div class="header clearfix">
    <a id="MainBannerHomeLink" href="/" target="public" title="" runat="server"><img src="/images/logo-2019-48h.png" class="logo" alt="Vote USA - Vote informed"></a>
    <div class="title">Manage Issue Responses</div>
  </div>--%>
    
  <div class="header">
    <a id="MainBannerHomeLink" href="/" target="public" class="logo-link" runat="server"><img src="/images/logo-2019-48h.png" class="logo" alt="Vote USA - Vote informed"></a>
    <div class="title">Manage Issue Responses</div>
    <div class="back-container">
      <a href="/Politician/main.aspx" class="back-link button-2 button-smallest">Back to Start Page</a>
    </div>
  </div>

  <div class="content">

    <div class="search-container">
      <div class="politician-name" id="PoliticianName" runat="server"></div>
      <div class="search-text-container">
        <input type="text" class="search-text search-item" placeholder="search for an issue or topic to enter"/>
        <img src="/images/cancel16.png" class="cancel-box"/>
      </div>
      <div class="search-results search-item hide">
        <div class="results"></div>
      </div>
      <div class="or">OR</div>
      <select class="issues-select search-item no-appearance">
        <asp:Literal ID="IssuesSelectOptions" runat="server"></asp:Literal>
      </select>
      <br/>
      <select class="topics-select search-item no-appearance hide">
      </select>
    </div>
  </div>

  <div class="edit-boxes">
    <div class="new-text-response response edit-box hide hideable" data-is-video="false">
      <span class="head">New Text Response</span>
      <textarea class="answer-text the-answer" rows="10"></textarea>
      <input type="button" value="Save" class="save-response button-1 button-smallest"/>
      <input type="button" value="Cancel" class="cancel-response button-3 button-smallest"/>
    </div>

    <div class="new-youtube-response response edit-box hide hideable" data-is-video="true">
      <span class="head">New YouTube Response</span>
      <div class="label">YouTube URL</div>
      <input type="text" class="answer-youtube-url the-answer" value=""/>
      <input type="button" value="Save" class="save-response button-1 button-smallest"/>
      <input type="button" value="Cancel" class="cancel-response button-3 button-smallest"/>
    </div>

    <div class="edit-text-response response edit-box hide hideable" data-is-video="false">
      <input type="hidden" class="question-id"/>
      <input type="hidden" class="sequence"/>
      <span class="head">Edit Text Response</span>
      <textarea class="answer-text the-answer" rows="10"></textarea>
      <input type="button" value="Update" class="update-response button-1 button-smallest"/>
      <input type="button" value="Cancel" class="cancel-response button-3 button-smallest"/>
    </div>

    <div class="edit-youtube-response response edit-box hide hideable" data-is-video="true">
      <input type="hidden" class="question-id"/>
      <input type="hidden" class="sequence"/>
      <span class="head">Edit YouTube Response</span>
      <div class="label">YouTube URL</div>
      <input type="text" class="answer-youtube-url the-answer"/>
      <input type="button" value="Update" class="update-response button-1 button-smallest"/>
      <input type="button" value="Cancel" class="cancel-response button-3 button-smallest"/>
    </div>
  </div>
  
  <div class="answers-container hide hideable">
    <div class="head">
      <p class="msg"></p>
      <button type="button" class="add-text-response add-button button-1 button-smaller"><img src="/images/pencil16.png"/>Add a response</button>
      <button type="button" class="add-youtube-response add-button button-1 button-smaller"><img src="/images/yt16.png"/>Add YouTube video URL</button>
    </div>
    <div class="answers"></div>
  </div>

  <div class="overlay hide"><img src="/images/loader.gif" /></div>
</asp:Content>
