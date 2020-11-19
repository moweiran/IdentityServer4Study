# IdentityServer4Study
IdentityServer4 Study

```
Pomelo.EntityFrameworkCore.MySql
Pomelo.EntityFrameworkCore.MySql.Design

add-migration InitialIdentityServerPersistedGrantDbMigration -c PersistedGrantDbContext -o Data/Migrations/IdentityServer/PersistedGrantDb
add-migration InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext -o Data/Migrations/IdentityServer/ConfigurationDb

drop table apiresourceclaims;
drop table apiresourceproperties;
drop table apiresourcescopes;
drop table apiresourcesecrets;
drop table apiscopeclaims;
drop table apiscopeproperties;
drop table clientclaims;
drop table clientcorsorigins;
drop table clientgranttypes;
drop table clientidprestrictions;
drop table clientpostlogoutredirecturis;
drop table clientproperties;
drop table clientredirecturis;
drop table persistedgrants;
drop table devicecodes;
drop table identityresourceclaims;
drop table identityresourceproperties;
drop table clientscopes;
drop table clientsecrets;
drop table identityresources;
drop table apiscopes;
drop table apiresources;
drop table clients;
drop table __efmigrationshistory;
```