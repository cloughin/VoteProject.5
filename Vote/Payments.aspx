<%@ Page Title="" Language="C#" MasterPageFile="~/Public.Master" AutoEventWireup="true" 
CodeBehind="Payments.aspx.cs" Inherits="Vote.PaymentsPage" %>

<asp:Content ID="HeadTopContent" ContentPlaceHolderID="MasterHeadTopContent" runat="server">
</asp:Content>

<asp:Content ID="HeadBottomContent" ContentPlaceHolderID="MasterHeadBottomContent" runat="server">
  <style>
    p
    {
      margin: 1em 0;
    }
    #payment-for
    {
      width: 100%;
    }
    .error
    {
      background-color: #ff0;
    }
  </style>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <script src="https://www.paypal.com/sdk/js?client-id=AQhKnX0I0t0jlHoDS8zayIRCKql2YfoF0FToKxdKU7Ii_FrnvDTk76TM6AsR7sVNwQafJR_3wImI-ixu"></script>

  <h1>PayPal Payment Test</h1>

<div id="the-form">
<p><label>This payment is for: </label><br/><input id="payment-for" /></p>

<p><label>Amount: </label><input id="payment-amount"/></p>
</div>
  <div id="paypal-button-container"></div>

<script>
  function paymentForIsValid() {
    return $.trim($("#payment-for").val()) != "";
  }
  function paymentAmountIsValid() {
    return /^((-?[0-9]+)|(-?([0-9]+)?[.][0-9]+))$/.test($("#payment-amount").val());
  }
  paypal.Buttons({
    onInit: function(data, actions) {
      actions.disable();
      $("#payment-amount,#payment-for").on("propertychange change click keyup input paste spinchange", function() {
        if (paymentForIsValid() && paymentAmountIsValid())
          actions.enable();
        else
          actions.disable();
        if (paymentForIsValid()) $("#payment-for").removeClass("error");
        if (paymentAmountIsValid()) $("#payment-amount").removeClass("error");
      });
    },
    onClick() {
      var msgs = [];
      if (!paymentForIsValid()) {
        $("#payment-for").addClass("error");
        msgs.push("\"This payment is for\" is required");
      }
      if (!paymentAmountIsValid()) {
        $("#payment-amount").addClass("error");
        msgs.push("\"Amount\" is required and must be a valid number");
      }
      if (msgs.length) {
        UTIL.alert(msgs.join("\n"));
      }
    },
    createOrder: function (data, actions) {
      // Set up the transaction
      return actions.order.create({
        //payer: {
        //  name: {
        //    given_name: 'Bozo',
        //    surname: 'Clown'
        //  },
        //  address: {
        //    address_line_1: '123 Some Street',
        //    admin_area_2: 'Nowhere',
        //    admin_area_1: 'MT',
        //    postal_code: '87654',
        //    country_code: 'US'
        //  }
        //},
        purchase_units: [{
          //description: 'For services rendered by Vote-USA.org',
          items: [
            {
              name: $.trim($("#payment-for").val()),
              unit_amount: { currency_code: 'USD', value: $("#payment-amount").val() },
              quantity: '1'
            }],
          amount: {
            value: $("#payment-amount").val(),
            breakdown: { item_total: { currency_code: 'USD', value: $("#payment-amount").val() } }
          }
        }]
      });
    },
    onApprove: function (data, actions) {
      // Capture the funds from the transaction
      return actions.order.capture().then(function (details) {
        // Show a success message to your buyer
        alert('Transaction completed by ' + details.payer.name.given_name + ' ' + details.payer.name.surname);
      });
    }
  }).render('#paypal-button-container');
</script>
</asp:Content>

<asp:Content ID="BottomContent" ContentPlaceHolderID="MasterBottomContent" runat="server">
</asp:Content>
