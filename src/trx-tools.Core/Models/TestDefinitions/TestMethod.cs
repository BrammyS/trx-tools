using System.ComponentModel;
using System.Xml.Serialization;

namespace trx_tools.Core.Models.TestDefinitions;

[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
public class TestMethod
{
    [XmlAttribute("codeBase")] public required string CodeBase { get; set; }

    [XmlAttribute("adapterTypeName")] public required string AdapterTypeName { get; set; }

    [XmlAttribute("className")] public required string ClassName { get; set; }

    [XmlAttribute("name")] public required string Name { get; set; }
}