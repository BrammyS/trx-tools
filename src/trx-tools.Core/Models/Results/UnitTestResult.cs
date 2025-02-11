using System.ComponentModel;
using System.Xml.Serialization;

namespace trx_tools.Core.Models.Results;

[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
public class UnitTestResult
{
    public required UnitTestResultOutput Output { get; set; }

    [XmlAttribute("executionId")] public required string ExecutionId { get; set; }

    [XmlAttribute("testId")] public required string TestId { get; set; }

    [XmlAttribute("testName")] public required string TestName { get; set; }

    [XmlAttribute("computerName")] public required string ComputerName { get; set; }

    [XmlAttribute("duration")] public required string Duration { get; set; }

    [XmlAttribute("startTime")] public DateTime StartTime { get; set; }

    [XmlAttribute("endTime")] public DateTime EndTime { get; set; }

    [XmlAttribute("testType")] public required string TestType { get; set; }

    [XmlAttribute("outcome")] public required string Outcome { get; set; }

    [XmlAttribute("testListId")] public required string TestListId { get; set; }

    [XmlAttribute("relativeResultsDirectory")]
    public required string RelativeResultsDirectory { get; set; }
}