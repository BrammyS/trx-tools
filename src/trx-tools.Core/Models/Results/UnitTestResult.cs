namespace trx_tools.Core.Models.Results;

[Serializable]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
public class UnitTestResult
{
    public required UnitTestResultOutput Output { get; set; }

    [System.Xml.Serialization.XmlAttributeAttribute("executionId")]
    public required string ExecutionId { get; set; }

    [System.Xml.Serialization.XmlAttributeAttribute("testId")]
    public required string TestId { get; set; }

    [System.Xml.Serialization.XmlAttributeAttribute("testName")]
    public required string TestName { get; set; }

    [System.Xml.Serialization.XmlAttributeAttribute("computerName")]
    public required string ComputerName { get; set; }

    [System.Xml.Serialization.XmlAttributeAttribute("duration")]
    public required string Duration { get; set; }

    [System.Xml.Serialization.XmlAttributeAttribute("startTime")]
    public DateTime StartTime { get; set; }

    [System.Xml.Serialization.XmlAttributeAttribute("endTime")]
    public DateTime EndTime { get; set; }

    [System.Xml.Serialization.XmlAttributeAttribute("testType")]
    public required string TestType { get; set; }

    [System.Xml.Serialization.XmlAttributeAttribute("outcome")]
    public required string Outcome { get; set; }

    [System.Xml.Serialization.XmlAttributeAttribute("testListId")]
    public required string TestListId { get; set; }

    [System.Xml.Serialization.XmlAttributeAttribute("relativeResultsDirectory")]
    public required string RelativeResultsDirectory { get; set; }
}