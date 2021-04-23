<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SampleBallotEmailDialog.ascx.cs" Inherits="Vote.Controls.SampleBallotEmailDialog" %>

<div id="sampleBallotEmailDialog">
  <script type="text/javascript">
    var SampleBallotEmailDialogEnabled = <%=DB.Vote.Master.GetPresentGetFutureSampleBallotsDialog(false) ? "true" : "false" %>;
  </script>
  <div class="header">
    <div>Get Future Sample Ballots Automatically</div>
  </div>
  <div class="inner">
    <div class="blurb">If you would like us to send future sample ballots as soon as they are available, enter your email address below:</div>
    <div class="inner2">
      <div class="entry"><div class="input"><div class="input2"><input type="text" class="email" /></div></div><a class="emailEnter"><img src="/images/findmylocation.png" alt=""/>Enter email address</a></div>
      <div class="nothanks"><a class="emailNoThanks">No thanks</a></div>
    </div>
  </div>
</div>