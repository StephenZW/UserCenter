using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserCenter.DTO;

namespace UserCenter.IServices
{
    public interface IGroupService : IServiceTag
    {
        Task<long> AddNewAsync(string name);
        Task<GroupDTO> GetByIdAsync(long id);
        Task<GroupDTO[]> GetGroupsAsync(long userId);
        Task<UserDTO[]> GetGroupUsersAsync(long userGroupId);

        Task AddUserToGroupAsync(long userGroupId, long userId);
        Task RemoveUserFromGroupAsync(long userGroupId, long userId);
    }
}
