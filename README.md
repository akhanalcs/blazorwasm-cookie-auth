# blazorwasm-cookie-auth
This repo contains 2 different projects:
1. Blazor Web App that uses WebAssembly components for interactivity (`BlazorWASM.Backend.csproj` + `BlazorWASM.Frontend.csproj`). Basically it's a hosted WebAssembly app with prerendering enabled (by default in .NET 8 RC2).
2. Blazor Web App that uses SignalR for interactivity and runs on the server (`BlazorServer.csproj`). Basically a Blazor Server App.

`BlazorWASM.Backend` uses AspNetCore Identity with cookie authentication to sign in and manage users.
`BlazorServer` just sits alongside to demonstrate Single Sign On (SSO).

Any user who signs into `BlazorWASM.Backend` app gets automatically signed into `BlazorServer` app. Read on to see how it's done.

Also check out [this sample](https://github.com/dotnet/blazor-samples/tree/main/8.0/BlazorWebAssemblyStandaloneWithIdentity) for an example of standalone Blazor WASM with Identity. 

## Create a project
1. Create a new repo in Github.
2. Clone that into Rider.
   
   <img width="400" alt="image" src="https://github.com/akhanalcs/blazorwasm-cookie-auth/assets/30603497/d7006b57-f41d-4ac2-a126-bd78c35c5282">

3. Go to Rider's Local Terminal and create the project.
   ````
   Ashishs-MacBook-Pro:blazorwasm-cookie-auth ashishkhanal$ dotnet new blazor -int webassembly -au Individual -n BlazorWASM.Backend -o .
   ````
   1. `-int webassembly` means create a new Blazor Web App with Webassembly hosting the interactive components.
   2. `-au Individual` means use Individual Authentication which uses AspNetCOre Identity.
   3. `-n BlazorWASM.Backend` means name the project as _BlazorWASM.Backend_. In this case the WASM project is served by a AspNetCore web app, that's why I named it as **Backend** (backend for frontend, frontend being WASM app).
   4. `-o .` means put the output of the command in the current folder.

4. Rename the client project to just `BlazorWASM.Frontend`.
   So first, find the usage of `BlazorWASM.Backend.Client`.
   
   <img width="750" alt="image" src="https://github.com/akhanalcs/blazorwasm-cookie-auth/assets/30603497/938ac03f-fe35-4f0e-97a7-e13e3728b9f2">
   
   Change it manually, because Rider wasn't able to do it with `Right Click the project -> Edit -> Rename`.

   Don't bother with `bin` and `obj` folders, just delete them.
   
   <img width="650" alt="image" src="https://github.com/akhanalcs/blazorwasm-cookie-auth/assets/30603497/e2b094c4-ff79-4be3-8576-46ea6f021db3">
   
   Replace `Backend.Client` with `Frontend`.

   Finally, change the project reference in `.sln` file, rename the project folder name and the project file name.

5. Rename the `BlazorWASM.Backend.sln` file to just `BlazorApps.sln`.
6. The solution should look like this at this point and should build just fine.

   <img width="250" alt="image" src="https://github.com/akhanalcs/blazorwasm-cookie-auth/assets/30603497/dbcef89f-2fe7-4c35-a79c-cdbc1ee1dc58">

## Run the project
1. Select the Profile `BlazorWASM.Backend: https` and hit Debug. 

    <img width="350" alt="image" src="https://github.com/akhanalcs/blazorwasm-cookie-auth/assets/30603497/12649d16-cf87-48cb-b466-c37e7fd8e320">

2. Register an user.
   
   <img width="500" alt="image" src="https://github.com/akhanalcs/blazorwasm-cookie-auth/assets/30603497/9d2056a5-c83c-4441-ac10-8df8f50ee849">

   For eg: Username: `ashish@example.com` and Password: `Password1!`
   
3. Log In

4. Access protected page.
   
   <img width="500" alt="image" src="https://github.com/akhanalcs/blazorwasm-cookie-auth/assets/30603497/9af36bef-b6f4-4ba1-b58d-12bd63d41552">

   View the cookie dealt out to the browser:
   
   <img width="650" alt="image" src="https://github.com/akhanalcs/blazorwasm-cookie-auth/assets/30603497/6d36574b-72b6-49a8-b1e0-a7cb2b2ccf38">

## View how the auth works
This gets called when the app launches, or whenever you try navigating to any page.
`BlazorWASM.Backend/Identity/PersistingServerAuthenticationStateProvider.cs`

<img width="500" alt="image" src="https://github.com/akhanalcs/blazorwasm-cookie-auth/assets/30603497/7611a15a-3ee3-4554-9955-6a9ea950c4ab">

Go to Login Page, provide credentials and hit login, LoginUser method gets called.
`BlazorWASM.Backend/Components/Pages/Account/Login.razor`

<img width="600" alt="image" src="https://github.com/akhanalcs/blazorwasm-cookie-auth/assets/30603497/7b62fe57-b946-4024-b4b6-4f5c34349d4a">

After this method call completes, the cookie isn't set at this point.
The Login page is still spinning:

<img width="80" alt="image" src="https://github.com/akhanalcs/blazorwasm-cookie-auth/assets/30603497/dff3e7e1-0c01-45c5-a96c-6ae9a801c3f9">

Now back to `PersistingServerAuthenticationStateProvider.OnPersistingAsync` method.

<img width="450" alt="image" src="https://github.com/akhanalcs/blazorwasm-cookie-auth/assets/30603497/e600d45c-a9ae-4b20-8c08-42d8c33b533b">

During this pass, `principal.Identity?.IsAuthenticated` is false and cookie is still not set.

The control gets back to it right after, and at this time, the cookie is set and `principal.Identity?.IsAuthenticated` is true which will get the user persisted into `PersistentComponentState`.

Now I navigate to the `auth` page, this gets called again:

<img width="450" alt="image" src="https://github.com/akhanalcs/blazorwasm-cookie-auth/assets/30603497/a39b0a67-3634-4f7b-af99-53ca25863adb">

And `GetAuthenticationStateAsync` method gets called in the `PersistentAuthenticationStateProvider.cs` file of `Frontend` project because it needs to call `AuthenticationStateProvider` to get `Name`. For eg:

<img width="300" alt="image" src="https://github.com/akhanalcs/blazorwasm-cookie-auth/assets/30603497/1bce5cc7-a761-448c-967e-82a226e9c535">

This method gets the userinfo from the `PersistentComponentState` set by the backend. **[I'm not sure how this is passed to the frontend.]**
<img width="600" alt="image" src="https://github.com/akhanalcs/blazorwasm-cookie-auth/assets/30603497/22b69c0f-6028-4ba3-ab4b-7631bdc6130f">

This method gets called once when I navigate to this page. After that, it doesn't get called no matter how many times I come back to `/auth` page.

## Enable SSO on a new app
Let's login users to a new app after they have signed in with our Blazor WASM app.

### Add a new app to the solution
Note: This app should run on the same domain as our WASM app because cookie gets issued for a domain, `localhost` in this case.

<img width="550" alt="image" src="https://github.com/akhanalcs/blazorwasm-cookie-auth/assets/30603497/d8341b31-a5b9-492d-898f-c5bbf5bc08b1">

### Use data protection API to share cookies between applications

#### Install Redis on macOs
1. Follow this [guide](https://redis.io/docs/getting-started/installation/install-redis-on-mac-os/).

   When I tried this command, I got some errors:

   <img width="750" alt="image" src="https://github.com/akhanalcs/blazorwasm-cookie-auth/assets/30603497/79feea02-87f0-4fac-992b-03aba361f745">

   I fixed the errors by running:
   ````
   sudo chown -R $(whoami) /usr/local/var
   ````
   I then ran:
   ````
   Ashishs-MacBook-Pro:blazorwasm-cookie-auth ashishkhanal$ brew install redis
   ````
   And everything went well. 🎉

2. Run Redis server
   ````
   brew services start redis
   ````
3. Connect to Redis
   ````
   redis-cli
   ````
4. Check for Keys which will be empty at this time
   
   <img width="650" alt="image" src="https://github.com/akhanalcs/blazorwasm-cookie-auth/assets/30603497/0cd4c1e2-cfd8-4550-8def-7f603049d93a">

#### Install Nuget package that will allow you to persist keys to Redis
Install this package on `BlazorWASM.Backend` and `BlazorServer` projects:

<img width="850" alt="image" src="https://github.com/akhanalcs/blazorwasm-cookie-auth/assets/30603497/5a2605e3-8726-4c4a-9361-1e1dccb2a722">

#### Setup connection to Redis
1. Grab location of Redis
   
   <img width="250" alt="image" src="https://github.com/akhanalcs/blazorwasm-cookie-auth/assets/30603497/ce5c70e8-2474-4727-ae8c-05254bcb5604">

2. Plug it in on `BlazorWASM.Backend` and `BlazorServer` projects
   <img width="650" alt="image" src="https://github.com/akhanalcs/blazorwasm-cookie-auth/assets/30603497/0deef712-6479-4107-98dd-fb93a8e59788">

#### Configure `BlazorServer` app to read the same cookie issued by `BlazorWASM.Backend` app
`_Imports.razor` changes:
   
<img width="400" alt="image" src="https://github.com/akhanalcs/blazorwasm-cookie-auth/assets/30603497/0f6e9193-a3e1-4870-b944-133e78439899">

`Auth.razor` changes:
   
<img width="650" alt="image" src="https://github.com/akhanalcs/blazorwasm-cookie-auth/assets/30603497/c127bedf-04ac-4880-b327-1bebca95c92c">
   
`NavMenu.razor` changes:
   
<img width="450" alt="image" src="https://github.com/akhanalcs/blazorwasm-cookie-auth/assets/30603497/01762c86-4132-411e-8bfd-59855f9676e2">

`Routes.razor` changes:
   
<img width="650" alt="image" src="https://github.com/akhanalcs/blazorwasm-cookie-auth/assets/30603497/fa3f338c-8e2e-44ba-8475-9cfe276deb79">

`Program.cs` changes:
   
<img width="650" alt="image" src="https://github.com/akhanalcs/blazorwasm-cookie-auth/assets/30603497/8405d3aa-154a-4728-ae01-94a564e04259">

#### Take it for a test drive
1. Run both of the apps: `BlazorWASM.Backend` and `BlazorServer`.
2. You should be able to see your key in Redis.
   
   <img width="300" alt="image" src="https://github.com/akhanalcs/blazorwasm-cookie-auth/assets/30603497/0abc3c02-cfb8-4aad-b8e6-cb5dac7c54a6">

4. Login to your `BlazorWASM.Backend` app.
   
   <img width="450" alt="image" src="https://github.com/akhanalcs/blazorwasm-cookie-auth/assets/30603497/634eba7a-ecef-4117-9e26-631065d511d8">

6. Go to your `BlazorServer` app's 'Auth Required' page and you should be able to view this page without any problem. 🎉

   <img width="450" alt="image" src="https://github.com/akhanalcs/blazorwasm-cookie-auth/assets/30603497/ad687a37-25e6-43b5-8b63-5c519605be02">

#### Stop Redis after you're done with your testing
Hit `^C` and run this command: `brew services stop redis`.

<img width="650" alt="image" src="https://github.com/akhanalcs/blazorwasm-cookie-auth/assets/30603497/6b90cae9-4249-4a90-9cf3-3defa7e19aff">

