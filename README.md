# Nexar.Design Demo

[nexar.com]: https://nexar.com/
[nexar-token-cs]: https://github.com/NexarDeveloper/nexar-token-cs
[nexar-token-py]: https://github.com/NexarDeveloper/nexar-token-py

Demo Blazor WebAssembly with design queries powered by Nexar.

Live demo: <https://web-design-demo.nexar.com>

## How to use

If you have not done this already, please register at [nexar.com].

In order to see anything useful in the app, you need your Altium Live
credentials and have to be a member of at least one Altium 365 workspace.

The app requires a token. Go to your [nexar.com] application details, click
"Generate token". Alternatively, you may get a token using one of the tools:
[nexar-token-cs], [nexar-token-py].

Having got a token, copy it to the clipboard and navigate to the app URL. At
the `Connect` page paste the token and click `CONNECT`. The browser remembers
and restores the token on next runs. The token may be used until it expires.

## Building blocks

[Blazor]: https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor
[MudBlazor]: https://github.com/Garderoben/MudBlazor

The app is built with [Blazor] using [MudBlazor] components.

The Design domain data are provided by Nexar API: <https://api.nexar.com/graphql>.
This is the endpoint for GraphQL queries and also "Banana Cake Pop", the GraphQL IDE.

The package [HotChocolate StrawberryShake](https://github.com/ChilliCream/hotchocolate)
is used for generating strongly typed C# client code for invoking GraphQL queries.
See the source queries in [Resources](Nexar.Client/Resources).
