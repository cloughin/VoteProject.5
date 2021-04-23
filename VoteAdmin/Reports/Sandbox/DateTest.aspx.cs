using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Vote.Sandbox
{
  public partial class DateTest : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      new LocalDate(DateTime.UtcNow, "M/D/YYYY h:mm:ss A").AddTo(
        LocalDatePlaceHolder);
    }
  }
}