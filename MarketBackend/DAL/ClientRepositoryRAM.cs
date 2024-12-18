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

        public IEnumerable<Member> getAll()
        {
            Load();
            return IdxMember.Values;
        }

        public void Delete(Member entity)
        {
            Delete(entity.Id);
        }

        public void Add(Member item)
        {
            DBcontext dbContext = DBcontext.GetInstance();
            IdxMember.Add(item.Id, item);
            UsernamexMember.Add(item.UserName, item);
            try{
                lock (Lock)
                {
                    dbContext.Members.Add(new MemberDTO(item));
                    
                    dbContext.SaveChanges();
                }
            }
            catch(Exception){
                throw new Exception("There was a problem in Database use- Add Member");
            }
            

        }

        public void Delete(int id)
        {
            try{
                lock (Lock)
                {
                    var dbContext = DBcontext.GetInstance();
                    var dbMember = dbContext.Members.Find(id);
                    if(dbMember is not null) {
                        if (IdxMember.ContainsKey(id))
                        {
                            Member member = IdxMember[id];
                            UsernamexMember.Remove(member.UserName);
                            IdxMember.Remove(id);
                        }

                        dbContext.Members.Remove(dbMember);
                        dbContext.SaveChanges();
                    }
                }
            }
            catch(Exception){
                throw new Exception("There was a problem in Database use- Delete Member");
            }
            
        }
        public List<Member> GetAll()
        {
            Load();
            return IdxMember.Values.ToList();
        }

        public Member GetById(int id)
        {
            if (IdxMember.ContainsKey(id))
                return IdxMember[id];
            else
            {
                try{
                    var dbContext = DBcontext.GetInstance();
                    MemberDTO mDto = dbContext.Members.Find(id);
                    if (mDto != null)
                    {
                        LoadMember(mDto);
                        return IdxMember[id];
                    }
                    throw new ArgumentException("Invalid user ID.");
                }
                catch(Exception){
                throw new Exception("There was a problem in Database use- Get Member");
                }
            }
        }

        public void Update(Member item)
        {
            if (ContainsValue(item))
            {
                IdxMember[item.Id] = item;
                UsernamexMember[item.UserName] = item;
                try{
                    lock (Lock)
                    {
                        MemberDTO p = DBcontext.GetInstance().Members.Find(item.Id);
                        if (p != null){
                            p.Password = item.Password;
                            if (item.alerts != null) {
                                List<MessageDTO> Alerts = new List<MessageDTO>();
                                foreach (Message message in item.alerts)
                                {
                                    Alerts.Add(new MessageDTO(message));
                                }
                                p.Alerts = Alerts;
                            }
                            p.IsNotification = item.IsNotification;
                            if (item.OrderHistory != null){
                                List<ShoppingCartHistoryDTO> OrderHistory = new ();
                                foreach (var order in item.OrderHistory.Values.ToList())
                                OrderHistory.Add(new ShoppingCartHistoryDTO(order));
                                p.OrderHistory = OrderHistory;
                            }
                            p.IsSystemAdmin = item.IsSystemAdmin;
                            DBcontext.GetInstance().SaveChanges();
                        }
                    }
                }
                catch(Exception){
                throw new Exception("There was a problem in Database use- Update Member");
                }
                
            }
            else{
                throw new KeyNotFoundException($"Client with ID {item.Id} not found.");
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
                try{
                    MemberDTO mDto;
                    lock(Lock){
                        var dbContext = DBcontext.GetInstance();
                        mDto = dbContext.Members.FirstOrDefault(m => m.UserName == userName);
                    }
                    if (mDto != null)
                    {
                        LoadMember(mDto);
                        return UsernamexMember[userName];
                    }
                    else
                    {
                        throw new ArgumentException("Invalid user name.");
                    }
                }
                catch(Exception){
                throw new Exception("There was a problem in Database use- Get Member");
                }
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

        private void Load()
        {
            var dbContext = DBcontext.GetInstance();
            List<MemberDTO> members = dbContext.Members.ToList();
            foreach (MemberDTO member in members)
            {
                IdxMember.TryAdd(member.Id, new Member(member));
                UsernamexMember.TryAdd(member.UserName, new Member(member));
            }
        }

        private void LoadMember(MemberDTO memberDto)
        {
            Member member = new Member(memberDto);
            IdxMember[member.Id] = member;
            UsernamexMember[member.UserName] = member;
        }


    }
}