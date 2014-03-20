using System.Runtime.Serialization;

namespace Core.Common.Core
{
    [DataContract]
    public class EntityBase : IExtensibleDataObject
    {
        public ExtensionDataObject ExtensionData { get; set; }
    }
}
