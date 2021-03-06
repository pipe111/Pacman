﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using PacMan;
using PacmanWeb.Data;
using PacmanWeb.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using PacmanWeb.Models.GameModels;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace PacmanWeb.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private IConfiguration Configuration { get; }
        private GameCollection GameCollection { get; }
        private ApplicationDbContext Context { get; }
        private IHubContext<PacmanHub> HubContext { get; }
        private IHostingEnvironment HostingEnvironment { get; }

        public GameController(GameCollection gameCollection, IConfiguration configuration,
            ApplicationDbContext context, IHubContext<PacmanHub> hubContext, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            GameCollection = gameCollection;
            Context = context;
            HubContext = hubContext;
            HostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult BlueMap()
        {
            var info = CreateGame("Blue");
            return View(info);
        }

        public IActionResult GreenMap()
        {
            var info = CreateGame("Green");
            return View(info);
        }

        public IActionResult RedMap()
        {
            var info = CreateGame("Red");
            return View(info);
        }

        [AllowAnonymous]
        public IActionResult Records()
        {
            return View(Context.Records.OrderByDescending(model => model.Score));
        }

        private InformationModel CreateGame(string map)
        {
            var id = Guid.NewGuid().ToString();
            var basePath = HostingEnvironment.WebRootPath;
            var mapPath = Configuration.GetSection("AppConfig:Map" + map + "Path").Value;
            var fullPath = Path.Combine(basePath, mapPath);
            var game = new Game(fullPath, map + "Map");
            GameCollection.AddGame(id, new GameConnection(game, HubContext, id));
            return new InformationModel { Widht = game.Map.Widht, Height = game.Map.Height, Id = id };
        }
    }
}