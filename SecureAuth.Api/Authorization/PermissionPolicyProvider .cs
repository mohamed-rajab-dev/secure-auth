using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using SecureAuth.Api.Attributes;

namespace SecureAuth.Api.Authorization
{
    public class PermissionPolicyProvider(IOptions<AuthorizationOptions> options) : DefaultAuthorizationPolicyProvider(options)
    {
        public override Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith(HasPermissionAttribute.PolicyPrefix, StringComparison.OrdinalIgnoreCase))
            {
                var permission = policyName[HasPermissionAttribute.PolicyPrefix.Length..];

                var policy = new AuthorizationPolicyBuilder()
                    .AddRequirements(
                        new PermissionRequirement(permission))
                    .Build();

                return Task.FromResult<AuthorizationPolicy?>(policy);

            }

            return base.GetPolicyAsync(policyName);
        }
    }
}
