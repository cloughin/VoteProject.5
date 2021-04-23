﻿namespace DocumentServices.Modules.Readers.MsgReader.Rtf
{
    /// <summary>
    /// Rtf dom header
    /// </summary>
    public class DomHeader : DomElement
    {
        #region Properties
        /// <summary>
        /// Header style
        /// </summary>
        public RtfHeaderFooterStyle Style { get; set; }

        /// <summary>
        /// If the header has a content element
        /// </summary>
        public bool HasContentElement
        {
            get { return Util.HasContentElement(this); }
        }
        #endregion

        #region Constructor
        public DomHeader()
        {
            Style = RtfHeaderFooterStyle.AllPages;
        }
        #endregion

        #region ToString
        public override string ToString()
        {
            return "Header " + Style;
        }
        #endregion
    }

    public class DomFooter : DomElement
    {
        #region Constructor
        public DomFooter()
        {
            Style = RtfHeaderFooterStyle.AllPages;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Footer style
        /// </summary>
        public RtfHeaderFooterStyle Style { get; set; }
        
        /// <summary>
        /// If the footer has a content element
        /// </summary>
        public bool HasContentElement
        {
            get { return Util.HasContentElement(this); }
        }
        #endregion

        #region ToString
        public override string ToString()
        {
            return "Footer " + Style;
        }
        #endregion
    }
}