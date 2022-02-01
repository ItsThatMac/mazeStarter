using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Flags]
public enum WallState
{
    // 0000 --> No walls 
    // 1111 --> Left, right, up, down
    LEFT = 1, // 0001
    RIGHT = 2, // 0010
    UP = 4, // 0100
    DOWN = 8, // 1000

    VISITED = 128, // 1000 0000
}

public struct Position
{
    public int x;
    public int y;
}

public struct Neighbour
{
    public Position Position;
    public WallState SharedWall;
}

public static class mazeGeneratorScript
{

    private static WallState GetOppositeWall(WallState wall)
    {
        switch(wall)
        {
            case WallState.RIGHT: return WallState.LEFT;
            case WallState.LEFT: return WallState.RIGHT;
            case WallState.UP: return WallState.DOWN;
            case WallState.DOWN: return WallState.UP;
            default: return WallState.LEFT;
        }
    }

    private static WallState[,] ApplyRecursiveBackTracker(WallState[,] maze, int width, int height)
    {
        var rng = new System.Random();
        var positionStack = new Stack<Position>();
        var position = new Position { x = rng.Next(0, width), y = rng.Next(0, height) };

        maze[position.x, position.y] |= WallState.VISITED; // 1000 1111
        positionStack.Push(position);

        while (positionStack.Count > 0)
        {
            var current = positionStack.Pop();
            var neighbours = GetUnvisitedNeighbours(current, maze, width, height);

            if (neighbours.Count > 0)
            {
                positionStack.Push(current);

                var randIndex = rng.Next(0, neighbours.Count);
                var randomNeighbour = neighbours[randIndex];

                var nPosition = randomNeighbour.Position;
                maze[current.x, current.y] &= ~randomNeighbour.SharedWall;
                maze[nPosition.x, nPosition.y] &= ~GetOppositeWall(randomNeighbour.SharedWall);

                maze[nPosition.x, nPosition.y] |= WallState.VISITED;

                positionStack.Push(nPosition);
            }
        }

        return maze;
    }

    private static List<Neighbour> GetUnvisitedNeighbours(Position p, WallState[,] maze, int width, int height)
    {
        var list = new List<Neighbour>(0);

        if(p.x >0) // left
        {
            if(!maze[p.x - 1, p.y].HasFlag(WallState.VISITED))
            {
                list.Add(new Neighbour
                {
                    Position = new Position
                    {
                        x = p.x - 1,
                        y = p.y
                    },
                    SharedWall = WallState.LEFT
                });
            }
        }

        if (p.y > 0) // DOWN
        {
            if (!maze[p.x, p.y - 1].HasFlag(WallState.VISITED))
            {
                list.Add(new Neighbour
                {
                    Position = new Position
                    {
                        x = p.x,
                        y = p.y - 1
                    },
                    SharedWall = WallState.DOWN
                });
            }
        }

        if (p.y < height -1) // UP
        {
            if (!maze[p.x, p.y + 1].HasFlag(WallState.VISITED))
            {
                list.Add(new Neighbour
                {
                    Position = new Position
                    {
                        x = p.x,
                        y = p.y + 1
                    },
                    SharedWall = WallState.UP
                });
            }
        }

        if (p.x < width - 1) // left
        {
            if (!maze[p.x + 1, p.y].HasFlag(WallState.VISITED))
            {
                list.Add(new Neighbour
                {
                    Position = new Position
                    {
                        x = p.x + 1,
                        y = p.y
                    },
                    SharedWall = WallState.RIGHT
                });
            }
        }

        return list;
    }

    public static WallState[,] Generate(int width, int height)
    {
        WallState[,] maze = new WallState[width, height];
        WallState initial = WallState.RIGHT | WallState.LEFT | WallState.UP | WallState.DOWN;

        for (int i=1; i < width - 1; ++i)
        {
            for (int j=1; j<height - 1; ++j)
            {
                maze[i, j] = initial; // 1111
            }
        }



        return ApplyRecursiveBackTracker(maze, width, height);
    }
}
