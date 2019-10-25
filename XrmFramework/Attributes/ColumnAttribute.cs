﻿using System;

namespace XrmFramework.Attributes
{
    // [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method)]
    public sealed class ColumnAttribute : Attribute
    {
        // logicalName

        // maxLength

        // attributeType

        public ColumnAttribute(string logicalName)
        {
            LogicalName = logicalName;
        }

        public string LogicalName { get; }

        public int Length { get; set; }

        public string AttributeType { get; set; }
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