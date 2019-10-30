﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// Created via this command line: "C:\Users\ricardosaracino\AppData\Roaming\MscrmTools\XrmToolBox\Plugins\DLaB.EarlyBoundGenerator\crmsvcutil.exe" /url:"https://dev-csc-scc.api.crm3.dynamics.com/XRMServices/2011/Organization.svc" /namespace:"CrmEarlyBound" /out:"C:\Users\ricardosaracino\Projects\TodoNET\TodoNET\Entities\CrmServiceContext.cs" /servicecontextname:"CrmServiceContext" /codecustomization:"DLaB.CrmSvcUtilExtensions.Entity.CustomizeCodeDomService,DLaB.CrmSvcUtilExtensions" /codegenerationservice:"DLaB.CrmSvcUtilExtensions.Entity.CustomCodeGenerationService,DLaB.CrmSvcUtilExtensions" /codewriterfilter:"DLaB.CrmSvcUtilExtensions.Entity.CodeWriterFilterService,DLaB.CrmSvcUtilExtensions" /namingservice:"DLaB.CrmSvcUtilExtensions.NamingService,DLaB.CrmSvcUtilExtensions" /metadataproviderservice:"DLaB.CrmSvcUtilExtensions.Entity.MetadataProviderService,DLaB.CrmSvcUtilExtensions" 
//------------------------------------------------------------------------------

[assembly: Microsoft.Xrm.Sdk.Client.ProxyTypesAssemblyAttribute()]

namespace CrmEarlyBound
{
	
	/// <summary>
	/// Represents a source of entities bound to a CRM service. It tracks and manages changes made to the retrieved entities.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "9.0.0.9479")]
	public partial class CrmServiceContext : Microsoft.Xrm.Sdk.Client.OrganizationServiceContext
	{
		
		/// <summary>
		/// Constructor.
		/// </summary>
		[System.Diagnostics.DebuggerNonUserCode()]
		public CrmServiceContext(Microsoft.Xrm.Sdk.IOrganizationService service) : 
				base(service)
		{
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="CrmEarlyBound.csc_PortalUser"/> entities.
		/// </summary>
		public System.Linq.IQueryable<CrmEarlyBound.csc_PortalUser> csc_PortalUserSet
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.CreateQuery<CrmEarlyBound.csc_PortalUser>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="CrmEarlyBound.csc_ProvinceOrState"/> entities.
		/// </summary>
		public System.Linq.IQueryable<CrmEarlyBound.csc_ProvinceOrState> csc_ProvinceOrStateSet
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.CreateQuery<CrmEarlyBound.csc_ProvinceOrState>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="CrmEarlyBound.csc_Volunteer"/> entities.
		/// </summary>
		public System.Linq.IQueryable<CrmEarlyBound.csc_Volunteer> csc_VolunteerSet
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.CreateQuery<CrmEarlyBound.csc_Volunteer>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="CrmEarlyBound.csc_VolunteerApplication"/> entities.
		/// </summary>
		public System.Linq.IQueryable<CrmEarlyBound.csc_VolunteerApplication> csc_VolunteerApplicationSet
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.CreateQuery<CrmEarlyBound.csc_VolunteerApplication>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="CrmEarlyBound.csc_VolunteerEmergencyContact"/> entities.
		/// </summary>
		public System.Linq.IQueryable<CrmEarlyBound.csc_VolunteerEmergencyContact> csc_VolunteerEmergencyContactSet
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.CreateQuery<CrmEarlyBound.csc_VolunteerEmergencyContact>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="CrmEarlyBound.csc_VolunteerLanguage"/> entities.
		/// </summary>
		public System.Linq.IQueryable<CrmEarlyBound.csc_VolunteerLanguage> csc_VolunteerLanguageSet
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.CreateQuery<CrmEarlyBound.csc_VolunteerLanguage>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="CrmEarlyBound.csc_VolunteerReference"/> entities.
		/// </summary>
		public System.Linq.IQueryable<CrmEarlyBound.csc_VolunteerReference> csc_VolunteerReferenceSet
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.CreateQuery<CrmEarlyBound.csc_VolunteerReference>();
			}
		}
	}
	
	internal sealed class EntityOptionSetEnum
	{
		
		[System.Diagnostics.DebuggerNonUserCode()]
		public static System.Nullable<int> GetEnum(Microsoft.Xrm.Sdk.Entity entity, string attributeLogicalName)
		{
			if (entity.Attributes.ContainsKey(attributeLogicalName))
			{
				Microsoft.Xrm.Sdk.OptionSetValue value = entity.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>(attributeLogicalName);
				if (value != null)
				{
					return value.Value;
				}
			}
			return null;
		}
		
		[System.Diagnostics.DebuggerNonUserCode()]
		public static System.Collections.Generic.IEnumerable<T> GetMultiEnum<T>(Microsoft.Xrm.Sdk.Entity entity, string attributeLogicalName)
		
		{
			Microsoft.Xrm.Sdk.OptionSetValueCollection value = entity.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValueCollection>(attributeLogicalName);
			System.Collections.Generic.List<T> list = new System.Collections.Generic.List<T>();
			list.AddRange(System.Linq.Enumerable.Select(value, v => (T)(object)v.Value));
			return list;
		}
		
		[System.Diagnostics.DebuggerNonUserCode()]
		public static Microsoft.Xrm.Sdk.OptionSetValueCollection GetMultiEnum<T>(Microsoft.Xrm.Sdk.Entity entity, string attributeLogicalName, System.Collections.Generic.IEnumerable<T> values)
		
		{
			Microsoft.Xrm.Sdk.OptionSetValueCollection collection = new Microsoft.Xrm.Sdk.OptionSetValueCollection();
			collection.AddRange(System.Linq.Enumerable.Select(values, v => new Microsoft.Xrm.Sdk.OptionSetValue((int)(object)v)));
			return collection;
		}
	}
}