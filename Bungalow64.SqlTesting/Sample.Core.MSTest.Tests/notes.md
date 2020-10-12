#Setup

Add appsettings.json, and set Build Action to Content and Copy to Output Directory as Copy if newer.  Added connection string to it.

Installed Microsoft.Extensions.Configuration.Json

#Environment

Installed SQL 2019 Developer and SSMS 2019.  Added database, set as Simple recovery model.

#Terminology

Template - a helper class that is used to generate a new row in a specific table, containing default values that can be overridden
Complex Template - a helper class that combines multiple templates together, allowing for complex scenarios to be generated
Resolver - a deferred function that is only called when the value is actually needed
Placeholder - an indication that a particular field requires a value of some kind

#Tasks

Next: Handle connection string as part of library?
Next: Look at RelationshipValidator - is it needed?

