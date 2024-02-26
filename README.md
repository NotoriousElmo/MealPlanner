## EF

~~~bash
dotnet tool update --global dotnet-ef

dotnet ef migrations add --project DAL --startup-project WebApp InitialCreate
~~~

## WebPage

~~~bash
dotnet tool install --global dotnet-aspnet-codegenerator

cd WebApp
dotnet aspnet-codegenerator razorpage -m Domain.Ingredient -outDir Pages/Ingredients -dc AppDbContext -udl --referenceScriptLibraries 
dotnet aspnet-codegenerator razorpage -m Domain.Product -outDir Pages/Products -dc AppDbContext -udl --referenceScriptLibraries     
dotnet aspnet-codegenerator razorpage -m Domain.Recipe -outDir Pages/Recipes -dc AppDbContext -udl --referenceScriptLibraries
dotnet aspnet-codegenerator razorpage -m Domain.RecipeIngredient -outDir Pages/RecipeIngredients -dc AppDbContext -udl --referenceScriptLibraries 


~~~
