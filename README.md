# 5204-SamBebenek-PetGrooming
This project is for the Excercise 2 Assignment for 5204 Mobile Development. This web app created in ASP.NET MVC acts as a partial CMS application for a 
pet grooming business, and contains CRUD functionality for pet and species tables. The app is styled with Bootstrap. This app uses code segments from the class' example code.
<h2>Libraries Used:</h2>
<ul>
<li>Bootstrap</li>
<li>jQuery</li>
</ul>
<i>Below cloning instructions are on how to get the EF6 database working, and are from the original class example</i>
<h2>Updating an existing repo steps</h2>
<ol>
  <li>run git stash to avoid conflicts between remote and local repository</li>
  <li>Use git cmd to run git pull origin master</li>
  <li>Tools > Nuget Package Manager > Package Manager Console</li>
  <li>run command "update-database"</li>
  </ol>

<h2>Cloning a new repo steps</h2>
<ol>
  <li>use git cmd to clone repository</li>
  <li>create "App_Data" Folder</li>
  <li>right click project "Build" -> 1 successful</li>
  <li>run "Enable-Migrations" in package manager console</li>
  <li>run "Add-Migration initial" in package manager console</li>
  <li>run "Update-Database"</li>
  <li>navigate to View/Pet/List.cshtml</li>
  <li>run List.cshtml and click yes to SSL self signed prompts</li>
  <li>should just show list of pets with no data</li>
  </ol>

