// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Reflection.Metadata;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using osu.Framework;
using osu.Framework.Platform;
using osu.Game.Tournament;
using osu.Web;


// normal blazor stuff

// --------

public static partial class Program
{
#if DEBUG
    private const string base_game_name = @"osu-development";
#else
        private const string base_game_name = @"osu";
#endif

    public static async void Main(string[] args)
    {

        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

        await builder.Build().RunAsync().ConfigureAwait(true);

        string gameName = base_game_name;
        bool tournamentClient = false;

        using (DesktopGameHost host = Host.GetSuitableDesktopHost(gameName, new HostOptions { BindIPC = false }))
        {
            if (tournamentClient)
                host.Run(new TournamentGame());
            else
                host.Run(new OsuGameWeb(args));
        }
    }

}
