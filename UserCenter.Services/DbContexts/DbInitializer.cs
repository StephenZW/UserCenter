using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserCenter.IServices;
using UserCenter.Services.Entities;

namespace UserCenter.Services
{
    public class DbInitializer
    {
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="groupService"></param>
        /// <param name="appInfoService"></param>
        /// <returns></returns>
        public static async Task Initialize(IUserService userService, IGroupService groupService, IAppInfoService appInfoService)
        {
            if (await userService.UserExistsAsync("9527"))
            {
                return;
            }
            long userId = await userService.AddNewAsync("9527", "周星星", "123");
            long userId2 = await userService.AddNewAsync("9528", "杨幂", "123");
            long userId3 = await userService.AddNewAsync("9529", "科比", "123");

            long groupId = await groupService.AddNewAsync("演员");
            long groupId2 = await groupService.AddNewAsync("导演");
            long groupId3 = await groupService.AddNewAsync("编剧");
            long groupId4 = await groupService.AddNewAsync("监制");

            await groupService.AddUserToGroupAsync(groupId, userId);
            await groupService.AddUserToGroupAsync(groupId2, userId);
            await groupService.AddUserToGroupAsync(groupId3, userId);
            await groupService.AddUserToGroupAsync(groupId4, userId);
            await groupService.AddUserToGroupAsync(groupId, userId2);
            await groupService.AddUserToGroupAsync(groupId3, userId3);

            await appInfoService.AddNewAsync("测试 Key", "test");
        }
    }
}
