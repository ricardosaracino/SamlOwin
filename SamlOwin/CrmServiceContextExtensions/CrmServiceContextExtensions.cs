using System;
using System.Linq;
using CrmEarlyBound;

namespace SamlOwin.CrmServiceContextExtensions
{
    public static class CrmServiceContextExtensions
    {
        internal static csc_Volunteer GetVolunteerEntity(this ICrmServiceContext apiController, Guid volunteerId)
        {
            return (from volunteerEntity in apiController.GetCrmServiceContext().csc_VolunteerSet
                where volunteerEntity.Id.Equals(volunteerId)
                select volunteerEntity).Single();
        }
        
        internal static csc_VolunteerApplication GetVolunteerApplicationEntity(this ICrmServiceContext apiController,
            Guid volunteerApplicationId, Guid volunteerId)
        {
            return (from volunteerApplicationEntity in apiController.GetCrmServiceContext().csc_VolunteerApplicationSet
                where volunteerApplicationEntity.csc_Volunteer.Id.Equals(volunteerId)
                where volunteerApplicationEntity.Id.Equals(volunteerApplicationId)
                select volunteerApplicationEntity).Single();
        }
    }
}