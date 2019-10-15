﻿using System;

namespace XrmFramework.Attributes
{
    // [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method)]
    public class ColumnAttribute : Attribute
    {
        // logicalName

        // maxLength

        // attributeType

        public ColumnAttribute(string logicalName)
        {
            this.LogicalName = logicalName;
        }

        public virtual string LogicalName { get; }

        public virtual int Length { get; set; }

        public virtual string AttributeType { get; set; }
    }
}

/**
"entityLogicalName": "bps_todo",
"logicalName": "createdonbehalfbyyominame",
"attributeType": "StringType",
"required": "SystemRequired",
"format": "Text",
"maxLength": 100,
"autoNumberFormat": null
*/