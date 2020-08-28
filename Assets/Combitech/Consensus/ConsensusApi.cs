using Assets.Combitech.Authentication;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using UnityEngine;
using Wasp.Consensus.Domain.Interfaces;
using Wasp.Consensus.Domain.Models;
using Wasp.Consensus.Shared.Serialization;

namespace Assets.Combitech.Consensus
{
    public delegate void Flush();

    public delegate void SelectionUpdate(Selection selection);
    public delegate void FocusUpdate(BaseEntity entity);
    public delegate void UnitUpdate(Unit unit);

    public class ConsensusApi : MonoBehaviour
    {
        public static ConsensusApi Instance { get; private set; }

        public event Flush OnFlush;

        public event SelectionUpdate OnSelectionUpdate;
        public event FocusUpdate OnFocusUpdate;
        public event UnitUpdate OnUnitCreated;
        public event UnitUpdate OnUnitUpdated;
        public event UnitUpdate OnUnitRemoved;

        private HubConnection _connection;

        static ConsensusApi()
        {
            JsonConvert.DefaultSettings = SerializationSettings.GetJsonSettings;
        }

        private void Awake()
        {
            if (Instance != null)
            {
                DestroyImmediate(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
            Instance = this;
        }

        private void Start()
        {
            AuthManager.Instance.OnAuthUpdate += async (status) =>
            {
                Debug.Log($"OnAuthUpdate {status}");
                if(status == AuthStatus.Authenticated)
                {
                    await Connect();
                }
            };
        }

        private async void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                await Connect();
            }
        }

        private async void OnDestroy()
        {
            if (_connection != null)
            {
                await _connection.StopAsync();
            }
        }

        private async Task Connect()
        {
            if (_connection == null)
            {
                _connection = new HubConnectionBuilder()
                    .WithUrl("https://consensus.waraps.org/frontend", options =>
                    {
                        options.AccessTokenProvider = async () => await Task.FromResult(AuthManager.Instance.AccessToken);
                    })
                    .WithAutomaticReconnect()
                    .AddNewtonsoftJsonProtocol(options =>
                    {
                        options.PayloadSerializerSettings = SerializationSettings.GetJsonSettings();
                    })
                    .Build();

                _connection.On<Selection>(nameof(IFrontendClient.SelectionUpdated), HandleSelectionUpdated);
                _connection.On<BaseEntity>(nameof(IFrontendClient.FocusUpdated), HandleFocusUpdated);
                _connection.On<Unit>(nameof(IFrontendClient.UnitAdded), HandleUnitCreated);
                _connection.On<Unit>(nameof(IFrontendClient.UnitUpdated), HandleUnitUpdated);
                _connection.On<Unit>(nameof(IFrontendClient.UnitRemoved), HandleUnitRemoved);
            }

            if (_connection.State == HubConnectionState.Disconnected)
            {
                await _connection.StartAsync();
            }

            await Synchronize();
        }

        private async Task Synchronize()
        {
            Debug.Log("Synchronizing...");
            OnFlush?.Invoke();
            await _connection.InvokeAsync("Synchronize", TimeSpan.FromHours(24).ToString());
        }

        #region Downstream

        private Task HandleSelectionUpdated(Selection selection)
        {
            OnSelectionUpdate?.Invoke(selection);
            return Task.CompletedTask;
        }

        private Task HandleFocusUpdated(BaseEntity entity)
        {
            OnFocusUpdate?.Invoke(entity);
            return Task.CompletedTask;
        }

        private Task HandleUnitCreated(Unit unit)
        {
            OnUnitCreated?.Invoke(unit);
            return Task.CompletedTask;
        }

        private Task HandleUnitUpdated(Unit unit)
        {
            OnUnitUpdated?.Invoke(unit);
            return Task.CompletedTask;
        }

        private Task HandleUnitRemoved(Unit unit)
        {
            OnUnitRemoved?.Invoke(unit);
            return Task.CompletedTask;
        }

        #endregion

        #region Upstream

        public async Task Select(BaseEntity entity)
        {
            if (entity != null)
            {
                await _connection.InvokeAsync(nameof(IFrontendService.Select), entity, SelectionType.Replace);
            }
            else
            {
                await _connection.InvokeAsync(nameof(IFrontendService.Select), null, SelectionType.Clear);
            }
        }

        #endregion
    }
}
