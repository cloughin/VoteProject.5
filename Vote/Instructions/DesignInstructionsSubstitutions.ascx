<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DesignInstructionsSubstitutions.ascx.cs" Inherits="Vote.Admin.SubstitutionsState" %>
<%--
    <table class="tableAdmin" id="Table18" cellspacing="0" cellpadding="0">
      <tr>
        <td class="H" valign="top" colspan="2">
          Text and HTML Substitutions in Textboxes</td>
      </tr>
      <tr>
        <td align="left" colspan="2" valign="top" class="T">
          Below are substitutions you can use to have the appropriate data inserted in the
          content. Each substitution place holder must start with [[ and end with ]]. The substitution name between thes opening and closing double brackes is case INSENSITIVE.&nbsp;</td>
      </tr>
      <tr>
        <td align="left" class="TSmall" valign="top">
          <strong>--State Substitutions---</strong><br />
          Depends on which State's data is being presented<br />
          [[State]] = State Name<br />
          [[BallotName]] = State Title on ballots<br />
          [[StateEmail]] = State BOE Email Link<br />
          [[StateAnchor]] = State Board of Elections Anchor<br />
          <strong>-- Organization Substitutions---<br />
          </strong>Depends on the organization that
          ownes the domain<br />
          [[Org]] = Organization Name<br />
          [[OrgEmail]] = Organization Email Link<br />
          [[OrgAnchor]] = Organization Anchor<br />
          <strong>---Vote-USA Substitutions&nbsp; ---</strong><br />
          These will place an email or webpage anchor
          to the Vote-USA.org website<br />
          [[VoteEmail]] = Vote-USA Email Link
          <br />
          [[VoteAnchor]] = Vote-USA.org Anchor<br />
          -<strong>--Misc Substitutions---</strong><br />
          [[Title]] = The title of a report<br />
          These will place a custom email or webpage anchor<br />
          ##ANCHOR## = Any web address (like: abc.com)<br />
          @@EMAIL@@ = Any email address<br />
          </td>
        <td align="left" class="TSmall" valign="top">
          <strong>--Upcoming Viewable Election Substitutions---<br />
          </strong>[[ElectionDesc]] = Most Recent Viewable Description 
          <br />[[ElectionDate]] = Most Recent Viewable Date
          <br />[[ElectionType]] = like General Election, Off Year Election
          <br />
          [[Contests]] = list of the election contests<br />
          <strong>--Future Election Substitutions---</strong><br />[[FutureElectionDesc]] = Next Future Description 
          <br />[[FutureElectionDate]] = Next Future Date
          <br />
         [[FutureElectionType]] = like General Election, Off Year Election
          <br />
          <strong>--Next Election Substitutions---</strong>
          <br />[[NextElectionDesc]] = Next Future Description 
          <br />[[NextElectionDate]] = Next Future Date
          <br />
         [[NextElectionType]] = like General Election, Off Year Election<br />
          <strong>-- Old Election Substitutions (may have no further use for) --</strong>
          <br />
          [[ElectionDescGeneral]] = Description Upcoming Generalription Upcoming Generall
           <br />
          [[ElectionDateGeneral]] = Date Upcoming General<br />
          [[ElectionDescOffYear]] = Description Upcoming Off Year<br />
          [[ElectionDateOffYear]] = Date Upcoming Off Year
          <br />
          [ElectionDescSpecial]] = Description Upcoming Special<br />
          [[ElectionDateSpecial]] = Date Upcoming Special
          <br />
          [ElectionDescPrimary]] = Description Upcoming Primary<br />
          [[ElectionDatePrimary]] = Date Upcoming Primary
          <br />
         [[UpcomingElections]] = List of elections and dates<br />
          </td>
      </tr>
    </table>
    --%>