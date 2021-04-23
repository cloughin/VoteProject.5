using System;

namespace Vote
{
  public partial class ImagePage : VotePage
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      ImageManager.ServeImagePage(this);
    }
  }
}