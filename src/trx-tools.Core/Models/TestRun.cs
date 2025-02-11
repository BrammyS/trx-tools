using System.ComponentModel;
using System.Xml.Serialization;
using trx_tools.Core.Models.Results;
using trx_tools.Core.Models.TestDefinitions;
using trx_tools.Core.Models.TestEntries;
using trx_tools.Core.Models.TestLists;

namespace trx_tools.Core.Models;

[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
[XmlRoot(Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010", IsNullable = false)]
public class TestRun
{
    public required Times Times { get; set; }

    public required TestSettings.TestSettings TestSettings { get; set; }

    [XmlArrayItem("UnitTestResult", IsNullable = false)]
    public required UnitTestResult[] Results { get; set; }

    [XmlArrayItem("UnitTest", IsNullable = false)]
    public required UnitTest[] TestDefinitions { get; set; }

    [XmlArrayItem("TestEntry", IsNullable = false)]
    public required TestEntry[] TestEntries { get; set; }

    [XmlArrayItem("TestList", IsNullable = false)]
    public required TestList[] TestLists { get; set; }

    public required ResultSummary.ResultSummary ResultSummary { get; set; }

    [XmlAttribute("id")] public required string Id { get; set; }

    [XmlAttribute("name")] public required string Name { get; set; }

    [XmlAttribute("runUser")] public required string RunUser { get; set; }
}