using AdminPortal.Business.Services.Interfaces;
using HotChocolate;
using HotChocolate.Types;

namespace AdminPortal.GraphQL
{
    [ExtendObjectType(OperationTypeNames.Query)]
    public class Query
    {
        public Person GetPerson() => new Person("Luke Skywalker");
    }

    public class Person
    {
        public Person(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
