﻿using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace SportCommunityRM.WebSite.Models
{
    public class ClaimPoliciesConstants
    {
        private const string CreateCoachesPolicyName = "Create Coaches";
        private const string DeleteCoachesPolicyName = "Delete Coaches";
        private const string CreateActivitiesPolicyName = "Create Activities";
        private const string DeleteActivitiesPolicyName = "Delete Activities";
        private const string EditActivitiesPolicyName = "Edit Activities";
        private const string CreateTeamsPolicyName = "Create Teams";
        private const string DeleteTeamsPolicyName = "Delete Teams";
        private const string EditTeamsPolicyName = "Edit Teams";

        public static KeyValuePair<string, string> CreateCoaches = new KeyValuePair<string, string>(nameof(CreateCoaches), CreateCoachesPolicyName);
        public static KeyValuePair<string, string> DeleteCoaches = new KeyValuePair<string, string>(nameof(DeleteCoaches), DeleteCoachesPolicyName);
        public static KeyValuePair<string, string> CreateActivities = new KeyValuePair<string, string>(nameof(CreateActivities), CreateActivitiesPolicyName);
        public static KeyValuePair<string, string> DeleteActivities = new KeyValuePair<string, string>(nameof(DeleteActivities), DeleteActivitiesPolicyName);
        public static KeyValuePair<string, string> EditActivities = new KeyValuePair<string, string>(nameof(EditActivities), EditActivitiesPolicyName);
        public static KeyValuePair<string, string> CreateTeams = new KeyValuePair<string, string>(nameof(CreateTeams), CreateTeamsPolicyName);
        public static KeyValuePair<string, string> DeleteTeams = new KeyValuePair<string, string>(nameof(DeleteTeams), DeleteTeamsPolicyName);
        public static KeyValuePair<string, string> EditTeams = new KeyValuePair<string, string>(nameof(EditTeams), EditTeamsPolicyName);

        public static IEnumerable<Claim> AdministratorClaims
        {
            get
            {
                return new[]
                {
                    CreateCoaches,
                    DeleteCoaches,
                    CreateActivities,
                    DeleteActivities,
                    EditActivities,
                    CreateTeams,
                    DeleteTeams,
                    EditTeams
                }
                .Select(claim => new IdentityUserClaim<string>
                {
                    ClaimType = claim.Value,
                    ClaimValue = claim.Value
                }.ToClaim());
            }
        }

        public static IEnumerable<Claim> CoachesClaims
        {
            get
            {
                return new[]
                {
                    CreateActivities,
                    DeleteActivities,
                    EditActivities
                }
                .Select(claim => new IdentityUserClaim<string>
                {
                    ClaimType = claim.Value,
                    ClaimValue = claim.Value
                }.ToClaim());
            }
        }

        public static IEnumerable<KeyValuePair<string, string>> AvailableClaims
        {
            get
            {
                return new []
                {
                    CreateCoaches,
                    DeleteCoaches,
                    CreateActivities,
                    DeleteActivities,
                    EditActivities,
                    CreateTeams,
                    DeleteTeams,
                    EditTeams
                };
            }
        }
    }
}
