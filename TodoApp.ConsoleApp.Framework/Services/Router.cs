﻿using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TodoApp.ConsoleApp.Framework.Services
{
    public class Router
    {
        private readonly ILogger<Router> Logger;
        private readonly IServiceProvider ServiceProvider;
        private readonly Props<IProps> Props;

        private View Active;
        private readonly RouteList History = new RouteList();
        private readonly RouteList Future = new RouteList();

        internal delegate void RouteChangedHanlder(Router r, RouteChangedEventArgs args);
        internal event RouteChangedHanlder RouteChanged;

        public Router(ILogger<Router> logger, IServiceProvider serviceProvider, Props<IProps> props)
        {
            Logger = logger;
            ServiceProvider = serviceProvider;
            Props = props;
        }

        private void NotifyRouteChanged()
        {
            Logger.LogInformation("Open View: {0}", Active.GetType().FullName);

            RouteChanged?.Invoke(this, new RouteChangedEventArgs(Active));
        }

        private async Task NotifyRouteChangedAsync()
        {
            await Task.Run(NotifyRouteChanged);
        }

        private TView CreateView<TView>()
            where TView : View
        {
            Props.Data = default;

            var view = ServiceProvider.GetRequiredService<TView>();
            return view;
        }

        private TView CreateView<TView, TProps>(TProps props)
            where TView : View
            where TProps : IProps
        {
            Props.Data = props;

            var view = ServiceProvider.GetRequiredService<TView>();
            return view;
        }

        public async void Open<TView>()
            where TView : View
        {
            History.Push(Active);
            Active = CreateView<TView>();
            Future.Clear();

            await NotifyRouteChangedAsync();
        }

        public async void Open<TView, TProps>(TProps props)
            where TView : View
            where TProps : IProps
        {
            History.Push(Active);
            Active = CreateView<TView, TProps>(props);
            Future.Clear();

            await NotifyRouteChangedAsync();
        }

        public bool CanGoTo(int count)
        {
            return
                count != 0 &&
                (
                  (count < 0 && Math.Abs(count) <= History.Count) ||
                  (count > 0 && count <= Future.Count)
                );
        }

        public async void GoTo(int count)
        {
            for (int i = count; i < 0; i++)
            {
                Future.Push(Active);
                Active = History.Pop();
            }

            for (int i = count; i > 0; i++)

            {
                History.Push(Active);
                Active = Future.Pop();
            }

            await NotifyRouteChangedAsync();
        }

        internal async void Start(View homeView)
        {
            Active = homeView;
            await NotifyRouteChangedAsync();
        }

        internal void Stop()
        {
            History.Clear();
            Future.Clear();
        }
    }

    internal class RouteChangedEventArgs : EventArgs
    {
        public View View { get; }

        public RouteChangedEventArgs(View v)
        {
            View = v;
        }
    }
}
