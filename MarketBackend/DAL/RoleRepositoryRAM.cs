using MarketBackend.DAL.DTO;
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
        public ConcurrentDictionary<int, ConcurrentDictionary<string, Role>> roles; //<storeId, <memberId, Role>>
        private static RoleRepositoryRAM roleRepositoryRAM = null;

        private object _lock;
        

        private RoleRepositoryRAM()
        {
            roles = new ConcurrentDictionary<int, ConcurrentDictionary<string, Role>>();
            _lock = new object();
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

        public Role GetById(int storeId, string userName)
        {
            if (!roles.ContainsKey(storeId) && roles[storeId].ContainsKey(userName)){
                lock (_lock)
                {
                    RoleDTO roleDTO = DBcontext.GetInstance().Roles.Find(storeId, userName);
                    if (roleDTO != null)
                    {
                        Role role = new Role(roleDTO);
                        if (!roles.ContainsKey(storeId))
                        {
                            roles[storeId] = new ConcurrentDictionary<string, Role>();
                            roles[storeId][userName] = role;
                        }
                        else{
                            roles[storeId].TryAdd(userName, role);
                        }
                    }
                    else
                    {
                        throw new KeyNotFoundException($"member with ID {userName} at store with ID {storeId} not found.");
                    }
                }
            }
            return roles[storeId][userName];
        }
        public void Add(Role entity)
        {
            if (!roles.ContainsKey(entity.storeId))
            {
                roles[entity.storeId] = new ConcurrentDictionary<string, Role>();
                roles[entity.storeId][entity.userName] = entity;
            }
            else{
                roles[entity.storeId].TryAdd(entity.userName, entity);
            }
            lock (_lock)
                {
                    DBcontext.GetInstance().Roles.Add(new RoleDTO(entity));
                    DBcontext.GetInstance().SaveChanges();
                }

        }
        public IEnumerable<Role> getAll()
        {
            return roles.Values.SelectMany(innerDict => innerDict.Values).ToList();
        }
        public void Update(Role entity)
        {
            if(!roles.ContainsKey(entity.storeId) && roles[entity.storeId].ContainsKey(entity.userName))
                throw new KeyNotFoundException($"member with ID {entity.userName} at store with ID {entity.storeId} not found.");
            
            roles[entity.storeId][entity.userName] = entity;
        }
        public void Delete(Role entity)
        {
            if (!roles.ContainsKey(entity.storeId) && roles[entity.storeId].ContainsKey(entity.userName))
                throw new KeyNotFoundException($"member with ID {entity.userName} at store with ID {entity.storeId} not found.");
            roles[entity.storeId].TryRemove(new KeyValuePair<string, Role>(entity.userName, entity));
        }

        public ConcurrentDictionary<string, Role> getShopRoles(int storeId)
        {
            if (!roles.ContainsKey(storeId))
                // throw new KeyNotFoundException($"store with ID {storeId} not found.");
                return new ConcurrentDictionary<string, Role>();
            return roles[storeId];
        }
    }
}

