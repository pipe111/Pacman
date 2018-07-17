﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using PacMan;
using PacMan.Interfaces;

namespace PacmanWeb.ManagerPacman
{
    public class PacmanHub : Hub
    {
        private readonly Game game;
        private readonly IHubContext<PacmanHub> hubContext;

        public PacmanHub(Game game, IHubContext<PacmanHub> hubContext)
        {
            this.game = game;
            this.hubContext = hubContext;
        }

        private void Game_PacmanIsDied()
        {
        }

        public void Start()
        {
            game.AddMoveHandlerToGhosts(Move);
            game.AddMoveHandlerToPacman(Move);
            game.PacmanIsDied += Game_PacmanIsDied;
            game.Start();
        }

        public void Stop()
        {
            game.Stop();
        }

        private void Move(ICoord coord)
        {
            Task.Run(() => hubContext.Clients.All.SendAsync("Move", coord.Position.X, coord.Position.Y, coord.GetId()));
        }

        public void Update()
        {
            Task.Run(() => hubContext.Clients.All.SendAsync("Init"));
        }
        public void PacmanDirection(string direction)
        {
            switch(direction)
            {
                case "37":
                    game.SetDirection(Direction.Left);
                    break;
                case "38":
                    game.SetDirection(Direction.Up);
                    break;
                case "39":
                    game.SetDirection(Direction.Right);
                    break;
                case "40":
                    game.SetDirection(Direction.Down);
                    break;
                default:
                    break;
            }
        }
    }
}