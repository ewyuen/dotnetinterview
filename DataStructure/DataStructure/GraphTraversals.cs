using System;
using System.Collections.Generic;

namespace DataStructure
{
    internal class GraphTraversals
    {
        // Sample: Number of Islands (LeetCode 200, BFS & DFS)
        // Time Complexity: O(mn)
        // Space Complexity: O(mn)
        public static int NumIslands(char[][] grid)
        {
            int m = grid.Length, n = grid[0].Length, count = 0;
            var dirs = new[] { (0,1), (1,0), (0,-1), (-1,0) };
            // BFS implementation
            void BFS(int i, int j)
            {
                var q = new Queue<(int,int)>();
                q.Enqueue((i,j));
                grid[i][j] = '0';
                while (q.Count > 0)
                {
                    var (x,y) = q.Dequeue();
                    foreach (var (dx,dy) in dirs)
                    {
                        int nx = x+dx, ny = y+dy;
                        if (nx>=0 && ny>=0 && nx<m && ny<n && grid[nx][ny]=='1')
                        {
                            grid[nx][ny]='0';
                            q.Enqueue((nx,ny));
                        }
                    }
                }
            }
            // DFS implementation
            void DFS(int i, int j)
            {
                if (i < 0 || j < 0 || i >= m || j >= n || grid[i][j] != '1')
                    return;
                grid[i][j] = '0'; // Mark as visited
                foreach (var (dx, dy) in dirs)
                {
                    DFS(i + dx, j + dy);
                }
            }
            for (int i=0;i<m;i++)
                for (int j=0;j<n;j++)
                    if (grid[i][j]=='1') { count++; DFS(i,j); } // Use DFS here; swap with BFS(i,j) to use BFS
            return count;
        }
        // DFS implementation using a HashSet to track visited nodes
        public static int NumIslandsDFSWithHashSet(char[][] grid)
        {
            int m = grid.Length, n = grid[0].Length, count = 0;
            var dirs = new[] { (0,1), (1,0), (0,-1), (-1,0) };
            var visited = new HashSet<(int, int)>();
            void DFS(int i, int j)
            {
                if (i < 0 || j < 0 || i >= m || j >= n || grid[i][j] != '1' || visited.Contains((i, j)))
                    return;
                visited.Add((i, j)); // Mark as visited
                foreach (var (dx, dy) in dirs)
                {
                    DFS(i + dx, j + dy);
                }
            }
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                    if (grid[i][j] == '1' && !visited.Contains((i, j))) { count++; DFS(i, j); }
            return count;
        }
        // Example usage:
        public static void ExampleUsage()
        {
            char[][] grid = {
                new[] {'1','1','1','1','0'},
                new[] {'1','1','0','1','0'},
                new[] {'1','1','0','0','0'},
                new[] {'0','0','0','0','0'}
            };
            Console.WriteLine($"Number of islands: {NumIslands(grid)}");
        }
    }
}
