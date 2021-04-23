<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DonationRequestNew.ascx.cs"
Inherits="Vote.Controls.DonationRequestNew" %>

<style>

  #donation-request
  {
    width: 100%;
    text-align: center;
    position: fixed;
    bottom: -200px;
  }

  #donation-request.showing
  {
    bottom: 0;
    transition: all 2s;
  }

  #donation-request .inner
  {
    width: 100%;
    max-width: 940px;
    margin: 0 auto;
    border: 4px solid #f60;
    position: relative;
    background: #fff;
    padding-bottom: 15px;
  }

  #donation-request .heading
  {
    font-weight: bold;
    color: white;
    background: #f60;
    width: 100%;
    padding: 5px 0 8px;
    font-size: 115%;
    margin-bottom: 15px;
  }

  #donation-request .message
  {
    font-weight: bold;
    padding: 0 15px;
    line-height: 130%;
  }

  #donation-request .close-box
  {
    position: absolute;
    top: -20px;
    right: 10px;
    cursor: pointer;
  }

  #donation-request .donate-button
  {
    background-color: #ff6600;
    border-radius: 10px;
    display: inline-block;
    cursor: pointer;
    color: #ffffff;
    font-family: Arial;
    font-size: 16px;
    font-weight: bold;
    padding: 6px 12px;
    text-decoration: none;
    border: none;
    margin-top: 16px;
  }

  #donation-request .donate-button:hover
  {
    opacity: 0.7;
  }

</style>

<script>
  (function ($) {

    var $donationRequest;

    function showRequest() {
      var nnx = parseInt($.cookie('nnx'));
      if (isNaN(nnx))
        nnx = 1;
      if (nnx >= 0) {
        $.ajax({
          type: "POST",
          url: "/WebService.asmx/GetDonationNagNew",
          data: "{" + "'cookieIndex': " + nnx + "}",
          contentType: "application/json; charset=utf-8",
          dataType: "json",
          success: function (result) {
            var d = result.d;
            $.cookie('nnx', d.NextMessageNumber, { expires: 365 });
            if (d.MessageText) {
              $('.message', $donationRequest).html(d.MessageText);
              $('.heading', $donationRequest).html(d.MessageHeading);
              $donationRequest.addClass("showing");
            }
          }
        });
      }
    }

    function setTimer(seconds) {
      setTimeout(function () {
        showRequest();
      }, seconds * 1000);
    }

    function resetTimer() {
      $donationRequest.removeClass("showing");
      setTimer(<%=FollowupSeconds%>);
    }

    $(function () {

      $donationRequest = $("#donation-request");

      $(".donate-button", $donationRequest).on("click", function () {
        resetTimer();
      });

      $(".close-box", $donationRequest).on("click", function () {
        resetTimer();
      });
     
      setTimer(<%=InitialSeconds%>);

    });

  })(jQuery);
</script>

<div id="donation-request" class="">
  <div class="inner">
    <div class="heading">Pardon the interruption, but &hellip;</div>
    <div class="message">If we helped you vote your interests, not the special interest groups, please help us with a donation.</div>
    <div>
      <a class="donate-button" href="/donate.aspx" target="donate">Donate Now</a>
    </div>
    <img class="close-box" src="/images/close-box.png" alt="close box">
  </div>
</div>