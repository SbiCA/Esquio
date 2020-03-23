﻿using Esquio.UI.Api.Shared.Models.Users.My;
using Serilog;

namespace Esquio.UI.Client.Services
{
    public interface IPolicyBuilder
    {
        Policy Build(MyResponse my);
    }

    public class DefaultPolicyBuilder : IPolicyBuilder
    {
        private readonly PolicySubject[] _defaultSubjects = new PolicySubject[]
        {
            PolicySubject.Product,
            PolicySubject.Feature,
            PolicySubject.Toggle,
            PolicySubject.Ring,
        };

        private readonly PolicySubject[] _allSubjects = new PolicySubject[]
        {
            PolicySubject.Product,
            PolicySubject.Feature,
            PolicySubject.Toggle,
            PolicySubject.Ring,
            PolicySubject.Token,
            PolicySubject.Permission,
            PolicySubject.ApiKeys
        };


        public Policy Build(MyResponse my)
        {
            var policy = new Policy();

            var actAs = ActAs.From(my.ActAs);

            if (actAs == ActAs.Reader )
            {
                policy.Allow(PolicyAction.Read, _defaultSubjects);
            }
            else if (actAs == ActAs.Contributor || actAs == ActAs.Management)
            {
                policy.Allow(PolicyAction.Read, _defaultSubjects);
                policy.Allow(PolicyAction.Modify, _defaultSubjects);
            }
            else if (actAs == ActAs.Management)
            {
                policy.Allow(PolicyAction.Read, _allSubjects);
                policy.Allow(PolicyAction.Modify, _allSubjects);
            }

            return policy;
        }
    }
}