# Nexar.Design Demo

Demo Blazor WebAssembly with design queries powered by Nexar.

Live demo: [web-design-demo.nexar.com](https://web-design-demo.nexar.com)

## How to use

In order to see anything useful in the app, you need your Altium Live
credentials and have to be a member of at least one Altium 365 workspace.

[nexar-token-cs]: https://github.com/NexarDeveloper/nexar-token-cs

Get a Nexar Design token. For the moment there is no straightforward way which
is both secure and friendly. Please use the dotnet console app [nexar-token-cs]
and run it without arguments in order to sign in and get the token.

Having got the token, copy it to the clipboard and navigate to the app URL. At
the `Connect` page paste the token and click `Connect`. The browser remembers
and restores the token on next runs. The token may be used many times until it
expires.

## Building blocks

[Blazor]: https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor
[Blazorise]: https://github.com/Megabit/Blazorise

The app is built with [Blazor] using [Blazorise] components.

The Design domain data are provided by Nexar API: <https://api.nexar.com/graphql>.
This is the GraphQL endpoint for queries and also the Banana Cake Pop GraphQL IDE in browsers.

The [HotChocolate StrawberryShake](https://github.com/ChilliCream/hotchocolate) package
is used for generating strongly typed C# client code for invoking GraphQL queries.
See the source queries in [Resources](Nexar.Design/GraphQL/Resources).
