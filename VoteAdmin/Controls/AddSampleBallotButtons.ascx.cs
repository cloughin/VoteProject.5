﻿using System;
using System.Web.UI;

namespace Vote.Controls
{
  public partial class AddSampleBallotButtons : UserControl
  {
    protected void Page_Load(object sender, EventArgs e) => 
      Page.IncludeJs("~/js/AddSampleBallotButtons.js");
  }
}