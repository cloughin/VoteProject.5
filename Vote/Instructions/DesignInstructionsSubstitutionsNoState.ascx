<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DesignInstructionsSubstitutionsNoState.ascx.cs" Inherits="Vote.DesignInstructionsSubstitutionsNoState" %>
    <table class="tableAdmin" id="Table18" cellspacing="0" cellpadding="0">
      <tr>
        <td align="center" class="H" valign="top" colspan="2">
          Textbox Substitutions</td>
      </tr>
      <tr>
        <td align="left" colspan="2" valign="top" class="T">
          Below are substitutions you can use to cause the appropriate data to be retrieved
          from the database when pages are presented. &nbsp;&nbsp;Each substitution place
          holder must start with [[ and end with ]]. The substitution name between thes opening
          and closing double brackes is case INSENSITIVE.&nbsp;<br />
          <strong>Organization Substitutions:</strong> .<br />
          <strong>Vote-USA Substitutions: </strong>These will place an email or webpage anchor
          to the Vote-USA.org website.<br />
          <strong>Misc Substitutions: </strong>These will place a custom email or webpage
          anchor.</td>
      </tr>
      <tr>
        <td align="left" class="TSmall" valign="top">
          <strong>-- Organization Substitutions---<br />
          </strong>
          These depend on the organization that
          ownes the domain<br />
          [[Org]] = Organization Name<br />
          [[OrgEmail]] = Organization Email Link<br />
          [[OrgAnchor]] = Organization Anchor<br />
          </td>
        <td align="left" class="TSmall" valign="top">
          <strong>---Vote-USA Substitutions&nbsp; ---</strong><br />
          These will place an email or webpage anchor to the Vote-USA.org website<br />
          [[VoteEmail]] = Vote-USA Email Link
          <br />
          [[VoteAnchor]] = Vote-USA.org Anchor<br />
          <strong>---Misc Substitutions---</strong><br />
          These will place a custom email or webpage anchor<br />
          ##ANCHOR## = Any web address (like: abc.com)<br />
          @@EMAIL@@ = Any email address</td>
      </tr>
    </table>
