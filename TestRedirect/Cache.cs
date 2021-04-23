using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace TestRedirect
{
  public partial class Cache : Form
  {
    public Cache()
    {
      InitializeComponent();
    }

    private void GoButton_Click(object sender, EventArgs e)
    {
      //string testString = "►";
      //DB.VoteCacheLocal.LastProcessedId.UpdateTest(testString);
      //string localTest = DB.VoteCacheLocal.LastProcessedId.GetTest();
      //DB.VoteCache.LastProcessedId.UpdateTest(testString);
      //string commonTest = DB.VoteCache.LastProcessedId.GetTest();
      //byte[] testBytes = System.Text.Encoding.UTF8.GetBytes(testString);
      //MemoryStream stream = new MemoryStream(testBytes);
      //DB.VoteCacheLocal.LastProcessedId.UpdateTestBlob(stream.ToArray());
      //byte[] localBytes = DB.VoteCacheLocal.LastProcessedId.GetTestBlob();
      //string localString = System.Text.Encoding.UTF8.GetString(localBytes);
      //DB.VoteCache.LastProcessedId.UpdateTestBlob(stream.ToArray());
      //byte[] commonBytes = DB.VoteCacheLocal.LastProcessedId.GetTestBlob();
      //string commonString = System.Text.Encoding.UTF8.GetString(commonBytes);
    }
  }
}
