using MarketBackend.Domain.Market_Client;
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
            //return founder id. can't be null cuz there has to be a founder.
            return roles[storeId].Values.First(role => role.getRoleType() == "Founder");
        }

        public Role GetById(params int[] keyValues)
        {
            return roles[keyValues[0]][keyValues[1]];
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
            roles[entity.storeId][entity.memberId] = entity;
        }
        public void Delete(Role entity)
        {
            roles[entity.storeId].Remove(entity.memberId);
        }

        public Dictionary<int, Role> getShopRoles(int storeId)
        {
            return roles[storeId];
        }
    }
}

