<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AnyPage.aspx.cs" Inherits="PatchPOC.AnyPage" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
  <iframe id="voteusa" scrolling="no" src="http://vote.localhost-usa/default2.aspx" style="width: 100%; border: none; overflow: hidden; height:596px"></iframe>
<%--  <p>
    <iframe id="ecIframe" scrolling="no" src="https://calc.milgard.com/" style="width: 100%; border: none"></iframe>
    <script type="text/javascript" src="https://calc.milgard.com/_js/iframeResizer.js"></script>
    <script>
      (function($) {
        iFrameResize(
          {
            log:
              false
          },
          '#ecIframe');
        $(
          function() {
            $(
                window)
              .on(
                'resize orientationchange first',
                function(event) {
                  $(
                      "#ecIframe")
                    [0
                    ].contentWindow
                    .postMessage(
                      "[resize]" +
                      $(
                        window)
                      .width() +
                      ":" +
                      $(
                        window)
                      .height(),
                      "*");
                })
              .on(
                "message",
                function(event) {
                  var
                    e =
                      event
                        .originalEvent;
                  var
                    data =
                      e.data
                        ? e
                        .data
                        : e
                        .message;
                  if
                  (typeof
                    data ===
                    "string" &&
                    data
                    .substr(
                      0,
                      6) ===
                    "[show]"
                  ) {
                    var
                      etop =
                        $(
                            "#ecIframe")
                          .offset()
                          .top;
                    $(
                        "html, body")
                      .animate(
                        {
                          scrollTop:
                            etop -
                              2
                        },
                        250);
                  }
                });
            $(
                "#ecIframe")
              .on(
                "load",
                function() {
                  $(
                      window)
                    .trigger(
                      "first")
                });
          });
      })(
        jQuery);
    </script>
  </p>--%>
</asp:Content>
