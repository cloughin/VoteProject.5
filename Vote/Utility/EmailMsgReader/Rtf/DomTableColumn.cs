namespace DocumentServices.Modules.Readers.MsgReader.Rtf
{
    /// <summary>
    /// Rtf table column
    /// </summary>
    public class DomTableColumn : DomElement
    {
        #region Properties
        /// <summary>
        /// Width
        /// </summary>
        public int Width { get; set; }
        #endregion

        #region ToString
        public override string ToString()
        {
            return "Column";
        }
        #endregion
    }
}