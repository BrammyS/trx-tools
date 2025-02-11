using trx_tools.Core.Models.Results;
using trx_tools.Core.Models.TestDefinitions;
using trx_tools.Core.Models.TestEntries;
using trx_tools.Core.Models.TestLists;

namespace trx_tools.Core.Models;

[Serializable]
[System.ComponentModel.DesignerCategory("code")]
[System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
[System.Xml.Serialization.XmlRoot(Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010", IsNullable = false)]
public class TestRun
{
    public required Times Times { get; set; }

    public required TestSettings.TestSettings TestSettings { get; set; }

    [System.Xml.Serialization.XmlArrayItemAttribute("UnitTestResult", IsNullable = false)]
    public required UnitTestResult[] Results { get; set; }

    [System.Xml.Serialization.XmlArrayItemAttribute("UnitTest", IsNullable = false)]
    public required UnitTest[] TestDefinitions { get; set; }

    [System.Xml.Serialization.XmlArrayItemAttribute("TestEntry", IsNullable = false)]
    public required TestEntry[] TestEntries { get; set; }

    [System.Xml.Serialization.XmlArrayItemAttribute("TestList", IsNullable = false)]
    public required TestList[] TestLists { get; set; }

    public required ResultSummary.ResultSummary ResultSummary { get; set; }

    [System.Xml.Serialization.XmlAttributeAttribute("id")]
    public required string Id { get; set; }

    [System.Xml.Serialization.XmlAttributeAttribute("name")]
    public required string Name { get; set; }

    [System.Xml.Serialization.XmlAttributeAttribute("runUser")]
    public required string RunUser { get; set; }
}