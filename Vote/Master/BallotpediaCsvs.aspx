<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Admin.Master" AutoEventWireup="true" 
CodeBehind="BallotPediaCsvs.aspx.cs" Inherits="Vote.Master.BallotPediaCsvsPage" %>

<%@ Register TagPrefix="user" Namespace="Vote" Assembly="VoteLibrary" %>
<%@ Register Src="/Controls/FeedbackContainer.ascx" TagName="FeedbackContainer" TagPrefix="user" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <div id="outer">
    <h1 id="H1" runat="server"></h1>
      
    <div id="main-tabs">
      <div id="ContainerUpload" class="update-all rounded-border boxed-group upload-boxed-group">
        <div class="boxed-group-label">Upload New BallotPedia CSV</div>

        <%-- 
          This is not in an update panel because we do a jQuery plugin ajax file upload.
          That's because a file control isn't supported in partial page updates.
          We also don't use web controls for the input and button, but html controls.
        --%>

        <input type="hidden" id="UploadUrl" EnableViewState="false" class="upload-url mc mc-g-uploadurl" runat="server" />
        <input type="hidden" id="DescriptionUpload" EnableViewState="false" runat="server" />
          <div class="fileinputs mt-upload">
          <div class="masker masker1"></div>
          <div class="masker masker2"></div>
          <input type="file" id="ControlUpload" name="ControlUpload" runat="server"
          class="upload-file mc-mt-upload" />
          <input type="button" value="Browse" onclick="$$('<%=ControlUpload.ClientID %>').click()"
          class="button-1 browse-button" />
          <div class="file-name-container">
            <p id="UploadFilename" class="shadow-2 file-name mc mc-filename mc-group-upload" />
            <div class="tab-ast tiptip" id="AsteriskUpload" EnableViewState="false" runat="server"
            title="There is a CSV selected but it has not yet been uploaded"></div>
            <div class="file-name-clear tiptip" title="Clear the selected filename"></div>
            <div class="clear-both"></div>               
          </div>
          <div class="clear-both"></div>
        </div>
        <div class="input-element saveas">
          <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
            ID="ControlUploadSaveAs" placeholder="Save CSV using original filename" 
            CssClass="shadow-2 tiptip mc-nomonitor" title="Enter an alternate filename if desired" runat="server"/>
        </div>
        <div class="input-element overwrite">
          <div class="databox kalypto-checkbox">
            <div class="kalypto-container">
              <input id="ControlUploadOverwrite" class="kalypto mc-nomonitor" 
              runat="server" type="checkbox" />
            </div>
            <div class="kalypto-checkbox-label">Check to overwrite existing file with same name</div>
          </div>
        </div>
        <div class="clear-both"></div>
        <div class="bottom">
          <hr />
          <div class="feedback-floater-for-ie7">
            <user:FeedbackContainer ID="FeedbackUpload" EnableViewState="false" runat="server" />
          </div>
          <div class="upload-button">
            <input type="button" id="ButtonUpload" EnableViewState="false" runat="server" value="Upload"
            class="upload-button button-1 tiptip" title="Upload the CSV"/>
          </div>   
          <div class="clear-both"></div>  
        </div>             

      </div>

      <div class="rounded-border boxed-group listcsvs-boxed-group">
        <div class="boxed-group-label">Uploaded CSVs</div>
        <div class="input-element showallcsvs">
          <div class="databox kalypto-checkbox">
            <div class="kalypto-container">
              <input id="ControlListCsvsShowAllCsvs" class="kalypto" type="checkbox" />
            </div>
            <div class="kalypto-checkbox-label">Check to show all CSVs, uncheck to show only incomplete CSVs</div>
          </div>
        </div>
        <div class="clear-both"></div>
        <hr />
        <div class="uploaded-csvs list-container">
          <p>There are no incomplete uploaded CSVs</p>
        </div>
        <hr />
          <div class="rename-button">
            <input type="button" value="Rename" disabled="disabled"
            class="apply-button button-2 tiptip" title="Rename the selected CSV"/>
          </div>   
          <div class="delete-button">
            <input type="button" value="Delete" disabled="disabled"
            class="apply-button button-3 tiptip" title="Delete the selected CSV"/>
          </div>   
        <div class="download-button">
          <input type="button" value="Download"
          class="button-1 tiptip" title="Download the selected CSV" disabled="disabled"/>
          <a class="hidden"></a>
        </div>   
        <div class="clear-both"></div>  
      </div>

      <div class="rounded-border boxed-group listcandidates-boxed-group">
        <div class="boxed-group-label">Candidates in Selected CSV</div>
          <div class="input-element showallcandidates">
            <div class="databox kalypto-checkbox">
              <div class="kalypto-container">
                <input class="kalypto" type="checkbox" />
              </div>
              <div class="kalypto-checkbox-label">Check to show all candidates, uncheck to show only incomplete candidates</div>
            </div>
          </div>
          <div class="clear-both"></div>
          <div class="input-element markcomplete">
            <div class="databox kalypto-checkbox">
              <div class="kalypto-container">
                <input class="kalypto" type="checkbox" />
              </div>
              <div class="kalypto-checkbox-label">Check to mark this file as complete</div>
            </div>
          </div>
          <div class="clear-both"></div>
          <hr />
          <div class="loaded-candidates list-container"></div>
          <hr class="line2 hidden" />
          <div class="apply-button">
            <input type="button" value="Apply Links to Candidates" disabled="disabled"
            class="apply-button button-2 tiptip" title="Apply the BallotPedia links to the candidates on the VoteUSA website"/>
          </div>   
          <div class="update-button">
            <input type="button" value="Update" disabled="disabled"
            class="update-button button-1 tiptip" title="Update the candidates"/>
          </div>   
          <div class="clear-both"></div>  
      </div>
    
    </div>

  </div>

  <div id="ballotpedia-csv-rename-dialog" class="ballotpedia-csv-rename-dialog hidden">
    <div class="inner">
      <div>
        <p class="label">Original name:</p>
        <p class="original"></p>
        <p class="label">Rename to:</p>
        <p class="new-name"><input type="text"/></p>
      </div>
      <div class="bottom-box button-box">
        <input type="button" value="Rename" 
          class="rename-button button-1 button-smallest"/>
        <input type="button" value="Cancel" 
          class="cancel-button button-3 button-smallest"/>
       </div>
    </div>
  </div>

</asp:Content>
