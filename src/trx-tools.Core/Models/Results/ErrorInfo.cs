namespace trx_tools.Core.Models.Results;

[Serializable]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
public class ErrorInfo
{
    public required string Message { get; set; }

    public required string StackTrace { get; set; }
}