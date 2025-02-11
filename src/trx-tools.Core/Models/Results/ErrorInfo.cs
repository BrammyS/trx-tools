using System.ComponentModel;
using System.Xml.Serialization;

namespace trx_tools.Core.Models.Results;

[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
public class ErrorInfo
{
    public required string Message { get; set; }

    public required string StackTrace { get; set; }
}