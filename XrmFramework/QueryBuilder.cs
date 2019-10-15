using System;
using System.Reflection;
using Microsoft.Xrm.Sdk.Query;
using XrmFramework.Attributes;

namespace XrmFramework
{
    public class QueryBuilder
    {
        public static QueryExpression Build<T>()
        {
            var modelType = typeof(T);

            var queryExpression = new QueryExpression(XrmAttribute.GetEntityName(modelType));

            var columnSet = new ColumnSet();

            foreach (var property in modelType.GetProperties())
            {
                var columnAnnotation = XrmAttribute.GetColumnAttribute(property);

                if (columnAnnotation == null)
                {
                    continue;
                }

                columnSet.AddColumn(columnAnnotation.LogicalName);

                if (columnAnnotation.AttributeType == "LookupType")
                {
                    queryExpression.LinkEntities.Add(BuildLinkEntity(queryExpression.EntityName, property));
                }
            }

            queryExpression.ColumnSet = columnSet;

            queryExpression.AddOrder("createdon", OrderType.Descending);

            return queryExpression;
        }

        // TODO Depth in LinkAnnotation
        // TODO OrderBy in LinkAnnotation

        public static LinkEntity BuildLinkEntity(string linkFromEntityName, PropertyInfo fromProperty,
            string aliasPrefix = "", int depth = 1)
        {
            var linkEntity = new LinkEntity
            {
                LinkFromEntityName = linkFromEntityName,
                LinkFromAttributeName = XrmAttribute.GetLogicalName(fromProperty)
            };

            var toType = fromProperty.PropertyType;

            var linkToEntityName = XrmAttribute.GetEntityName(toType);

            if (linkToEntityName == null)
            {
                throw new Exception("To Link Entity missing EntityName");
            }

            linkEntity.LinkToEntityName = linkToEntityName;

            linkEntity.EntityAlias = $"{aliasPrefix}{linkEntity.LinkFromAttributeName}";

            // TODO add LinkAnnotation stuff here

            var linkAnnotation = XrmAttribute.GetLinkAttribute(fromProperty);

            if (linkAnnotation != null)
            {
                linkEntity.JoinOperator = linkAnnotation.JoinOperator;
            }

            var toIdProperty = XrmAttribute.FindFirstIdProperty(toType);

            linkEntity.LinkToAttributeName = XrmAttribute.GetLogicalName(toIdProperty);

            var columnSet = new ColumnSet();

            foreach (var property in toType.GetProperties())
            {
                var columnAnnotation = XrmAttribute.GetColumnAttribute(property);

                if (columnAnnotation == null)
                {
                    continue;
                }

                columnSet.AddColumn(columnAnnotation.LogicalName);

                if (depth > 0 && columnAnnotation.AttributeType == "LookupType")
                {
                    linkEntity.LinkEntities.Add(BuildLinkEntity(linkEntity.LinkToEntityName, property,
                        $"{linkEntity.EntityAlias}.", --depth));
                }
            }

            linkEntity.Columns = columnSet;

            return linkEntity;
        }
    }
}