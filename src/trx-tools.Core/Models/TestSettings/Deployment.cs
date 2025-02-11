namespace trx_tools.Core.Models.TestSettings;

[Serializable]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
public class Deployment
{
    [System.Xml.Serialization.XmlAttributeAttribute("runDeploymentRoot")]
    public required string RunDeploymentRoot { get; set; }
}