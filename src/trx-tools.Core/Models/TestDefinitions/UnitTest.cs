using System.ComponentModel;
using System.Xml.Serialization;

namespace trx_tools.Core.Models.TestDefinitions;

[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
public class UnitTest
{
    public required Execution Execution { get; set; }

    public required TestMethod TestMethod { get; set; }

    public TestCategory? TestCategory { get; set; }

    [XmlAttribute("name")] public required string Name { get; set; }

    [XmlAttribute("storage")] public required string Storage { get; set; }

    [XmlAttribute("id")] public required string Id { get; set; }
}