
### Overview

This app was built due to a request from Sql Server DBA's for devs to provide
documenation on database tables. They wanted, at a glance, to be able to get a high-
level idea of their purposes, without being dependent upon a developer. They had
a tool that could auto-generate documenation about the database based on a table's
and its column's extended properties. The UI in Sql Server Managment Studio to accomplish
this is very clunky - requiring numerous clicks and popup windows to add properties
for each table and for each column individually.

Therefore, this app was written to ease the pain of adding the exteneded properties and
make it harder to miss tables and/or columns. Additionally, it provides a good jumping 
off point for some experimentation. Some of the technologies/libraries/frameworks @ play are: 

1. [ASP.NET MVC](http://www.asp.net/mvc)
2. [ASP.NET Web API](http://www.asp.net/web-api)
3. [Nancy](http://nancyfx.org/)
4. [Angular](https://angularjs.org/)
5. [Bootstrap](http://getbootstrap.com/)
6. [Gulp](http://gulpjs.com/)
7. [Angular UI-Bootstrap](https://angular-ui.github.io/bootstrap/)
8. [Toastr](https://github.com/CodeSeven/toastr)
9. [ng-table](https://github.com/esvit/ng-table)

### Configuration of Database Connections

Within the web application's Database directory are 2 small .mdf files. In the web.configthe connection strings in the web.config
are configured like so:
```
<add name="Sample 1" connectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\sample.mdf;Integrated Security=True;Connect Timeout=30"/>

<add name="Sample 2" connectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\sample2.mdf;Integrated Security=True;Connect Timeout=30"/>
```

The connection string is setup to attempt to connect automatically to the mdf's, but there is one snag. The `|DataDirectory|` section 
tells ASP.NET to look for the database in App_Data. They are in the Database folder because the App_Data folder doesn't want to commit, but that's
okay, because we have to alter the connection anyway for Nancy. Within the 2 databases are a extremely small sampling of tables. 
Additional connection strings **should** be able to be added by simply adding them to the connectionStrings section. I say should 
because I've, as of yet, been unable to just drop in an actual Sql Server database server's connection string to see what happens. 

In the next section I will go over what needs to be done in order for the connection to be setup to optionally use the Nancy API.

### Nancy API Configuration

#### Database
In order for a database to have the option to use the Nancy API implementation, a connection string with the same name has to exist
in the NancyApi project's web.config. It doesn't need an actual connection string value, but the names have to match between the 2 
projects. When getting a list of the connnections, when connection names match between the 2 sources, a checkbox for enabling usage
of the Nancy API will appear when that connection is selected from the dropdown. Below is the connection string setting up the 
"Sample 1" connection for Nancy `<add name="Sample 1" connectionString=""/>`

Circling back to the reason for the mdf's being in a Databases folder instead of App_Data: Because, I wanted to put the .mdf's 
in a web project folder, it presented some very unique challenges when the 
connection string from the web app is passed to the Nancy API. For local files the AttachDbFilename value looks like
`|DataDirectory|\sample.mdf`. The DataDirectory portion tells ASP.NET to look in App_Data directory. Trying to use that connection
in the NancyApi project, resulted in it looking in the NancyApi project's App_Data. To work around this, in the Web API 
DocumenterController, when returning the connections I am replacing `|DataDirectory|` with a mapped server path to the .mdf's.
```
var connectionString = connection.ConnectionString;
if (connectionString.Contains("|DataDirectory|"))
{
    connectionString = connectionString.Replace("|DataDirectory|", System.Web.HttpContext.Current.Server.MapPath("\\Databases"));
}
```
It works well and the transformation only occurs for connections that are attached files.

#### API 
In addtion to the database connection required for Nancy to be setup. The web app config must have an app setting for the Nancy API's
host. This is setup for localhost right now and should work for free as long as the application is run locally on the specified ports.
`<add key="NancyApiPath" value="http://localhost:56771/"/>`

#### Taking Nancy Further
The Nancy API was added late just to see how easy it would be to get going and if the app could support the "hot-swapping" of the API
implementations (the other is ASP.NET Web API). There were a couple minor gotchas: 

  * I had to add an ignore to the web apps RouteConfig to ignore the nancy calls
  * In the web app global.asax, I configured the models returned by ASP.NET Web API to be camel-cased the same as Nancy's responses

Other than those the API was very straight forward to get working and most of the work was in refactoring the angular services to 
communicate with both APIs. That said, there are things missing or that I would've liked to add and need to research some more:

* I would've liked to get the content negotiation setup to allow for me to play with the api a bit more directly
* Better model validation
* IOC implementation

### Bundling and Minification Setup
Bundling and Minification is turned off by default in the web.config but can be turned on by changing 
the appSetting: `<add key="OptimizeResources" value="false"/>` to true. It is configured in the Web.Release.config to be true
when published!

Gulps was used for the actual bundling and minification.

### Gulp Notes

[Gulp](http://gulpjs.com/) was used for handful of tasks: 

* cleaning - wiping the `dist/` directory
* linting - evaluating js code
* packaging - bundling and minifying
* template caching - setup of angular's template cache
* a watch task to kick off lintin and re-packaging when files are changed

The gulpfile is just the setup for the tasks. The tasks themselves are split into indivitual files in the `gulp/` directory. 
Additionally the plugin & config definitions are in their own files. 

Using the [VS Task Runner Explorer](https://visualstudiogallery.msdn.microsoft.com/8e1b4368-4afb-467a-bc13-9650572db708) I was 
able to quickly setup the following bindings:

1. After Build - runs default (lint & package-resources)
2. Clean - runs cleaner
3. Project Open - lint

One gotcha that I don't like is that I originally had the watcher task setup to run as part of the default task, but doing
so with the build binding resulted in a never ending build. This is because the watcher task keeps running - keeping the 
MSBuild from "completing".

### What's Left to Do?
There are a couple things missing that hopefully I'll circle back to that have this feeling a incomplete:

1. Unit Testing - This is the first priority!
2. IOC Implemenation - Right now I'm using a manually method of dependency injection I've has success with before, but 
    would like to put in place a framework for it
3. [Content Negotiation](https://github.com/NancyFx/Nancy/wiki/Content-Negotiation) for Nancy
4. User Input Validation - There is some around requiring fields and validation before and after data layer interaction, 
    but there is more that can be done (i.e. validation on the lenght of inputs for example)
