# blazorwasm-cookie-auth
Blazor WASM project that uses cookie authentication to authenticate users. It uses `.NET 8 RC2`.

## Create a project
1. Create a new repo in Github.
2. Clone that into Rider.
   
   <img width="400" alt="image" src="https://github.com/affableashish/blazorwasm-cookie-auth/assets/30603497/d7006b57-f41d-4ac2-a126-bd78c35c5282">
3. Open that folder in Finder. And copy the location.
   
   <img width="450" alt="image" src="https://github.com/affableashish/blazorwasm-cookie-auth/assets/30603497/ec28628f-eb2a-4d6b-b2ac-c061cc7d6bda">

5. Go to Rider's Local Terminal and create the project.
   
   <img width="1000" alt="image" src="https://github.com/affableashish/blazorwasm-cookie-auth/assets/30603497/46d541a2-c4a4-4cf1-a383-694baf949349">

   ````
   dotnet new blazor -int webassembly -au Individual -n BlazorWASM.Backend -o .
   ````
   1. `-int webassembly` means create a new Blazor Web App with Webassembly hosting the interactive components.
   2. `-au Individual` means use Individual Authentication which uses AspNetCOre Identity.
   3. `-n BlazorWASM.Backend` means name the project as _BlazorWASM.Backend_. In this case the WASM project is served by a AspNetCore web app, that's why I named it as **Backend** (backend for frontend, frontend being WASM app).
   4. `-o .` means put the output of the command in the current folder.

6. Rename the client project to just `BlazorWASM.Frontend`.
   So first, find the usage of `BlazorWASM.Backend.Client`.
   
   <img width="750" alt="image" src="https://github.com/affableashish/blazorwasm-cookie-auth/assets/30603497/938ac03f-fe35-4f0e-97a7-e13e3728b9f2">
   
   Change it manually, because Rider wasn't able to do it with `Right Click the project -> Edit -> Rename`.

   Don't bother with `bin` and `obj` folders, just delete them.
   
   <img width="650" alt="image" src="https://github.com/affableashish/blazorwasm-cookie-auth/assets/30603497/e2b094c4-ff79-4be3-8576-46ea6f021db3">
   
   Replace `Backend.Client` with `Frontend`.

   Finally, change the project reference in `.sln` file, rename the project folder name and the project file name.

7. Rename the `BlazorWASM.Backend.sln` file to just `BlazorWASM.sln`.
8. The solution should look like this at this point and should build just fine.
   
   <img width="250" alt="image" src="https://github.com/affableashish/blazorwasm-cookie-auth/assets/30603497/8262876b-b71a-45a2-8727-70f6f66250ee">

## Run the project
1. Select the Profile `BlazorWASM.Backend: https` and hit Debug. 

<img width="350" alt="image" src="https://github.com/affableashish/blazorwasm-cookie-auth/assets/30603497/12649d16-cf87-48cb-b466-c37e7fd8e320">

2. Register an user.
   
   <img width="500" alt="image" src="https://github.com/affableashish/blazorwasm-cookie-auth/assets/30603497/9d2056a5-c83c-4441-ac10-8df8f50ee849">

   For eg: Username: `ashish@example.com` and Password: `Password1!`
   
3. Log In

4. Access protected page.
   
   <img width="500" alt="image" src="https://github.com/affableashish/blazorwasm-cookie-auth/assets/30603497/9af36bef-b6f4-4ba1-b58d-12bd63d41552">

   View the cookie dealt out to the browser:
   
   <img width="650" alt="image" src="https://github.com/affableashish/blazorwasm-cookie-auth/assets/30603497/6d36574b-72b6-49a8-b1e0-a7cb2b2ccf38">

## View how the auth works

