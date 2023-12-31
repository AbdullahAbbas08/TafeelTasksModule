//Autogenerated code

namespace Models.Enums
{
    public enum AnnotationTypeEnum
    {
        ///<summary>Signature</summary>
        Signature = 1,

        ///<summary>Trkeen</summary>
        Trkeen = 2,

        ///<summary>Stamp</summary>
        Stamp = 3,

        ///<summary>Barcode</summary>
        Barcode = 4,

        ///<summary>QR</summary>
        QR = 5,

        ///<summary>Text</summary>
        Text = 6,

        ///<summary>Free</summary>
        Free = 7,

        ///<summary>QR_Print</summary>
		QR_Print = 8,

        ///<summary>Highlight</summary>
        Highlight = 9,

        ///<summary>Note</summary>
        Note = 10,
        EmployeeStamp = 11,

    }


    public enum QrAnnotationTypeEnum
    {
        ///<summary>QR</summary>
        QR = (int)AnnotationTypeEnum.QR,

        ///<summary>QR_Print</summary>
		QR_Print = (int)AnnotationTypeEnum.QR_Print,
    }
}
