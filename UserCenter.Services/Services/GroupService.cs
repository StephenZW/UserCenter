using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserCenter.DTO;
using UserCenter.IServices;
using UserCenter.Services.Entities;

namespace UserCenter.Services
{
    public class GroupService : BaseService<T_Group>, IGroupService
    {
        protected override DbContext Db { get; set; }

        public GroupService(UserCenterContext context)
        {
            this.Db = context;
        }

        public async Task<GroupDTO> GetByIdAsync(long id)
        {
            var group = await base.GetByIdAsync(id);
            return ToDTO(group);
        }

        public async Task<GroupDTO[]> GetGroupsAsync(long userId)
        {
            List<T_Group> groupList = await this.Db.Set<T_User>()
                   .Include(nameof(T_User) + "." + nameof(T_User.UserGroup))
                   .Include(nameof(T_UserGroup) + "." + nameof(T_UserGroup.Group))
                   .AsNoTracking()
                   .Where(u => u.Id == userId)
                   .SelectMany(t => t.UserGroup.Select(p => p.Group))
                   .ToListAsync();
            return groupList.Select(t => ToDTO(t)).ToArray();

        }

        public async Task<UserDTO[]> GetGroupUsersAsync(long groupId)
        {
            List<T_User> userList = await this.Db.Set<T_Group>()
                  .Include(nameof(T_Group) + "." + nameof(T_User.UserGroup))
                  .Include(nameof(T_UserGroup) + "." + nameof(T_UserGroup.User))
                  .AsNoTracking()
                  .Where(g => g.Id == groupId)
                  .SelectMany(t => t.UserGroup.Select(p => p.User))
                  .ToListAsync();
            List<UserDTO> dtos = new List<UserDTO>();
            foreach (var user in userList)
            {
                UserDTO userDto = new UserDTO();
                userDto.Id = user.Id;
                userDto.NickName = user.NickName;
                userDto.PhoneNum = user.PhoneNum;
                dtos.Add(userDto);
            }
            return dtos.ToArray();
        }

        public async Task AddUserToGroupAsync(long groupId, long userId)
        {
            T_UserGroup userGroup = new T_UserGroup
            {
                UserId = userId,
                GroupId = groupId,
                CreateDate = DateTime.Now
            };
            this.Db.Set<T_UserGroup>().Add(userGroup);
            await this.Db.SaveChangesAsync();
        }

        public async Task RemoveUserFromGroupAsync(long groupId, long userId)
        {
            var userGroups = this.Db.Set<T_UserGroup>();
            var userGroup = await userGroups
                 .SingleOrDefaultAsync(ug => ug.UserId == userId && ug.GroupId == groupId);
            if (userGroup!=null)
            {
                userGroups.Remove(userGroup);
                await this.Db.SaveChangesAsync();
            }
        }
        private static GroupDTO ToDTO(T_Group group)
        {
            if (group == null)
            {
                return null;
            }
            GroupDTO dto = new GroupDTO();
            dto.Id = group.Id;
            dto.Name = group.Name;
            return dto;
        }

    }
}
