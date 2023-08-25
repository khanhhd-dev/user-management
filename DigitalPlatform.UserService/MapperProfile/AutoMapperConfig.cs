using AutoMapper;

namespace DigitalPlatform.UserService.Api.MapperProfile
{
    public class AutoMapperConfig
    {
        public static IMapper Initialize()
        {
            var config = new MapperConfiguration(cfg =>
            {

            });

            return config.CreateMapper();
        }
    }
}