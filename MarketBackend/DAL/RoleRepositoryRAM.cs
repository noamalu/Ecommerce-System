using MarketBackend.Domain.Market_Client;
using MarketBackend.Domain.Models;
using MarketBackend.Services.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketBackend.DAL
{
    public class RoleRepositoryRAM : IRoleRepository
    {
        public ConcurrentDictionary<int, ConcurrentDictionary<int, Role>> roles; //<storeId, <memberId, Role>>
        private static RoleRepositoryRAM roleRepositoryRAM = null;
        

        private RoleRepositoryRAM()
        {
            roles = new ConcurrentDictionary<int, ConcurrentDictionary<int, Role>>();
        }

        public static RoleRepositoryRAM GetInstance()
        {
            if (roleRepositoryRAM == null)
                roleRepositoryRAM = new RoleRepositoryRAM();
            return roleRepositoryRAM;
        }

        public static void Dispose(){
            roleRepositoryRAM = new RoleRepositoryRAM();
        }

        public Role GetById(int storeId)
        {
            if(!roles.ContainsKey(storeId))
                throw new KeyNotFoundException($"store with ID {storeId} not found.");

            return roles[storeId].Values.First(role => role.getRoleName() == RoleName.Founder);
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
                roles[entity.storeId] = new ConcurrentDictionary<int, Role>();
                roles[entity.storeId][entity.memberId] = entity;
            }
            else
                roles[entity.storeId].TryAdd(entity.memberId, entity);
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
            roles[entity.storeId].TryRemove(new KeyValuePair<int, Role>(entity.memberId, entity));
        }

        public ConcurrentDictionary<int, Role> getShopRoles(int storeId)
        {
            if (!roles.ContainsKey(storeId))
                // throw new KeyNotFoundException($"store with ID {storeId} not found.");
                return new ConcurrentDictionary<int, Role>();
            return roles[storeId];
        }
    }
}

