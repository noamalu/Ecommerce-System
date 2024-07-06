using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketBackend.DAL.DTO;
using MarketBackend.Domain.Market_Client;
using MarketBackend.Services.Interfaces;

namespace MarketBackend.DAL
{
    public class ClientRepositoryRAM : IClientRepository
    {
        private static Dictionary<int, Member> IdxMember;

        private static Dictionary<string, Member> UsernamexMember;
        private object Lock;
        private MemberDTO memberDTO;

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

        public static void Dispose(){
            _memberRepository = new ClientRepositoryRAM();
        }

        public async Task<IEnumerable<Member>> getAll()
        {
            return IdxMember.Values;
        }

        public async Task Delete(Member entity)
        {
            await Delete(entity.Id);
        }

        public async Task Add(Member item)
        {
            DBcontext dbContext = DBcontext.GetInstance();
            await dbContext.PerformTransactionalOperationAsync(async () =>
            {
                dbContext.Members.Add(new MemberDTO(item));
            });
            IdxMember.Add(item.Id, item);
            UsernamexMember.Add(item.UserName, item);
        }

        public async Task Delete(int id)
        {
            var dbContext = DBcontext.GetInstance();
            await dbContext.PerformTransactionalOperationAsync(async () =>
            {
                var dbMember = dbContext.Members.Find(id);
                if(dbMember is not null) {
                dbContext.Members.Remove(dbMember);
                if (IdxMember.ContainsKey(id))
                {
                    Member member = IdxMember[id];
                    UsernamexMember.Remove(member.UserName);
                    IdxMember.Remove(id);
                }
                }
            });
        }
        

        public async Task<List<Member>> GetAll()
        {
            Load();
            return IdxMember.Values.ToList();
        }

        public async Task<Member> GetById(int id)
        {
            if (IdxMember.ContainsKey(id))
                return IdxMember[id];
            else
            {
                var dbContext = DBcontext.GetInstance();
                await dbContext.PerformTransactionalOperationAsync(async () =>
                {
                    memberDTO = dbContext.Members.Find(id);
                });
                if (memberDTO != null)
                {
                    LoadMember(memberDTO);
                    return IdxMember[id];
                }
                throw new ArgumentException("Invalid user ID.");
            }
        }

        public async Task Update(Member item)
        {
            if (await ContainsValue(item))
            {
                IdxMember[item.Id] = item;
                UsernamexMember[item.UserName] = item;
            }
        }

        public async Task<Member> GetByUserName(string userName)
        {
            if (UsernamexMember.ContainsKey(userName))
                return UsernamexMember[userName];
            else
            {
                var dbContext = DBcontext.GetInstance();
                await dbContext.PerformTransactionalOperationAsync(async () =>
                {
                    memberDTO = dbContext.Members.FirstOrDefault(m => m.UserName == userName);
                });
                if (memberDTO != null)
                {
                    LoadMember(memberDTO);
                    return UsernamexMember[userName];
                }
                else
                {
                    throw new ArgumentException("Invalid user name.");
                }
            }
        }

        public async Task<bool> ContainsUserName(string userName)
        {
            return UsernamexMember.ContainsKey(userName);
        }

        public async Task<bool> ContainsID(int id)
        {
            return IdxMember.ContainsKey(id);
        }

        public async Task<bool> ContainsValue(Member item)
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

        private async Task Load()
        {
            var dbContext = DBcontext.GetInstance();
            await dbContext.PerformTransactionalOperationAsync(async () =>
            {
                List<MemberDTO> members = dbContext.Members.ToList();
                foreach (MemberDTO member in members)
                {
                    IdxMember.TryAdd(member.Id, new Member(member));
                }
            });
            
        }

        private async Task LoadMember(MemberDTO memberDto)
        {
            Member member = new Member(memberDto);
            IdxMember[member.Id] = member;
            UsernamexMember[member.UserName] = member;
        }
    }
}