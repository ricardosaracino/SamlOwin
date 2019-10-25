using Microsoft.Xrm.Sdk.Query;
using System;

namespace XrmFramework.Attributes
{
    public sealed  class LinkAttribute : Attribute
    {
        // string linkFromEntityName
        // string linkToEntityName
        // string linkFromAttributeName
        // string linkToAttributeName
        // JoinOperator joinOperator

        public string LinkToAttributeName { get; set; } = null;

        public JoinOperator JoinOperator { get; set; }
    }
}