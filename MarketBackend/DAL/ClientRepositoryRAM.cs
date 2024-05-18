using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketBackend.Domain.Market_Client;
using MarketBackend.Services.Interfaces;

namespace MarketBackend.DAL
{
    public class ClientRepositoryRAM : IClientRepository
    {
        private static Dictionary<int, Member> IdxMember;

        private static Dictionary<string, Member> UsernamexMember;
        private object Lock;

        private static ClientRepositoryRAM _memberRepository = null;

        private ClientRepositoryRAM()
        {
            IdxMember = new Dictionary<int, Member>();
            UsernamexMember = new Dictionary<string, Member>();
            Lock = new object();
        }
        public static ClientRepositoryRAM GetInstance()
        {
            _memberRepository ??= new ClientRepositoryRAM();
            return _memberRepository;
        }

        public IEnumerable<Member> getAll()
        {
            return IdxMember.Values;
        }

        public void Delete(Member entity)
        {
            Delete(entity.Id);
        }

        public void Add(Member item)
        {
            IdxMember.Add(item.Id, item);
            UsernamexMember.Add(item.UserName, item);
            lock (Lock)
            {

            }

        }

        public void Delete(int id)
        {
            lock (Lock)
            {
 
                if (IdxMember.ContainsKey(id))
                {
                    Member member = IdxMember[id];
                    UsernamexMember.Remove(member.UserName);
                    IdxMember.Remove(id);
                }

            }
        }
        public List<Member> GetAll()
        {
            return IdxMember.Values.ToList();
        }

        public Member GetById(int id)
        {
            if (IdxMember.ContainsKey(id))
                return IdxMember[id];
            else
            {
                throw new ArgumentException("Invalid user ID.");
            }
        }

        public void Update(Member item)
        {
            if (ContainsValue(item))
            {
                IdxMember[item.Id] = item;
                UsernamexMember[item.UserName] = item;
            }
            lock (Lock)
            {

            }
        }
        public void SetAsSystemAdmin(Member item)
        {

            lock (Lock)
            {

            }
        }

        public Member GetByUserName(string userName)
        {
            if (UsernamexMember.ContainsKey(userName))
                return UsernamexMember[userName];
            else
            {
                throw new ArgumentException("Invalid user name.");
            }
        }

        public bool ContainsUserName(string userName)
        {
            return UsernamexMember.ContainsKey(userName);
        }

        public bool ContainsID(int id)
        {
            return IdxMember.ContainsKey(id);
        }

        public bool ContainsValue(Member item)
        {
            return IdxMember.ContainsKey(item.Id);
        }

        public void Clear()
        {
            IdxMember.Clear();
            UsernamexMember.Clear();
        }        

        public void ResetDomainData()
        {
            IdxMember = new Dictionary<int, Member>();
            UsernamexMember = new Dictionary<string, Member>();
        }

    }
}