﻿using Microsoft.Extensions.Hosting;
using TodoApp.ConsoleApp.Framework;

namespace TodoApp.ConsoleApp.Test.ViewModels
{
    public class Goodbye: ViewModel
    {
        IHostApplicationLifetime AppLifetime;

        public Goodbye(IHostApplicationLifetime appLifetime)
        {
            AppLifetime = appLifetime;
        }

        public void Exit()
        {
            
            AppLifetime.StopApplication();
        }
    }
}
