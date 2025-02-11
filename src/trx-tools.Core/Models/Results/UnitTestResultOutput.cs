namespace trx_tools.Core.Models.Results;

[Serializable]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
public class UnitTestResultOutput
{
    public required string StdOut { get; set; }

    public required ErrorInfo ErrorInfo { get; set; }
}