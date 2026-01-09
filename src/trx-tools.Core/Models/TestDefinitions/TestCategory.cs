using System.ComponentModel;
using System.Xml.Serialization;

namespace trx_tools.Core.Models.TestDefinitions;

[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
public class TestCategory
{
    [XmlElement("TestCategoryItem")]
    public required TestCategoryItem[] TestCategoryItems { get; set; }
}

[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
public class TestCategoryItem
{
    [XmlAttribute("TestCategory")]
    public required string TestCategory { get; set; }
}
