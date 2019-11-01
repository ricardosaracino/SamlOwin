using System;

namespace SamlOwin.Models
{
    /// <summary>
    /// Maps a Microsoft.Xrm.Sdk.EntityReference
    /// </summary>
    public class ReferenceRequest
    {
        /// <summary>
        /// Entity Primary Id Attribute
        /// </summary>
        public Guid Id { get; set; }
    }
}