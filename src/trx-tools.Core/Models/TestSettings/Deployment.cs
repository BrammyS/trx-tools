using System.ComponentModel;
using System.Xml.Serialization;

namespace trx_tools.Core.Models.TestSettings;

[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
public class Deployment
{
    [XmlAttribute("runDeploymentRoot")] public required string RunDeploymentRoot { get; set; }
}