using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Microsoft.Xrm.Sdk;
using XrmFramework.Attributes;

namespace XrmFramework
{
    public class Transformer
    {
        public static Entity GetEntityFromDto<TModelType, TDtoType>(TDtoType dto)
        {
            var modelType = typeof(TModelType);

            var dtoType = typeof(TDtoType);

            var entityAnnotation = XrmAttribute.GetEntityAttribute(modelType);

            if (entityAnnotation == null)
            {
                throw new Exception("The attribute was not found.");
            }

            var entity = new Entity(entityAnnotation.EntityName);

            var dtoProperties = dtoType.GetProperties().ToDictionary(p => p.Name, p => p);

            foreach (var property in modelType.GetProperties())
            {
                var columnAnnotation = XrmAttribute.GetColumnAttribute(property);

                if (columnAnnotation == null)
                {
                    continue;
                }

                if (!dtoProperties.ContainsKey(property.Name))
                {
                    continue;
                }


                var propertyValue = dtoProperties[property.Name].GetValue(dto);

                var idAttribute = XrmAttribute.GetIdAttribute(property);

                if (idAttribute != null)
                {
                    if (propertyValue != null)
                    {
                        entity.Id = (Guid) propertyValue;
                    }

                    continue;
                }

                if (columnAnnotation.AttributeType == "PicklistType")
                {
                    if (propertyValue == null)
                    {
                        entity.Attributes.Add(columnAnnotation.LogicalName, null);
                    }
                    else
                    {
                        entity.Attributes.Add(columnAnnotation.LogicalName,
                            new OptionSetValue(Convert.ToInt32(propertyValue)));
                    }

                    continue;
                }

                if (columnAnnotation.AttributeType == "LookupType" && propertyValue != null)
                {
                    var reference = GetEntity(propertyValue);
                    entity.Attributes.Add(columnAnnotation.LogicalName,
                        new EntityReference(reference.LogicalName, reference.Id));
                    continue;
                }

                entity.Attributes.Add(columnAnnotation.LogicalName, propertyValue);
            }

            return entity;
        }

        public static TDtoType GetDtoFromEntity<TModelType, TDtoType>(Entity entity)
        {
            return GetDtoFromModel<TModelType, TDtoType>(GetModel<TModelType>(entity));
        }

        public static TDtoType GetDtoFromModel<TModelType, TDtoType>(TModelType model)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<TModelType, TDtoType>());

            var mapper = new Mapper(config);

            return mapper.Map<TDtoType>(model);
        }


        public static Entity GetEntity<TModelType>(TModelType model)
        {
            return GetEntityFromDto<TModelType, TModelType>(model);
        }

        public static TModelType GetModel<TModelType>(Entity entity)
        {
            var modelType = typeof(TModelType);

            var model = (TModelType) Activator.CreateInstance(modelType);

            var entityName = XrmAttribute.GetEntityName(modelType);

            if (entityName != entity.LogicalName)
            {
                throw new Exception("Model not same as Entity");
            }

            foreach (PropertyInfo property in modelType.GetProperties())
            {
                var columnAnnotation = XrmAttribute.GetColumnAttribute(property);

                if (columnAnnotation == null)
                {
                    continue;
                }

                var attributeLogicalName = columnAnnotation.LogicalName;

                if (!entity.Attributes.Contains(attributeLogicalName))
                {
                    continue;
                }

                var attributeValue = entity.GetAttributeValue<object>(attributeLogicalName);

                if (columnAnnotation.AttributeType == "PicklistType")
                {
                    property.SetValue(model, ((OptionSetValue) attributeValue).Value);
                    continue;
                }

                if (columnAnnotation.AttributeType == "LookupType" && attributeValue != null)
                {
                    // bps_lookup.bps_name
                    var referenceModel = GeReferenceModel(columnAnnotation.LogicalName, property.PropertyType, entity);

                    property.SetValue(model, referenceModel);

                    continue;
                }

                property.SetValue(model, attributeValue);
            }

            return model;
        }

        private static object GeReferenceModel(string aliasPrefix, Type type, Entity entity)
        {
            var model = Activator.CreateInstance(type);

            var modelType = model.GetType();

            // Linked Aliased Attributes

            foreach (var property in modelType.GetProperties())
            {
                var columnAnnotation = XrmAttribute.GetColumnAttribute(property);

                if (columnAnnotation == null)
                {
                    continue;
                }

                var attributeLogicalName = string.Format("{0}.{1}", aliasPrefix, columnAnnotation.LogicalName);

                if (!entity.Attributes.Contains(attributeLogicalName))
                {
                    continue;
                }

                var aliasedValue = entity.GetAttributeValue<object>(attributeLogicalName);

                if (aliasedValue.GetType() != typeof(AliasedValue))
                {
                    continue;
                }

                var attributeValue = ((AliasedValue) aliasedValue).Value;

                if (columnAnnotation.AttributeType == "PicklistType")
                {
                    property.SetValue(model, ((OptionSetValue) attributeValue).Value);
                    continue;
                }

                if (columnAnnotation.AttributeType == "LookupType" && attributeValue != null)
                {
                    // bps_lookup.bps_lookup.bps_name
                    var referenceModel = GeReferenceModel(attributeLogicalName, property.PropertyType, entity);

                    property.SetValue(model, referenceModel);

                    continue;
                }

                property.SetValue(model, attributeValue);
            }

            return model;
        }

        private static object CreateReferenceModel(string logicalName, Type type, Entity entity)
        {
            var model = Activator.CreateInstance(type);

            var modelType = model.GetType();

            var entityReference = entity.GetAttributeValue<object>(logicalName);

            // bps_lookup.bps_lookup is a AliasedValue where bps_lookup is a EntityReference
            if (entityReference.GetType() != typeof(EntityReference))
            {
                throw new Exception("Attribute is not EntityReference");
            }

            var idProperty = XrmAttribute.FindFirstIdProperty(modelType);

            if (idProperty != null)
            {
                idProperty.SetValue(model, ((EntityReference) entityReference).Id);
            }
            else
            {
                throw new Exception("Model has not Id attribute");
            }

            var nameProperty = XrmAttribute.FindFirstNameProperty(modelType);

            if (nameProperty != null)
            {
                nameProperty.SetValue(model, ((EntityReference) entityReference).Name);
            }
            else
            {
                throw new Exception("Model has not Name attribute");
            }

            return model;
        }
    }
}