﻿using MarketBackend.Domain.Market_Client;
using MarketBackend.Domain.Models;
using MarketBackend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketBackend.DAL
{
    public class RoleRepositoryRAM : IRoleRepository
    {
        Dictionary<int, Dictionary<int, Role>> roles; //<storeId, <memberId, Role>>

        public RoleRepositoryRAM()
        {
            roles = new Dictionary<int, Dictionary<int, Role>>();
        }
        public Role GetById(int storeId)
        {
            if(!roles.ContainsKey(storeId))
                throw new KeyNotFoundException($"store with ID {storeId} not found.");

            return roles[storeId].Values.First(role => role.getRoleType() == "Founder");
        }

        public Role GetById(int storeId, int memberId)
        {
            if (!roles.ContainsKey(storeId) && roles[storeId].ContainsKey(memberId))
                throw new KeyNotFoundException($"member with ID {memberId} at store with ID {storeId} not found.");

            return roles[storeId][memberId];
        }
        public void Add(Role entity)
        {
            if (!roles.ContainsKey(entity.storeId))
            {
                roles[entity.storeId] = new Dictionary<int, Role>();
                roles[entity.storeId][entity.memberId] = entity;
            }
            else
                roles[entity.storeId].Add(entity.memberId, entity);
        }
        public IEnumerable<Role> getAll()
        {
            return roles.Values.SelectMany(innerDict => innerDict.Values).ToList();
        }
        public void Update(Role entity)
        {
            if(!roles.ContainsKey(entity.storeId) && roles[entity.storeId].ContainsKey(entity.memberId))
                throw new KeyNotFoundException($"member with ID {entity.memberId} at store with ID {entity.storeId} not found.");
            
            roles[entity.storeId][entity.memberId] = entity;
        }
        public void Delete(Role entity)
        {
            if (!roles.ContainsKey(entity.storeId) && roles[entity.storeId].ContainsKey(entity.memberId))
                throw new KeyNotFoundException($"member with ID {entity.memberId} at store with ID {entity.storeId} not found.");
            roles[entity.storeId].Remove(entity.memberId);
        }

        public Dictionary<int, Role> getShopRoles(int storeId)
        {
            if (!roles.ContainsKey(storeId))
                throw new KeyNotFoundException($"store with ID {storeId} not found.");
            return roles[storeId];
        }
    }
}
