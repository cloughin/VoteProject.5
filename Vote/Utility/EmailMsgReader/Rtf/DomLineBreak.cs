using System;

namespace DocumentServices.Modules.Readers.MsgReader.Rtf
{
    /// <summary>
    /// Rtf line element
    /// </summary>
    public class DomLineBreak : DomElement
    {
        #region Properties
        public override string InnerText
        {
            get { return Environment.NewLine; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// initialize instance
        /// </summary>
        public DomLineBreak()
        {
            Locked = true;
        }
        #endregion

        #region ToString
        public override string ToString()
        {
            return "linebreak";
        }
        #endregion
    }
}