using System.ComponentModel;
using System.Xml.Serialization;

namespace trx_tools.Core.Models.TestEntries;

[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
public class TestEntry
{
    [XmlAttribute("testId")] public required string TestId { get; set; }

    [XmlAttribute("executionId")] public required string ExecutionId { get; set; }

    [XmlAttribute("testListId")] public required string TestListId { get; set; }
}