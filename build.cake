#addin nuget:?package=Microsoft.Web.Administration&version=11.1.0

var target = Argument("target", "Default");

Task("Default")
.Does(()=>{
 Information("Hello World");
});

RunTarget(target);