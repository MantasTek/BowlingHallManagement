using BowlingHallManagement.Factories;
using BowlingHallManagement.Models;
using BowlingHallManagement.Services.Interfaces;

namespace BowlingHallManagement.Services
{
    /// <summary>
    /// Service for handling member-related operations
    /// </summary>
    public class MemberService : IMemberService
    {
        private readonly IDataStorage _dataStorage;

        public MemberService(IDataStorage dataStorage)
        {
            _dataStorage = dataStorage ?? throw new ArgumentNullException(nameof(dataStorage));
        }

        public void RegisterMember(string name, string email)
        {
            var member = MemberFactory.CreateMember(name, email);
            _dataStorage.AddMember(member);
            _dataStorage.SaveChanges();
        }

        public List<Member> GetAllMembers()
        {
            return _dataStorage.GetAllMembers();
        }

        public Member GetMemberById(int id)
        {
            return _dataStorage.GetMemberById(id);
        }
    }
}
