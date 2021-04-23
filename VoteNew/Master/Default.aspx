<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="VoteNew.Master.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/jq/jquery-ui.css" rel="stylesheet" type="text/css"/>
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js" type="text/javascript"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js" type="text/javascript"></script>
    <link rel='stylesheet' type='text/css' href='/jq/treeskin/ui.dynatree.css' /> 
    <script src="/jq/jquery.json-2.2.min.js" type="text/javascript"></script>
    <script src="/jq/jquery.dynatree.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
      $.globals = {}; // create global namespace
      $().ready(
        function ()
        {
          $("#navtree").dynatree(
          {
            fx: { height: "toggle", duration: 200 },
            imagePath: "/jq/treeskin/",
            autoCollapse: true,
            onLazyRead: function (node)
            {
              var data = node.data.ajaxData;
              if (typeof (data) == 'object')
                data = $.toJSON(data);
              $.ajax(
              {
                type: "POST",
                url: node.data.ajaxMethod,
                data: data,
                contentType: "application/json; charset=utf-8",
                success: function (result)
                {
                  node.setLazyNodeStatus(DTNodeStatus_Ok);
                  node.addChild(result.d);
                },
                error: function (result)
                {
                  node.setLazyNodeStatus(DTNodeStatus_Error, {
                    tooltip: result.status + ' ' + result.statusText,
                    info: result.statusText
                  });
                }
              });
            },
            children:
            [
              { title: "State Administration", isFolder: true,
                children:
                [
                  { title: "Elections, Elected Officials, Candidates, Legislative Districts, and Political Parties", isFolder: true,
                    isLazy: true, ajaxMethod: "/master/default.aspx/GetStateNodes", ajaxData: { option: 1 }
                  },
                  { title: "Issues and Questions", isFolder: true,
                    isLazy: true, ajaxMethod: "/master/default.aspx/GetStateNodes", ajaxData: { option: 2 }
                  }
                ]
              },
              { title: "Design & Content", isFolder: true,
                children:
                [
                  { title: "Default" },
                  { title: "Custom" }
                ]
              },
              { title: "Domains" },
              { title: "Organizations" },
              { title: "Political Parties & Email Addresses" },
              { title: "Emails" },
              { title: "Bulk Updates", isFolder: true,
                children:
                [
                  { title: "For ALL Bulk Updates" },
                  { title: "Upcoming Election Reports" },
                  { title: "Previous Election Reports" },
                  { title: "Other Election Reports Operations" },
                  { title: "Elected Officials (Incumbents) Reports" },
                  { title: "Offices, Politicians and Officials Lists - Set All NOT Current" }
                ]
              },
              { title: "301 & 404 Error Logs" },
              { title: "Office(s) Maintenance" },
              { title: "Politician(s) Maintenance" },
              { title: "Management Reports" },
              { title: "Fix Keys and Cleanup Critical Tables" },
              { title: "Zip Code & Legislative Districts" },
              { title: "Sitemaps" },
              { title: "Security" },
              { title: "Cleanup Elections Offices & Politicians" },
              { title: "Test Mode", isFolder: true,
                children:
                [
                  { title: "Visible MASTER Controls on Forms" },
                  { title: "Debug on Production Server" },
                  { title: "Set Test Mode" }
                ]
              },
              { title: "Navbars" },
              { title: "Cached Pages" },
              { title: "Election Deletions" },
              { title: "Edit Answers" },
              { title: "Database Fixup Utilities", isFolder: true,
                children:
                [
                  { title: "Politicians" },
                  { title: "Database Maintenance Utilities" },
                  { title: "Reports" },
                  { title: "Misc" }
                ]
              },
              { title: "Zip Codes" },
              { title: "Misc Database Maintenance" }
            ]
          });
        });
    </script>
    <style type="text/css">
      ul.dynatree-container
      {
        border: none;
      }
      span.dynatree-folder a
      {
        font-weight: normal;
      }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div id="navtree"></div>
    </div>
    </form>
</body>
</html>
