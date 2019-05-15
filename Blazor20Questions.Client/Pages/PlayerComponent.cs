using Blazor.Extensions;
using Blazor20Questions.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Timers;

namespace Blazor20Questions.Client.Pages
{
    public abstract class PlayerComponent : ComponentBase
    {
        [Inject]
        protected HttpClient Http { get; set; }
        [Inject]
        protected IJSRuntime JsRuntime { get; set; }

        [Parameter]
        public Guid Id { get; set; }

        protected GameResponse _game;

        protected bool _error;
        protected string _errorMessage;
        protected HubConnection _connection;

        protected override async Task OnInitAsync()
        {
            try
            {
                _error = false;
                _game = await Http.GetJsonAsync<GameResponse>($"api/Game/{Id}");

                await SetupRefresh();
            }
            catch (Exception e)
            {
                _error = true;
                _errorMessage = e.Message;
            }
        }

        private async Task SetupRefresh()
        {
            if (_game.Complete)
                return;

            // Re-render when game has expired
            var t = new Timer();
            t.Elapsed += (o, e) => StateHasChanged();
            t.Interval = (_game.EndTime - DateTime.UtcNow).TotalMilliseconds;
            t.Start();

            _connection = new HubConnectionBuilder(JsRuntime).WithUrl("/hubs/game").Build();
            _connection.On<string>("UpdateGame", UpdateGame);
            await _connection.StartAsync();
            await _connection.InvokeAsync("Register", Id);
        }

        private Task UpdateGame(string game)
        {
            _game = JsonConvert.DeserializeObject<GameResponse>(game);
            StateHasChanged();
            Console.WriteLine("Updated game");
            return Task.CompletedTask;
        }
    }
}
