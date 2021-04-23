<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SampleBallotEmailDialogResponsive.ascx.cs" Inherits="Vote.Controls.SampleBallotEmailDialogResponsive" %>

<div id="sampleBallotEmailDialog">
  <script type="text/javascript">
    var SampleBallotEmailDialogEnabled = <%=DB.Vote.Master.GetPresentGetFutureSampleBallotsDialog(false) ? "true" : "false" %>;
  </script>
  <div class="header">
    <div class="heading-text">Get Your Future Ballot Choices Automatically</div>
  </div>
  <div class="inner">
    <div class="blurb">If you would like us to send future ballot choices as soon as they are available, enter your email address below:</div>
    <div class="inner2">
      <div class="entry-container">
        <div class="entry">
          <input type="email" class="email no-zoom" />
          <a class="emailEnter">Enter</a>
        </div>
      </div>
      <div class="nothanks clearfix">
        <a class="emailNoThanks">No thanks</a>
        <a class="already-signed-up">I already signed up</a>
      </div>
    </div>
  </div>
</div>