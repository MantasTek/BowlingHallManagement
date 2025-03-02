using BowlingHallManagement.Models;

namespace BowlingHallManagement.Services.Interfaces
{
    /// <summary>
    /// Interface for member-related operations
    /// </summary>
    public interface IMemberService
    {
        void RegisterMember(string name, string email);
        List<Member> GetAllMembers();
        Member GetMemberById(int id);
    }
}
