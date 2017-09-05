using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserCenter.Common;
using UserCenter.DTO;
using UserCenter.IServices;
using UserCenter.Services.Entities;

namespace UserCenter.Services
{
    public class UserService : BaseService<T_User>, IUserService
    {
        public UserService(UserCenterContext context)
        {
            this.Db = context;
        }
        protected override DbContext Db { get; set; }

        public async Task<long> AddNewAsync(string phoneNum, string nickName, string password)
        {
            var isAny = await UserExistsAsync(phoneNum);
            if (isAny)
            {
                throw new InvalidOperationException("无效操作，该手机号码已注册！");
            }
            var salt = Guid.NewGuid().ToString("N");
            var user = new T_User()
            {
                CreateDate = DateTime.Now,
                NickName = nickName,
                PasswordHash = CreatePwd(password, salt),
                PasswordSalt = salt,
                PhoneNum = phoneNum
            };
            base.Entities.Add(user);
            return await this.Db.SaveChangesAsync();

        }

        public async Task<bool> CheckLoginAsync(string phoneNum, string password)
        {
            var user = await base.Entities.AsNoTracking()
                .SingleOrDefaultAsync(u => u.PhoneNum == phoneNum);
            if (user == null)
            {
                return false;
            }

            return CreatePwd(password, user.PasswordSalt) == user.PasswordHash;

        }

        public async Task<UserDTO> GetByIdAsync(long id)
        {
            var user = await base.GetByIdAsync(id);
            return ToDTO(user);
        }

        public async Task<UserDTO> GetByPhoneNumAsync(string phoneNum)
        {
            var user = await base.Entities.AsNoTracking()
                .SingleOrDefaultAsync(u => u.PhoneNum == phoneNum);
            return ToDTO(user);
        }

        public async Task<bool> UserExistsAsync(string phoneNum)
        {
            return await base.Entities.AnyAsync(u => u.PhoneNum == phoneNum);
        }

        private static string CreatePwd(string sourcePwd, string salt)
        {
            if (string.IsNullOrEmpty(sourcePwd) || string.IsNullOrEmpty(salt))
            {
                throw new ArgumentException("参数不能为空！");
            }

            return Algorithm.ToMD5(salt.Substring(2, 10) + sourcePwd);
        }
        private static UserDTO ToDTO(T_User user)
        {
            if (user == null)
            {
                return null;
            }
            UserDTO dto = new UserDTO();
            dto.Id = user.Id;
            dto.NickName = user.NickName;
            dto.PhoneNum = user.PhoneNum;
            return dto;
        }
    }
}
