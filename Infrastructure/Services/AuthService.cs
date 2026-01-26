using Application;
using Application.IServices;
using AutoMapper;
using Domain;

namespace Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _uOK;
        private readonly AppSettings _appSettings;
        private readonly IMapper _mapper;
        //private readonly 

    }
}
