using Models;

namespace Services.Api.Dtos
{
    public class DtoFactory
    {
        public ClusterDto Create(Cluster c)
        {
            return new ClusterDto
            {
                Id = c.Id,
                Name = c.Name
            };
        }
    }
}
