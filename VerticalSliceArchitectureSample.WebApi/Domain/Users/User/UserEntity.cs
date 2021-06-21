using Corex.Model.Infrastructure;
using System;
using VerticalSliceArchitectureSample.WebApi.Results;

namespace VerticalSliceArchitectureSample.WebApi.Domain.Users.User
{

    public class UserEntity : BaseGuidKeyEntity
    {
        public UserEntity()
        {

        }
        public string Name { get; private set; }
        public string Surname { get; private set; }
        public string Password { get; private set; }
        public string Email { get; private set; }
        public string Address { get; private set; }
        public static IResultObjectModel<UserEntity> Create(string email, string password)
        {
            UserEntity userEntity = new UserEntity
            {
                Email = email,
                Password = password
            };
            return new ResultObjectModel<UserEntity>(userEntity);
        }
    }
}
