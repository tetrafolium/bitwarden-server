﻿using System;
using Bit.Core.Models.Table;
using System.Collections.Generic;
using Newtonsoft.Json;
using Bit.Core.Enums;
using System.Linq;

namespace Bit.Core.Models.Api
{
public class DomainsResponseModel : ResponseModel
{
public DomainsResponseModel(User user, bool excluded = true)
	: base("domains")
{
	if(user == null)
	{
		throw new ArgumentNullException(nameof(user));
	}

	EquivalentDomains = user.EquivalentDomains != null?
	                    JsonConvert.DeserializeObject<List<List<string> > >(user.EquivalentDomains) : null;

	var excludedGlobalEquivalentDomains = user.ExcludedGlobalEquivalentDomains != null?
	                                      JsonConvert.DeserializeObject<List<GlobalEquivalentDomainsType> >(user.ExcludedGlobalEquivalentDomains) :
						      new List<GlobalEquivalentDomainsType>();
	var globalDomains = new List<GlobalDomains>();
	var domainsToInclude = excluded ? Core.Utilities.StaticStore.GlobalDomains :
	                       Core.Utilities.StaticStore.GlobalDomains.Where(d => !excludedGlobalEquivalentDomains.Contains(d.Key));
	foreach(var domain in domainsToInclude)
	{
		globalDomains.Add(new GlobalDomains(domain.Key, domain.Value, excludedGlobalEquivalentDomains, excluded));
	}
	GlobalEquivalentDomains = !globalDomains.Any() ? null : globalDomains;
}

public IEnumerable<IEnumerable<string> > EquivalentDomains {
	get;
	set;
}
public IEnumerable<GlobalDomains> GlobalEquivalentDomains {
	get;
	set;
}


public class GlobalDomains
{
public GlobalDomains(
	GlobalEquivalentDomainsType globalDomain,
	IEnumerable<string> domains,
	IEnumerable<GlobalEquivalentDomainsType> excludedDomains,
	bool excluded)
{
	Type = globalDomain;
	Domains = domains;
	Excluded = excluded && (excludedDomains?.Contains(globalDomain) ?? false);
}

public GlobalEquivalentDomainsType Type {
	get;
	set;
}
public IEnumerable<string> Domains {
	get;
	set;
}
public bool Excluded {
	get;
	set;
}
}
}
}
