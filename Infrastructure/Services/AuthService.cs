using Application;
using Application.IServices;
using AutoMapper;
using Domain;
using Domain.DTOs;
using Domain.Entities;
using Domain.Requests.User;
using Domain.Responses;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppSettings _appSettings;
        private readonly IMapper _mapper;
        private readonly IFirebaseStorageService _firebaseStorageService;

        public AuthService(IUnitOfWork unitOfWork, AppSettings appSettings, IMapper mapper, IFirebaseStorageService firebaseStorageService)
        {
            _unitOfWork = unitOfWork;
            _appSettings = appSettings;
            _mapper = mapper;
            _firebaseStorageService = firebaseStorageService;
        }

        public async Task<ApiResponse> LoginAsync(LoginRequest request)
        {
            ApiResponse response = new ApiResponse();
            var account = await _unitOfWork.Users.GetAsync(u => u.Email == request.Email);
            if (account == null || !VerifyPasswordHash(request.Password, account.PasswordHash, account.PasswordSalt))
            {
                response.SetBadRequest(message: "Email or password is wrong");
                return response;
            }

            if (account.IsVerfied == false)
            {
                response.SetBadRequest(message: "Please Verify your email");
                return response;
            }
            response.SetOk(CreateToken(account));
            return response;
        }
        public async Task<ApiResponse> RegisterAsync(RegisterRequest request)
        {
            ApiResponse response = new ApiResponse();
            try
            {

                var checkPassword = CheckUserPassword(request);
                if (!checkPassword)
                {
                    response.SetBadRequest(message: "Confirm password is wrong !");
                    return response;
                }
                var existingUser = await _unitOfWork.Users.GetAsync(x => x.Email == request.Email);
                if (existingUser != null)
                {
                    response.SetBadRequest(message: "The email address is already register");
                    return response;
                }

                var pass = CreatePasswordHash(request.Password);
                if (request.Role == "Admin") return response.SetBadRequest("Role must be specified");

                User user = new User()
                {
                    PasswordHash = pass.PasswordHash,
                    PasswordSalt = pass.PasswordSalt,
                    Email = request.Email,
                    FullName = request.FullName,
                    IsVerfied = true,
                    PhoneNumber = request.PhoneNumber
                };
                if (request.ImageFile != null)
                {
                    user.Image = await _firebaseStorageService.UploadUserImage(user.UserId.ToString(), request.ImageFile);
                }
                await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.SaveChangeAsync();

                return response.SetOk(user);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest($"Error: {ex.Message}. Details: {ex.InnerException?.Message}");
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim("Role", user.Role.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim( "Email" , user.Email!),
                new Claim("UserId", user.UserId.ToString()),
            };

          
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                 _appSettings!.SecretToken));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
        private PasswordDTO CreatePasswordHash(string password)
        {
            PasswordDTO pass = new PasswordDTO();
            using (var hmac = new HMACSHA512())
            {
                pass.PasswordSalt = hmac.Key;
                pass.PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
            return pass;
        }
        public bool CheckUserPassword(RegisterRequest request)
        {
            if (request.Password is null) return (false);
            return (request.Password.Equals(request.ConfirmPassword));
        }
    }
}
