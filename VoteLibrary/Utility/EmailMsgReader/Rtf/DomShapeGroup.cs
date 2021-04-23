namespace DocumentServices.Modules.Readers.MsgReader.Rtf
{
    /// <summary>
    /// Rtf shape group
    /// </summary>
    public class DomShapeGroup : DomElement
    {
        #region Properties
        /// <summary>
        /// External attributes
        /// </summary>
        public StringAttributeCollection ExtAttrbutes { get; set; }
        #endregion

        #region Constructor
        public DomShapeGroup()
        {
            ExtAttrbutes = new StringAttributeCollection();
        }
        #endregion

        #region ToString
        public override string ToString()
        {
            return "ShapeGroup";
        }
        #endregion
    }
}