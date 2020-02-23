﻿using System.Collections.Generic;

namespace NotificationService.Hubs
{
    public class ConnectionManager : IConnectionManager
    {
        private static Dictionary<string, HashSet<string>> userMap = new Dictionary<string, HashSet<string>>();

        public IEnumerable<string> OnlineUsers => userMap.Keys;

        public void AddConnection(string username, string connectinoId)
        {
            lock (userMap)
            {
                if (!userMap.ContainsKey(username))
                {
                    userMap[username] = new HashSet<string>();
                }
                userMap[username].Add(connectinoId);
            }
        }

        public HashSet<string> GetConnections(string username)
        {
            var conn = new HashSet<string>();
            try
            {
                lock(userMap)
                {
                    conn = userMap[username];
                }
            }
            catch
            {
                conn = null;
            }
            return conn;
        }

        public void RemoveConnection(string connectionId)
        {
            lock (userMap)
            {
                foreach (var username in userMap.Keys)
                {
                    if (userMap.ContainsKey(username))
                    {
                        if (userMap[username].Contains(connectionId))
                        {
                            userMap[username].Remove(connectionId);
                            break;
                        }
                    }
                }
            }
        }
    }
}
