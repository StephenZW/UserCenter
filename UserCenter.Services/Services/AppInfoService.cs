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
    public class AppInfoService : BaseService<T_AppInfo>, IAppInfoService
    {
        public AppInfoService(UserCenterContext context)
        {
            this.Db = context;
        }
        protected override DbContext Db { get; set; }

        public async Task<AppInfoDTO> GetByAppKeyAsync(string appKey)
        {
            var appInfo = await base.Entities.AsNoTracking().SingleOrDefaultAsync(a => a.AppKey == appKey && !a.IsEnabled);
            return ToDTO(appInfo);
        }

        public async Task<long> AddNewAsync(string name, string appKey)
        {
            var isAny = await this.Entities.AnyAsync(a => a.AppKey == appKey);
            if (isAny)
            {
                throw new InvalidOperationException("该 AppKey 已存在");
            }
            var appSecret = Guid.NewGuid().ToString("N");
            var appInfo = new T_AppInfo()
            {
                CreateDate = DateTime.Now,
                AppKey = appKey,
                AppSecret = appSecret,
                Name = name,
                IsEnabled = false
            };
            base.Entities.Add(appInfo);
            return await this.Db.SaveChangesAsync();

        }

        static AppInfoDTO ToDTO(T_AppInfo appInfo)
        {
            if (appInfo == null)
            {
                return null;
            }
            AppInfoDTO dto = new AppInfoDTO();
            dto.AppKey = appInfo.AppKey;
            dto.AppSecret = appInfo.AppSecret;
            dto.Id = appInfo.Id;
            dto.Name = appInfo.Name;
            dto.IsEnabled = appInfo.IsEnabled;
            return dto;
        }
    }
}
