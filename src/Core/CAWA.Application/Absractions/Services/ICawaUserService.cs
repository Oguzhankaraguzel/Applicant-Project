using CAWA.Application.MethodAnswers;
using CAWA.Application.ViewModels.AdminVM;
using CAWA.Application.ViewModels.ApplicantInformationVM;
using CAWA.Application.ViewModels.AppUserVM;

namespace CAWA.Application.Absractions.Services
{
    public interface ICawaUserService
    {
        /// <summary>
        /// Kullanıcıların başvuru sonucunu sotgulamaları için oluşturulmuş metod
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<CawaUserServiceAnswer> Login(string username);
        /// <summary>
        /// Yönetici girişi için oluşturulmuş metod
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<CawaUserServiceAnswer> Login(string username, string password);
        Task<bool> LogOut();
        /// <summary>
        /// Siteye başvuru yapma amacıyla gelen kullanıcının kayıt işlemi için kullanılan metod
        /// </summary>
        /// <param name="appUserCreateVM"></param>
        /// <param name="inviterUserName"></param>
        /// <returns></returns>
        Task<CawaUserServiceAnswer> CreateUser(AppUserCreateVM appUserCreateVM, string? inviterUserName);
        /// <summary>
        /// Yönetici oluşturmak için kullanılan metod.
        /// inviterName eğer null ise kullanıcı için oluşturulan username değeri kullanılır.
        /// </summary>
        /// <param name="adminCreateVM"></param>
        /// <param name="roleName"></param>
        /// <param name="inviterName">Yöneticinin kullanıcıları davet edeceği özel isim</param>
        /// <returns></returns>
        Task<CawaUserServiceAnswer> CreateAdmin(AdminCreateVM adminCreateVM);
        /// <summary>
        /// Başvuru yapan kullanıcının dosyalarını yüklemek için kullanılan metod
        /// </summary>
        /// <param name="createReference"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<CawaUserServiceAnswer> CreateReference(ApplicantInformationCreateVM createReference, string userName);
        Task<CawaUserServiceAnswer> GetUserWithAllDetails(string username);
        Task<CawaUserServiceAnswer> GetAllUsersWithAllDetails();
        /// <summary>
        /// Yöneticinin davet ettiği kişileri listeleme için kullanılan metod
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<CawaUserServiceAnswer> GetAllUsersWithAllDetailsAdminFilter(string username);
        Task<CawaUserServiceAnswer> GetUser(string username);
        /// <summary>
        /// Kullanıcı başka cihazdan başvurusunu tamamlarsa. İlk cihaza döndüğünde sessiondaki ismi yakalyayıp davet eden bilgisini tutmak için kullanılan metod
        /// </summary>
        /// <param name="username"></param>
        /// <param name="inviterName"></param>
        /// <returns></returns>
        Task<CawaUserServiceAnswer> UpdateUserInviter(string username, string inviterName);
        Task<CawaUserServiceAnswer> CheckUserByUserName(string username);
        Task<CawaUserServiceAnswer> CheckUserByEmail(string email);
        Task<CawaUserServiceAnswer> GetAllUsers();
        Task<CawaUserServiceAnswer> GetAllAdmin(bool includesSuperAdmin = false);
    }
}
