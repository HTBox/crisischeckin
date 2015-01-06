using System;
using System.Collections.Generic;
using Models;

namespace Services.Interfaces
{
    public interface IAdmin
    {
        IEnumerable<Person> GetVolunteers(Disaster disaster);
        IEnumerable<Person> GetVolunteersForDate(Disaster disaster, DateTime date, bool clusterCoordinatorsOnly);
        IEnumerable<Person> GetVolunteersForDate(int disasterId, DateTime date, bool clusterCoordinatorsOnly);
        IEnumerable<Person> GetVolunteersForDisaster(int disasterId, DateTime? commitmentDate);
    }
}
